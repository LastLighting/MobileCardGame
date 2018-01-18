using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnButton : MonoBehaviour
{
	private HandPhoton hand;
	
	private void OnMouseUp()
	{
		hand = GameObject.Find("CardHand" + PlayerPrefs.GetString("LoginUser", "Unknown")).GetComponent<HandPhoton>();
		if (hand.isAvailable) 
		{
			hand.isAvailable = false;
		}
	}
	
}
