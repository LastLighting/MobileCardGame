using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInDeckBuild : MonoBehaviour {

    public ItemList itemList;
    private GameObject list;
    GameObject gfff;

    public void OnMouseUp()
    {
        itemList = GameObject.Find("Item List").GetComponent<ItemList>();
        Debug.Log(gameObject.GetComponent<Card>().Id + gameObject.GetComponent<Card>().Name);
        itemList.AddToList(gameObject.GetComponent<Card>().Id, gameObject.GetComponent<Card>().Name, true);
    }
}
