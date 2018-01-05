using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class okButton : MonoBehaviour {

    public GameObject modal;
    public GameObject blur;

    public void OnMouseUp()
    {
        blur.SetActive(false);
        modal.SetActive(false);
    }
}
