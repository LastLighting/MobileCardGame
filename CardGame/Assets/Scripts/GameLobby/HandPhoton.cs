using System;
using UnityEngine;
using UnityEngine.UI;

public class HandPhoton : Photon.MonoBehaviour {
    
    public bool isAvailable;
    
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        int count = gameObject.transform.childCount;
        bool available = isAvailable;
        stream.Serialize(ref available);
        stream.Serialize(ref count);
        if (stream.isReading)
        {
            isAvailable = available;
            if (!available)
            {
                GameObject.Find("CardHand" + PlayerPrefs.GetString("LoginUser", "Unknown")).GetComponent<HandPhoton>().isAvailable = true;
            }
            if (GameObject.Find("CardHand" + PlayerPrefs.GetString("LoginUser", "Unknown")).transform.childCount == 0 && count == 0)
            {
                GameObject.Find("Modal").transform.GetChild(0).gameObject.SetActive(true);
                int mineScore = Int32.Parse(GameObject.Find("Score" + PlayerPrefs.GetString("LoginUser", "Unknown")).GetComponentInChildren<Text>().text);
                int enemyScore = Int32.Parse(GameObject.Find("ScoreBox(Clone)").GetComponentInChildren<Text>().text);
                if (mineScore > enemyScore)
                {
                    GameObject.Find("FinalModal").GetComponentInChildren<Text>().text = "Безоговорочная победа!";
                }
                else
                {
                    GameObject.Find("FinalModal").GetComponentInChildren<Text>().text = "Бесславное поражение!";
                }
            }
        }
    }
}
