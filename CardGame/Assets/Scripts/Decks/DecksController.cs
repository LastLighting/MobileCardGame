using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DecksController : MonoBehaviour
{
    public Deck deck;
    public float startTransformX;
    public float startTransformY;
    public float transformX;
    public string[] ids;
    private DeckBean deckToServer = new DeckBean();
    public Card card;


    void Start()
    {
        
        StartCoroutine(getUserDecks());
    }   
    
    public void setDeckName(string name)
    {
        deckToServer.name = name;
    }

    public void setLeaderDeck(CardBean leader)
    {
        deckToServer.leader = leader;
    }

    public void createDeck()
    {
        User user = new User();
        user.email = PlayerPrefs.GetString("LoginUser", "Unknown");
        deckToServer.user = user;
        StartCoroutine(createNewDeck(deckToServer));
    }

    IEnumerator createNewDeck(DeckBean deckBean)
    {
        string jsonToServer = JsonUtility.ToJson(deckBean);
        UnityWebRequest request = new UnityWebRequest("https://cardgamejavaserver.herokuapp.com/deck/create", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonToServer);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send(); 
        if(request.isNetworkError) {
            // todo обработать
        }
        else
        {
            PlayerPrefs.SetString("DeckName", deckBean.name);
            SceneManager.LoadScene(4);
        }
    }

    IEnumerator getUserDecks()
    {
        User user = new User();
        user.email = PlayerPrefs.GetString("LoginUser", "Unknown");
        string jsonToServer = JsonUtility.ToJson(user);
        UnityWebRequest request = new UnityWebRequest("https://cardgamejavaserver.herokuapp.com/deck/list", "POST");
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
            DeckBean[] cards = JsonHelper.FromJson<DeckBean>(jsonString);
            Vector3 position = transform.position;
            position.x = position.x - startTransformX;
            foreach (DeckBean decka in cards)
            {
                Deck newDeck = Instantiate(deck, position, deck.transform.rotation) as Deck;
                Card card = new Card();
                newDeck.Name = decka.name;
                newDeck.changeName(newDeck.Name);
                newDeck.Leader = card;
                newDeck.Leader.Id = decka.leader.id;
                newDeck.Leader.Strength = decka.leader.strength;
                newDeck.changeStrengthLeader(newDeck.Leader.Strength);
                newDeck.Leader.sprites[0] = Resources.Load("sprites/Cards/SmallSize/" + newDeck.Leader.Id, typeof(Sprite)) as Sprite;
                newDeck.Leader.sprites[1] = Resources.Load("sprites/Cards/FullSize/" + newDeck.Leader.Id, typeof(Sprite)) as Sprite;
                newDeck.changeSprite(0);
                newDeck.transform.position = position;
                newDeck.transform.localScale = new Vector3(1.3F, 1.3F, 0);
                newDeck.transform.parent = GameObject.Find("Content").transform;
                position.x = position.x + transformX;
                Debug.Log(decka.leader.name);
            }
        }
    }

    public void getAllLeader()
    {
        StartCoroutine(getLeader());
    }

    IEnumerator getLeader()
    {
        UnityWebRequest request = new UnityWebRequest("https://cardgamejavaserver.herokuapp.com/deck/leaderCards", "POST");
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
            CardBean[] collection = JsonHelper.FromJson<CardBean>(jsonString);         
            Vector3 position = transform.position;
            position.y -= 0.4f;
            position.x = position.x-1;
            position.z = 0;
            foreach (CardBean cardLeader in collection)
            {
                Card newCard = Instantiate(card, position, card.transform.rotation) as Card;
                newCard.name = cardLeader.id;
                newCard.gameObject.AddComponent<LeaderInDeck>();
                newCard.Id = cardLeader.id;
                newCard.Strength = cardLeader.strength;
                newCard.changeStrength(newCard.Strength);
                newCard.sprites[0] = Resources.Load("sprites/Cards/SmallSize/" + newCard.Id, typeof(Sprite)) as Sprite;
                newCard.sprites[1] = Resources.Load("sprites/Cards/FullSize/" + newCard.Id, typeof(Sprite)) as Sprite;
                newCard.transform.Find("Count").gameObject.SetActive(false);
                newCard.changeSprite(0);
                newCard.transform.position = position;
                newCard.transform.localScale = new Vector3(1.4F, 1.4F, 0);
                newCard.transform.parent = GameObject.Find("ChoiseLeader").transform;
                position.x = position.x + transformX;
            }
        }
    }
}
