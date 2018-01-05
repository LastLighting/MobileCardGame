using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

    public string Id { get; set; }
    public string Name { get; set; }
    public Card Leader { get; set; }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void changeSprite(int i)
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Leader.sprites[i];
    }

    public void changeStrengthLeader(int power)
    {
        gameObject.transform.Find("Strenge").GetComponent<SpriteRenderer>().sprite = Resources.Load("sprites/Cards/power/" + power, typeof(Sprite)) as Sprite;
    }
}
