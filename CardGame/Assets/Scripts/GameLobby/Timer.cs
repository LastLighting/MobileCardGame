using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	float timeRamaining = 60;
	public Text time;
	HandPhoton hand;
	bool lastAvailable;

	void Update()
	{
		if (PhotonNetwork.playerList.Length == 2)
		{
			hand = GameObject.Find("CardHand" + PlayerPrefs.GetString("LoginUser", "Unknown")).GetComponent<HandPhoton>();
			
			timeRamaining -= Time.deltaTime;
			time.text = ((int) timeRamaining).ToString();
			
			if ((timeRamaining <= 0) && (hand.isAvailable))
			{
				hand.isAvailable = false;
			}

			if (hand.isAvailable)
			{
				if (!lastAvailable)
				{
					timeRamaining = 60;
				}
				lastAvailable = hand.isAvailable;
			}
			else
			{
				if (lastAvailable)
				{
					timeRamaining = 60;
				}
				lastAvailable = hand.isAvailable;
			}
			
			
		}
	}
}
