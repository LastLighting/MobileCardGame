using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePhoton : Photon.MonoBehaviour {

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        string score = gameObject.GetComponentInChildren<Text>().text;
        Vector3 pos = transform.position;
        stream.Serialize(ref pos);
        stream.Serialize(ref score);
        if (stream.isReading)
        {
            pos.y = -pos.y;
            transform.position = pos;
            transform.parent = GameObject.Find("NumberCanvas").transform;
            gameObject.GetComponentInChildren<Text>().text = score;
        }
    }
}
