using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class CompletedButtion : MonoBehaviour {

    public void OnMouseDown()
    {
        StartCoroutine(setDeck());
    }

    IEnumerator setDeck()
    {
        ItemList itemList = GameObject.Find("Item List").GetComponent<ItemList>();
        List<CardBean> list = itemList.GetList();
        User user = new User();
        user.email = PlayerPrefs.GetString("LoginUser", "Unknown");
        DeckBean deck = new DeckBean();
        deck.cards = list;
        deck.user = user;
        deck.name = "Тестовая";
        string jsonToServer = JsonUtility.ToJson(deck);
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/deck/editDeck", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonToServer);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send();
        if (request.isError)
        {
            Debug.Log(request.error);
        }
        else
        {
        }
    }
}
