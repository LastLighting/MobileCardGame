using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class DecksController : MonoBehaviour
{
    public Deck deck;
    public float startTransformX;
    public float startTransformY;
    public float transformX;
    public string[] ids;
    void Start()
    {
        Vector3 position = transform.position;
        position.y = position.y + startTransformY;
        position.x = position.x - startTransformX;
        position.z = 0;
        for (int x = 0; x < 3; x++)
        {
            Deck newDeck = Instantiate(deck, position, deck.transform.rotation) as Deck;
            Card card = new Card();
            
            newDeck.Leader = card;
            newDeck.Leader.Id = ids[x];
            newDeck.Leader.Strength = Random.Range(1, 20);
            newDeck.changeStrengthLeader(newDeck.Leader.Strength);
            newDeck.Leader.sprites[0] = Resources.Load("sprites/Cards/SmallSize/" + newDeck.Leader.Id, typeof(Sprite)) as Sprite;
            newDeck.Leader.sprites[1] = Resources.Load("sprites/Cards/FullSize/" + newDeck.Leader.Id, typeof(Sprite)) as Sprite;
            newDeck.changeSprite(0);
            newDeck.transform.position = position;
            newDeck.transform.localScale = new Vector3(1.3F, 1.3F, 0);
            newDeck.transform.parent = GameObject.Find("Content").transform;
            position.x = position.x + transformX;
        }
        
        DeckBean deckToServer = new DeckBean();
        deckToServer.name = "Тестовая";
        CardBean leader = new CardBean();
        leader.id = "806e2d08-aa1a-4fad-87cb-b86c5699534d";
        leader.name = "Тёмный властелин";
        leader.strength = 20;
        deckToServer.leader = leader;
        User user = new User();
        user.email = PlayerPrefs.GetString("LoginUser", "Unknown");
        deckToServer.user = user;
        StartCoroutine(createNewDeck(deckToServer)); 
    }   
    
    IEnumerator createNewDeck(DeckBean deckBean)
    {
        string jsonToServer = JsonUtility.ToJson(deckBean);
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/deck/create", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonToServer);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send(); 
        if(request.isError) {
           // todo обработать
        }
    }
}
