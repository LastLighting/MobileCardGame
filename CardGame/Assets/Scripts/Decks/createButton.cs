using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class createButton : MonoBehaviour {

    public GameObject modal;
    public GameObject blur;
    public Text deckName;
    public DecksController controller;
    public GameObject choiseLeaderModal;

    public void OnMouseUp()
    {
        modal.SetActive(false);
        controller.setDeckName(deckName.text);
        choiseLeaderModal.SetActive(true);
        controller.getAllLeader();
    }
}
