using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropCard : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
    
    private HandPhoton hand;
    
    public void OnDrop(PointerEventData eventData)
    {
        CardOnBoard card = eventData.pointerDrag.GetComponent<CardOnBoard>();
        if (card)
        {
            if (card.available)
            {
                card.defaultParent = transform;
                card.gameObject.GetComponent<CardPhoton>().isVisible = true;
                
                hand = GameObject.Find("CardHand" + PlayerPrefs.GetString("LoginUser", "Unknown")).GetComponent<HandPhoton>();
                if (hand.isAvailable) 
                {
                    hand.isAvailable = false;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
           return;
        }
        CardOnBoard card = eventData.pointerDrag.GetComponent<CardOnBoard>();
        if (card)
        {
            card.defaultTempCardParent = transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }
        CardOnBoard card = eventData.pointerDrag.GetComponent<CardOnBoard>();
        if (card && card.defaultTempCardParent == transform)
        {
            card.defaultTempCardParent = card.defaultParent;
        }
    }
}
