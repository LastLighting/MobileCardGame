using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropCard : MonoBehaviour, IDropHandler {
    
    public void OnDrop(PointerEventData eventData)
    {
        CardOnBoard card = eventData.pointerDrag.GetComponent<CardOnBoard>();

        if (card)
        {
            card.defaultParent = transform;
        }
    }
}
