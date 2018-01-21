using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Collection : MonoBehaviour
{

    public Card card;
    public float startTransformX;
    public float startTransformY;
    public float transformX;
    public float transformY;
    Dictionary<CardBean, int> playerCards;
    CardBean[] collection;

    void Start()
    {
        StartCoroutine(getCollection());
    }

    IEnumerator getCollection()
    {
        UnityWebRequest request = new UnityWebRequest("https://cardgamejavaserver.herokuapp.com/collection/cards", "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send();
        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            string jsonString = JsonHelper.fixJson(request.downloadHandler.text);
            collection = JsonHelper.FromJson<CardBean>(jsonString);
            int count = 1;
            Vector3 position = transform.position;
            position.y = position.y + startTransformY;
            position.x = position.x - startTransformX;
            position.z = 0;

            foreach (CardBean cardInColl in collection)
            {
                Card newCard = Instantiate(card, position, card.transform.rotation) as Card;
                newCard.name = cardInColl.id;
                newCard.gameObject.AddComponent<CardInColl>();
                newCard.Id = cardInColl.id;
                newCard.Strength = cardInColl.strength;
                newCard.changeStrength(newCard.Strength);
                newCard.sprites[0] = Resources.Load("sprites/Cards/SmallSize/" + newCard.Id, typeof(Sprite)) as Sprite;
                newCard.sprites[1] = Resources.Load("sprites/Cards/FullSize/" + newCard.Id, typeof(Sprite)) as Sprite;
                newCard.GetComponentInChildren<SpriteRenderer>().color = new Color32(100, 100, 100, 255);
                newCard.changeSprite(0);
                newCard.transform.position = position;
                newCard.transform.localScale = new Vector3(1.4F, 1.4F, 0);
                newCard.transform.parent = GameObject.Find("Content").transform;
                if (count % 3 == 0)
                {
                    position.y = position.y - transformY;
                    position.x = transform.position.x - startTransformX;
                }
                else
                {
                    position.x = position.x + transformX;
                }
                count++;
            }
            StartCoroutine(getUserCollection());
        }
    }

    IEnumerator getUserCollection()
    {
        User user = new User();
        user.email = PlayerPrefs.GetString("LoginUser", "Unknown"); ;
        string jsonToServer = JsonUtility.ToJson(user);
        UnityWebRequest request = new UnityWebRequest("https://cardgamejavaserver.herokuapp.com/collection/userCards", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonToServer);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send();
        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            string jsonString = JsonHelper.fixJson(request.downloadHandler.text);
            CardBean[] cards = JsonHelper.FromJson<CardBean>(jsonString);
            playerCards = new Dictionary<CardBean, int>();

            Dictionary<CardBean, int> notIniqCards = cards
                .GroupBy(n => n, (n, m) => new { Key = n, Cnt = m.Count() })
                .Where(n => n.Cnt > 1)
                .ToDictionary(n => n.Key, n => n.Cnt);

            Dictionary<CardBean, int> uniqCards = cards
                .GroupBy(n => n, (n, m) => new { Key = n, Cnt = m.Count() })
                .Where(n => n.Cnt == 1)
                .ToDictionary(n => n.Key, n => n.Cnt);

            playerCards = uniqCards.Union(notIniqCards).ToDictionary(n => n.Key, n => n.Value);
            GameObject findCard = new GameObject();
            foreach (KeyValuePair<CardBean, int> keyValue in playerCards)
            {
                findCard = GameObject.Find(keyValue.Key.id);
                findCard.GetComponentInChildren<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                findCard.transform.Find("Count").GetComponent<SpriteRenderer>().sprite = Resources.Load("sprites/Cards/power/" + keyValue.Value, typeof(Sprite)) as Sprite;
            }
        }
    }
}
