using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPhoton : Photon.MonoBehaviour
{
	public bool isVisible;
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		Vector3 pos = transform.position;
		string cardGuid = gameObject.GetComponent<Card>().Id;
		int cardStrength = gameObject.GetComponent<Card>().Strength;
		stream.Serialize(ref pos);
		stream.Serialize(ref cardGuid);
		stream.Serialize(ref cardStrength);
		stream.Serialize(ref isVisible);
		if (stream.isReading)
		{
			if (isVisible)
			{
				Card card = gameObject.GetComponent<Card>();
				card.Id = cardGuid;
				card.Strength = cardStrength;
				card.changeStrength(card.Strength);
				card.sprites[0] = Resources.Load("sprites/Cards/SmallSize/" + card.Id, typeof(Sprite)) as Sprite;
				card.changeSprite(0);
				card.transform.localScale = new Vector3(1F, 1F, 0);	
			}
			else
			{
				transform.localScale = new Vector3(0.6F, 0.6F, 0);
			}
			pos.y = -pos.y;
			transform.position = pos;
		}
	}
}
