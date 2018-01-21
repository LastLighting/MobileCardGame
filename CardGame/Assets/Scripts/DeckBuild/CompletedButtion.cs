using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CompletedButtion : MonoBehaviour {

    public DeckBuild controller;

    private IEnumerator changeColor()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(0, 0, 0, 255);
        gameObject.GetComponentInChildren<Text>().color = new Color32(0, 0, 0, 255);
        yield return new WaitForSeconds(0.4f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(163, 163, 163, 255);
        gameObject.GetComponentInChildren<Text>().color = Color.white;
    }

    public void OnMouseDown()
    {
        StartCoroutine(changeColor());
    }

    public void OnMouseUp()
    {
        StartCoroutine(setDeck());
    }

    IEnumerator setDeck()
    {
        DeckBean deck = controller.GetDeck();
        ItemList itemList = GameObject.Find("Item List").GetComponent<ItemList>();
        List<CardBean> list = itemList.GetList();
        User user = new User();
        user.email = PlayerPrefs.GetString("LoginUser", "Unknown");
        deck.cards = list;
        deck.user = user;
        string jsonToServer = JsonUtility.ToJson(deck);
        UnityWebRequest request = new UnityWebRequest("https://cardgamejavaserver.herokuapp.com/deck/editDeck", "POST");
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
            SceneManager.LoadScene(3);
        }
    }
}
