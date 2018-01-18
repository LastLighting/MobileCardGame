using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckPhoton : MonoBehaviour {

	public List<CardBean> cardsInDeck = new List<CardBean>();
	public int countCardsInDeck;

	private void OnMouseDown()
	{
		gameObject.transform.Find("CountPlace").gameObject.SetActive(true);
		gameObject.GetComponentInChildren<Text>().text = countCardsInDeck.ToString();
	}

	private void OnMouseUp()
	{
		gameObject.transform.Find("CountPlace").gameObject.SetActive(false);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		Vector3 pos = transform.position;
		countCardsInDeck = cardsInDeck.Count;
		int count = countCardsInDeck;
		stream.Serialize(ref pos);
		stream.Serialize(ref count);

		if (stream.isReading)
		{
			countCardsInDeck = count;
			pos.y = -pos.y;
			transform.position = pos;
			transform.parent = GameObject.Find("NumberCanvas").transform;
			gameObject.transform.Find("CountPlace").localPosition = new Vector3(2.2f, -3.1f);
		}
	}
}
