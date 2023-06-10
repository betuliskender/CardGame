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
    public Image backgroundImage;
    public TextMeshProUGUI cardName, health, manaCost, damage;
    private Transform originalParent;
    public event Action CardAction;
    public bool isSelected = false;
    public bool IsOnBoard { get; private set; }
    public bool isDiscarded = false;

    private void Awake()
    {
        image = GetComponent<Image>();
        backgroundImage = transform.Find("Background").GetComponent<Image>();
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
        IsOnBoard = false;
        UpdateVisibility(0);
    }

    private Card ManageCardActions(Card card, int ownerID)
    {
        if (card.GetType() == typeof(SpellCard))
        {
            SpellCard sc = (SpellCard)card;
            SpellCard spellCard = new SpellCard(card, sc.SpellPower);
            spellCard.ownerID = ownerID;

            CardAction = null;
            CardAction += Discard;
            CardAction += spellCard.Instant;

            return spellCard;
        }

        else if (card.GetType() == typeof(ItemCard))
        {
            ItemCard ic = (ItemCard)card;

            ItemCard itemCard = new ItemCard(card, ic.healAmount);
            itemCard.ownerID = ownerID;

            CardAction = null;
            CardAction += itemCard.Instant;
            CardAction += Discard;

            return itemCard;
        }
        else
        {

            this.card = new Card(card)
            {
                ownerID = ownerID
            };
            
            CardAction += this.card.CardActionTest;

             return this.card;
        }
        
    }

    public void Damage(int amount)
    {
        card.health -= amount;
        health.text = card.health.ToString();
    }

    public void Discard()
    {
        CardManager.instance.DiscardCard(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (MulliganManager.isMulliganActive)
        {
            if (!isSelected && MulliganManager.instance.selectedCards.Count < 3 && TurnManager.instance.CurrentPlayerTurn == card.ownerID)
            {
                isSelected = true;

                if (card.ownerID == 0 && isSelected)
                {
                    transform.localPosition += new Vector3(0f, 280f, 0f);
                    transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                }
                if (card.ownerID == 1 && isSelected)
                {
                    transform.localPosition += new Vector3(0f, -280f, 0f);
                    transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                }

                transform.SetParent(transform.root);
                image.raycastTarget = false;
                MulliganManager.instance.selectedCards.Add(this);
                Debug.Log("Amount of cards in selectedCards: " + MulliganManager.instance.selectedCards.Count);
            }
            else if (isSelected)
            {
                isSelected = false;

                if (card.ownerID == 0 && !isSelected)
                {
                    transform.localPosition -= new Vector3(0f, -280f, 0f);
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                if(card.ownerID == 1 && !isSelected)
                {
                    transform.localPosition += new Vector3(0f, 280f, 0f);
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                transform.SetParent(originalParent);
                image.raycastTarget = true;
                MulliganManager.instance.selectedCards.Remove(this);
                Debug.Log("Amount of cards in selectedCards: " + MulliganManager.instance.selectedCards.Count);
            }
        }
        else if (originalParent.name == $"Player{card.ownerID + 1}PlayArea" || TurnManager.instance.currentPlayerTurn != card.ownerID)
        {
            transform.DOShakeScale(0.35f, 0.5f, 5);
        }
        else
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
        IsOnBoard = true;
        UpdateVisibility(card.ownerID);

        CardManager.instance.PlayCard(this, card.ownerID); 
    }

    public void ReturnToHand()
    {
        if (!MulliganManager.isMulliganActive)
        {
        Tweener tween = transform.DOMove(originalParent.transform.position, 0.35f, true);
        tween.onComplete += () =>
        {
            transform.SetParent(originalParent);
        };
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!MulliganManager.isMulliganActive && transform.parent != originalParent)
        {
            transform.position = eventData.position;
        }
    }

    private List<CardController> GetSelectedCards()
    {
        List<CardController> selectedCards = new List<CardController>();


        foreach (Transform child in originalParent)
        {
            CardController card = child.GetComponent<CardController>();

            if (card != null && card.isSelected)
            {
                selectedCards.Add(card);
            }
        }

        return selectedCards;
    }

    public void UpdateVisibility(int currentPlayerID)
    {
        if(isDiscarded)
        {
            illustration.enabled = true;
            cardName.enabled = true;
            manaCost.enabled = true;
            damage.enabled = true;
            health.enabled = true;
            backgroundImage.enabled = false;
        }
        else if (IsOnBoard || card.ownerID == currentPlayerID)
        {

            illustration.enabled = true;
            cardName.enabled = true;
            manaCost.enabled = true;
            damage.enabled = true;
            health.enabled = true;
            backgroundImage.enabled = false;
        }
        else
        {
            illustration.enabled = false;
            cardName.enabled = false;
            manaCost.enabled = false;
            damage.enabled = false;
            health.enabled = false;
            backgroundImage.enabled = true;
        }

    }

}
