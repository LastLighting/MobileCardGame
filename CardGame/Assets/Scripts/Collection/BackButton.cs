using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackButton : MonoBehaviour {

    private IEnumerator changeColor()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(123, 65, 193, 255);
        gameObject.GetComponentInChildren<Text>().color = new Color32(123, 65, 193, 255);
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
        SceneManager.LoadScene("MainMenu");
    }
}
