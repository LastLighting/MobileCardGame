using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public Sprite[] sprites = new Sprite[2];
    public string Id { get; set; }
    public string Name { get; set; }
    public int Strength { get; set; }
    

    public void changeSprite(int i)
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().sprite = sprites[i];
    }

    public void changeStrength(int power)
    {
        gameObject.transform.Find("Strenge").GetComponent<SpriteRenderer>().sprite = Resources.Load("sprites/Cards/power/" + power, typeof(Sprite)) as Sprite;
        gameObject.transform.Find("Strenge").transform.localPosition = new Vector3(-0.754f, -0.665f, -1f);
    }

}
