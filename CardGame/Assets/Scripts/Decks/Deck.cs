using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Deck : MonoBehaviour {

    public string Id { get; set; }
    public string Name { get; set; }
    public Card Leader { get; set; }

    public void OnMouseUp()
    {
        PlayerPrefs.SetString("DeckName", Name);
        SceneManager.LoadScene(4);
    }

    public void changeName(string name)
    {
        gameObject.GetComponentInChildren<Text>().text = name;
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
