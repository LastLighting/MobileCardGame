using System.Collections;
using System.Collections.Generic;
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
        List<string> list = itemList.GetList();
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/collection/cards", "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send();
        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
        }
    }
}
