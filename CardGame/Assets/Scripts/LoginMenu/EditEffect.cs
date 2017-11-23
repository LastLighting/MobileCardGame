using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditEffect : MonoBehaviour {

    public void changeColor(string fdfd) 
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(0, 0, 0, 255);
        gameObject.GetComponentInChildren<Text>().color = new Color32(0, 0, 0, 255);
        Debug.Log(fdfd);
    }

    
}
