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
    int count = 0;
	void Update()
	{
		if (PhotonNetwork.playerList.Length == 2)
		{
			hand = GameObject.Find("CardHand" + PlayerPrefs.GetString("LoginUser", "Unknown")).GetComponent<HandPhoton>();
            if (count == 0)
            {
                if (hand.isAvailable)
                {
                    StartCoroutine(yourStep());                   
                }
                else
                {
                    StartCoroutine(enemyStep());
                }
            }
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
                    StartCoroutine(yourStep());
                    timeRamaining = 60;
				}
				lastAvailable = hand.isAvailable;
			}
			else
			{
				if (lastAvailable)
				{
                    StartCoroutine(enemyStep());
                    timeRamaining = 60;
				}
				lastAvailable = hand.isAvailable;
			}
			
			
		}
	}

    IEnumerator yourStep()
    {
        GameObject.Find("Modal").transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameObject.Find("Modal").transform.GetChild(1).gameObject.SetActive(false);
    }

    IEnumerator enemyStep()
    {
        GameObject.Find("Modal").transform.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameObject.Find("Modal").transform.GetChild(2).gameObject.SetActive(false);
    }
}
