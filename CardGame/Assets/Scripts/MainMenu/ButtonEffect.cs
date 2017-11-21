using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEffect: MonoBehaviour {

    private IEnumerator changeColor()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(0, 0, 0, 255);
        gameObject.GetComponentInChildren<Text>().color = new Color32(0, 0, 0, 255);
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(163, 163, 163, 255);
        gameObject.GetComponentInChildren<Text>().color = Color.white;
    }

    public void OnMouseDown()
    {
        StartCoroutine(changeColor());
    }
}
