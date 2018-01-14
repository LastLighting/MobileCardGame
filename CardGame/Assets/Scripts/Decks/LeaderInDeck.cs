using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderInDeck : MonoBehaviour {

    public DecksController controller;

    public void OnMouseUp()
    {
        controller = GameObject.Find("Canvas").GetComponent<DecksController>();
        Card card = gameObject.GetComponent<Card>();
        CardBean leader = new CardBean();
        leader.id = card.Id;
        leader.name = card.Name;
        leader.strength = card.Strength;
        controller.setLeaderDeck(leader);
        controller.createDeck();
    }
}
