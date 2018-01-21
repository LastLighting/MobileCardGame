using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInDeckBuild : MonoBehaviour {

    public ItemList itemList;

    public void OnMouseUp()
    {
        Sprite sprite = gameObject.transform.Find("Count").GetComponent<SpriteRenderer>().sprite;
        if (sprite != null)
        {
            itemList = GameObject.Find("Item List").GetComponent<ItemList>();
            itemList.AddToList(gameObject.GetComponent<Card>().Id, gameObject.GetComponent<Card>().Name, true);
            if (Int32.Parse(sprite.name) == 1)
            {
                sprite = null;
                gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(100, 100, 100, 255);
            }
            else
            {
                sprite = Resources.Load("sprites/Cards/power/" + (Int32.Parse(sprite.name) - 1).ToString(), typeof(Sprite)) as Sprite;
            }
            gameObject.transform.Find("Count").GetComponent<SpriteRenderer>().sprite = sprite;
        }     
    }
}
