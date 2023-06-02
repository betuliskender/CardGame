using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    public Card card;
    public Image illustration, image;
    public TextMeshProUGUI cardName, health, manaCost, damage;
    private Transform originalParent;
    public event Action CardAction;
    private bool isSelected = false;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void ActionCall()
    {
        CardAction?.Invoke();
        
    }
    
    public void Initialize(Card card, int ownerID, Transform intendedParent)
    {
       this.card = ManageCardActions(card, ownerID);

        illustration.sprite = card.illustration;
        cardName.text = card.cardName;
        manaCost.text = card.manaCost.ToString();
        damage.text = card.damage.ToString();
        health.text = card.health.ToString();
        originalParent = intendedParent;
        Tweener tween = transform.DOMove(intendedParent.transform.position, 1, true);
        transform.DOScale(1, 0.85f);
        tween.onComplete += () =>
        {
            transform.SetParent(intendedParent);
        };
        if (card.health == 0) health.text = "";
    }

    private Card ManageCardActions(Card card, int ownerID)
    {
        if (card.GetType() == typeof(SpellCard))
        {
            
            SpellCard sc = (SpellCard)card;
            SpellCard spellCard = new SpellCard(card, sc.SpellPower);
            spellCard.ownerID = ownerID;

            CardAction += spellCard.Instant;

            return spellCard;
        }

        if (card.GetType() == typeof(ItemCard))
        {
            ItemCard ic = (ItemCard)card;

            ItemCard itemCard = new ItemCard(card, ic.healAmount);
            itemCard.ownerID = ownerID;
            
            CardAction += itemCard.Instant;

            return itemCard;
        }
        
            this.card = new Card(card)
            {
                ownerID = ownerID
            };
            CardAction += this.card.CardActionTest;

             return this.card;
        
    }

    public void Damage(int amount)
    {
        card.health -= amount;
        health.text = card.health.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(MulliganManager.isMulliganActive)
        {
            return;
        }
        if(originalParent.name == $"Player{card.ownerID + 1}PlayArea" || TurnManager.instance.currentPlayerTurn != card.ownerID)
        {
            transform.DOShakeScale(0.35f, 0.5f, 5);
        } else
        {
            transform.SetParent(transform.root);
            image.raycastTarget = false;
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
        if (originalParent.name == $"Player{card.ownerID + 1}PlayArea" || TurnManager.instance.currentPlayerTurn != card.ownerID)
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
                PlayerManager.instance.SpendMana(card.ownerID, card.manaCost);

            }
            else
            {
                transform.DOShakeScale(0.25f, 0.25f, 3);
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
        CardManager.instance.PlayCard(this, card.ownerID); 
    }

    public void ReturnToHand()
    {
        if (MulliganManager.isMulliganActive)
        {
            return;
        }
        Tweener tween = transform.DOMove(originalParent.transform.position, 0.35f, true);
        tween.onComplete += () =>
        {
            transform.SetParent(originalParent);
        };
       
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        if (transform.parent == originalParent) return;
        transform.position = eventData.position;
    }

}
