using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    public Card card;
    public Image illustration, image;
    public TextMeshProUGUI cardName, health, manaCost, damage;
    private Transform originalParent;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    
    public void Initialize(Card card)
    {
        this.card = card;
        illustration.sprite = card.illustration;
        cardName.text = card.cardName;
        manaCost.text = card.manaCost.ToString();
        damage.text = card.damage.ToString();
        health.text = card.health.ToString();
        originalParent = transform.parent;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("MouseEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("MouseExit");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(originalParent.name == $"Player{card.ownerID + 1}PlayArea")
        {
            
        } else
        {
            transform.SetParent(transform.root);
            image.raycastTarget = false;
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (originalParent.name == $"Player{card.ownerID + 1}PlayArea")
        {

        } else
        {
            image.raycastTarget = true;
            AnalyzePointerUp(eventData);
        }
        
;       
    }

    private void AnalyzePointerUp(PointerEventData eventData)
    {
        if(eventData.pointerEnter != null && eventData.pointerEnter.name == $"Player{card.ownerID+1}PlayArea")
        {
            if(PlayerManager.instance.FindPlayerByID(card.ownerID).mana >= card.manaCost)
            {
                PlayCard(eventData.pointerEnter.transform);
            }else
            {
                ReturnToHand();
            }
        }else
        {
            ReturnToHand();
        }
    }

    private void PlayCard(Transform playArea)
    {
        transform.SetParent(playArea);
        transform.localPosition = Vector3.zero;
        originalParent = playArea;
    }

    private void ReturnToHand()
    {
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (transform.parent == originalParent) return;
        transform.position = eventData.position;
    }
}
