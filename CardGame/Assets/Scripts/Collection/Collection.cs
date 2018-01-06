﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Collection : MonoBehaviour {

    public Card card;
    public float startTransformX;
    public float startTransformY;
    public float transformX;
    public float transformY;
    public string[] ids;
    private FirebaseAuth firebaseAuth;

    public InputField emailInput;
    public InputField passwordInput;
    public GameObject loginButton;
    public GameObject singUpButton;
    
    void Start () {
        StartCoroutine(getCollection());
        StartCoroutine(getUserCollection());
        Vector3 position = transform.position;
        position.y = position.y + startTransformY;
        position.x = position.x - startTransformX;
        position.z = 0;
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                Card newCard = Instantiate(card, position, card.transform.rotation) as Card;
                newCard.Id = ids[y*3+x];
                newCard.Strength = Random.Range(1, 20);
                newCard.changeStrength(newCard.Strength);
                newCard.sprites[0] = Resources.Load("sprites/Cards/SmallSize/" + newCard.Id, typeof(Sprite)) as Sprite;
                newCard.sprites[1] = Resources.Load("sprites/Cards/FullSize/" + newCard.Id, typeof(Sprite)) as Sprite;
                newCard.changeSprite(0);
                newCard.transform.position = position;
                newCard.transform.localScale = new Vector3(1.3F, 1.3F, 0);
                   
                position.x = position.x + transformX;
            }
            position.x = transform.position.x - startTransformX;
            position.y = position.y - transformY;
        }
    }
    
    IEnumerator getCollection()
    {
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/collection/cards", "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send();
        if (request.isError)
        {
            Debug.Log(request.error);
        }
        else
        {
            string jsonString = JsonHelper.fixJson(request.downloadHandler.text);        
            CardBean[] collection = JsonHelper.FromJson<CardBean>(jsonString);
            foreach (CardBean card in collection)
            {
                Debug.Log(card.name);
            }
        }
    }

    IEnumerator getUserCollection()
    {
        User user = new User();
        user.email = PlayerPrefs.GetString("LoginUser", "Unknown");;
        string jsonToServer = JsonUtility.ToJson(user);
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/collection/userCards", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonToServer);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send(); 
        if(request.isError) {
            Debug.Log(request.error);
        }
        else
        {
            string jsonString = JsonHelper.fixJson(request.downloadHandler.text);        
            CardBean[] cards = JsonHelper.FromJson<CardBean>(jsonString);
            Dictionary<CardBean, int> playerCards = new Dictionary<CardBean, int>();

            Dictionary<CardBean, int> notIniqCards = cards
                .GroupBy(n => n, (n, m) => new {Key = n, Cnt = m.Count()})
                .Where(n => n.Cnt > 1)
                .ToDictionary(n => n.Key, n => n.Cnt);

            Dictionary<CardBean, int> uniqCards = cards
                .GroupBy(n => n, (n, m) => new { Key = n, Cnt = m.Count() })
                .Where(n => n.Cnt == 1)
                .ToDictionary(n => n.Key, n => n.Cnt);
            
            playerCards = uniqCards.Union(notIniqCards).ToDictionary(n => n.Key, n => n.Value);
            
            foreach (KeyValuePair<CardBean, int> keyValue in playerCards)
            {
                Debug.Log(keyValue.Key.name + " - " + keyValue.Value);
            }
        }
    }
}
