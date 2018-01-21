using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class DeckBuild : MonoBehaviour {

    public Card card;
    public float startTransformX;
    public float startTransformY;
    public float transformX;
    public float transformY;
    Dictionary<CardBean, int> playerCards;
    private DeckBean deck;

    public DeckBean GetDeck()
    {
        return deck;
    }

    void Start () {
        StartCoroutine(getUserCollection());
        
    }

    IEnumerator getUserDeck(DeckBean deckBean)
    {
        string jsonToServer = JsonUtility.ToJson(deckBean);
        UnityWebRequest request = new UnityWebRequest("https://cardgamejavaserver.herokuapp.com/deck/userDeck", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonToServer);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send();
        if (request.isNetworkError)
        {
            // todo обработать
        }
        else
        {            
            deck = JsonUtility.FromJson<DeckBean>(request.downloadHandler.text);
            Vector3 position = transform.position;
            position.y = position.y + 2.5f;
            position.x = position.x + 7f;
            position.z = 0;
            Card newCard = Instantiate(card, position, card.transform.rotation) as Card;
            newCard.name = deck.leader.id;
            newCard.gameObject.AddComponent<CardInColl>();
            newCard.Id = deck.leader.id;
            newCard.Strength = deck.leader.strength;
            newCard.changeStrength(newCard.Strength);
            newCard.sprites[0] = Resources.Load("sprites/Cards/SmallSize/" + newCard.Id, typeof(Sprite)) as Sprite;
            newCard.sprites[1] = Resources.Load("sprites/Cards/FullSize/" + newCard.Id, typeof(Sprite)) as Sprite;
            newCard.transform.Find("Count").gameObject.SetActive(false);
            newCard.changeSprite(0);
            newCard.transform.position = position;
            newCard.transform.localScale = new Vector3(0.9F, 0.9F, 0);
            newCard.transform.parent = GameObject.Find("RightPanel").transform;
            foreach (CardBean card in deck.cards)
            {
                ItemList itemList = GameObject.Find("Item List").GetComponent<ItemList>();
                itemList.AddToList(card.id, card.name, false);
                Sprite sprite = GameObject.Find(card.id).transform.Find("Count").GetComponent<SpriteRenderer>().sprite;
                if (sprite != null)
                {
                    if (Int32.Parse(sprite.name) == 1)
                    {
                        sprite = null;
                        GameObject.Find(card.id).GetComponentInChildren<SpriteRenderer>().color = new Color32(100, 100, 100, 255);
                    }
                    else
                    {
                        sprite = Resources.Load("sprites/Cards/power/" + (Int32.Parse(sprite.name) - 1).ToString(), typeof(Sprite)) as Sprite;
                    }
                    GameObject.Find(card.id).transform.Find("Count").GetComponent<SpriteRenderer>().sprite = sprite;
                }
            }
        }
    }

    IEnumerator getUserCollection()
    {
        User user = new User();
        user.email = PlayerPrefs.GetString("LoginUser", "Unknown");
        Debug.Log(user.email);
        string jsonToServer = JsonUtility.ToJson(user);
        UnityWebRequest request = new UnityWebRequest("https://cardgamejavaserver.herokuapp.com/collection/userDeckCards", "POST");
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
           
            int count = 1;
            Vector3 position = transform.position;
            position.y = position.y + startTransformY;
            position.x = position.x - startTransformX;
            position.z = 0;
            foreach (KeyValuePair<CardBean, int> keyValue in playerCards)
            {            
                Card newCard = Instantiate(card, position, card.transform.rotation) as Card;
                newCard.gameObject.AddComponent<CardInDeckBuild>();
                newCard.name = keyValue.Key.id;
                newCard.Id = keyValue.Key.id;
                newCard.Strength = keyValue.Key.strength;
                newCard.Name = keyValue.Key.name;
                newCard.changeStrength(newCard.Strength);
                newCard.sprites[0] = Resources.Load("sprites/Cards/SmallSize/" + newCard.Id, typeof(Sprite)) as Sprite;
                newCard.sprites[1] = Resources.Load("sprites/Cards/FullSize/" + newCard.Id, typeof(Sprite)) as Sprite;
                newCard.changeSprite(0);
                newCard.transform.position = position;
                newCard.transform.localScale = new Vector3(1.3F, 1.3F, 0);
                newCard.transform.Find("Count").GetComponent<SpriteRenderer>().sprite = Resources.Load("sprites/Cards/power/" + keyValue.Value, typeof(Sprite)) as Sprite;
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
            DeckBean deckBean = new DeckBean();
            User findUser = new User();
            findUser.email = PlayerPrefs.GetString("LoginUser", "Unknown");
            deckBean.user = findUser;
            deckBean.name = PlayerPrefs.GetString("DeckName", "Unknown");
            StartCoroutine(getUserDeck(deckBean));
        }
    }
}
