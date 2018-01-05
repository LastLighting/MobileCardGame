using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public Sprite[] sprites = new Sprite[2];
    private Vector3 startVector;
    private Vector3 startScale;
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

    public void OnMouseDown()
    {
        gameObject.transform.Find("Strenge").transform.localPosition = new Vector3(-0.762f, 0.049f, -1f);
        gameObject.GetComponentInChildren<SpriteRenderer>().sprite = sprites[1];
        startScale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        startVector = gameObject.transform.position;
        gameObject.transform.position = new Vector3(1.5f,0,-1);
    }

    public void OnMouseUp()
    {
        gameObject.transform.Find("Strenge").transform.localPosition = new Vector3(-0.754f, -0.665f, -1f);
        gameObject.GetComponentInChildren<SpriteRenderer>().sprite = sprites[0];
        gameObject.transform.localScale = startScale;
        gameObject.transform.position = startVector;
    }

}
