using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardOnBoard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Camera mainCamera;
	public Vector3 offset;
	public Transform defaultParent, defaultTempCardParent;
	GameObject tempCard;
	
	private void Awake()
	{
		mainCamera = Camera.allCameras[0];
		tempCard = GameObject.Find("TempCard");
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		offset = transform.position - mainCamera.ScreenToWorldPoint(eventData.position);
		defaultParent = transform.parent;
		transform.SetParent(defaultParent.parent);
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector3 newPosition = mainCamera.ScreenToWorldPoint(eventData.position);
		newPosition.z = 0;
		transform.position = newPosition + offset;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		transform.SetParent(defaultParent);
		GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
}
