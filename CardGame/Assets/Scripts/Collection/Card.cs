using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public Sprite[] texture;
    public int numberMaterial;
    public Sprite sdfs;
    private Vector3 startVector;

	void Start () {
        
    }
	
	void Update () {
		
	}

    public void changeSprite(int i)
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().sprite = texture[i];
    }

    public void OnMouseDown()
    {
        gameObject.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        startVector = gameObject.transform.position;
        gameObject.transform.position = new Vector3(1.5f,0,-1);
    }

    public void OnMouseUp()
    {
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        gameObject.transform.position = startVector;
    }

}
