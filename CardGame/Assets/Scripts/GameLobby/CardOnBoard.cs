using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardOnBoard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Camera mainCamera;
	public Vector3 offset;
	public Transform defaultParent, defaultTempCardParent;
	GameObject tempCard;
	public bool available;
	
	private void Awake()
	{
		available = true;
		mainCamera = Camera.allCameras[0];
		tempCard = GameObject.Find("TempCard");
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (available)
		{
			offset = transform.position - mainCamera.ScreenToWorldPoint(eventData.position);
			defaultParent = defaultTempCardParent = transform.parent;
			tempCard.transform.SetParent(defaultParent);
			tempCard.transform.SetSiblingIndex(transform.GetSiblingIndex());
			transform.SetParent(defaultParent.parent);
			GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (available)
		{
			Vector3 newPosition = mainCamera.ScreenToWorldPoint(eventData.position);
			newPosition.z = 0;
			transform.position = newPosition + offset;
			if (tempCard.transform.parent != defaultTempCardParent)
			{
				tempCard.transform.SetParent(defaultTempCardParent);
			}
			CheckPosition();
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (available)
		{
			transform.SetParent(defaultParent);
			GetComponent<CanvasGroup>().blocksRaycasts = true;
			transform.SetSiblingIndex(tempCard.transform.GetSiblingIndex());
			tempCard.transform.SetParent(GameObject.Find("Canvas").transform);
			tempCard.transform.localPosition = new Vector3(11.5f, 0);
			foreach (Transform child in defaultParent)
			{
				child.Find("Strenge").GetComponent<SpriteRenderer>().sortingOrder = child.GetSiblingIndex() + 2;
				child.Find("Front").GetComponent<SpriteRenderer>().sortingOrder = child.GetSiblingIndex() + 1;
			}
			GameObject.Find("Score" + PlayerPrefs.GetString("LoginUser", "Unknown")).GetComponentInChildren<Text>().text =
				countCardScore().ToString();
		}
	}

	void CheckPosition()
	{
		int newIndex = defaultTempCardParent.childCount;
		for (int i = 0; i < defaultTempCardParent.childCount; i++)
		{
			if (transform.position.x < defaultTempCardParent.GetChild(i).position.x)
			{
				newIndex = i;
				if (tempCard.transform.GetSiblingIndex() < newIndex)
				{
					newIndex--;
				}
				break;
			}
		}
		tempCard.transform.SetSiblingIndex(newIndex);
	}

	int countCardScore()
	{
		int countScore = 0;
		foreach (Transform child in GameObject.Find("БлижнийБой").transform)
		{
			countScore += child.GetComponent<Card>().Strength;
		}
		foreach (Transform child in GameObject.Find("ДальнийБой").transform)
		{
			countScore += child.GetComponent<Card>().Strength;
		}
		return countScore;
	}
	
	void Update()
	{
		if (gameObject.GetComponent<CardPhoton>().isAvailable)
		{
			if (PhotonNetwork.player.UserId == gameObject.GetPhotonView().owner.UserId)
			{
				available = true;
			}
		}
		else
		{
			available = false;
		}
	}
}
