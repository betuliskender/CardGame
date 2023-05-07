using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;


public class CardPreviewController : MonoBehaviour, IPointerClickHandler
{

    public Card card;
    public Image illustration, image;
    public TextMeshProUGUI cardName, health, manaCost, damage;
    private Transform originalParent;


    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Initialize(Card card, int ownerID, Transform intendedParent)
    {
       this.card = CastCardType(card, ownerID);

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

    private Card CastCardType(Card card, int ownerID)
    {
        if (card.GetType() == typeof(SpellCard))
        {
            SpellCard spellCard = (SpellCard)card;

            spellCard.ownerID = ownerID;

            return spellCard;

        }

        if (card.GetType() == typeof(ItemCard))
        {
            ItemCard itemCard = (ItemCard)card;


            itemCard.ownerID = ownerID;

            return itemCard;
        }


        
            card = new Card(card)
            {
                ownerID = ownerID

            };

        return card;
    }



    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {
           
            EditDeckManager.instance.AddChosenCard(card);
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            
            EditDeckManager.instance.RemoveChosenCard(card);
        }
    }
}
