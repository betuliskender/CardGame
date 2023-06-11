using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;


public class CardPreviewController : MonoBehaviour, IPointerClickHandler
{

    public Card card;
    public AbstractCard AbstractCard;
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

    public void Initialize(AbstractCard card, int ownerID, Transform intendedParent)
    {
        CastCardType(card.cardName, ownerID);

        illustration.sprite = AbstractCard.illustration;
        cardName.text = AbstractCard.cardName;
        manaCost.text = AbstractCard.manaCost.ToString();
        
        originalParent = intendedParent;
        Tweener tween = transform.DOMove(intendedParent.transform.position, 1, true);
        transform.DOScale(1, 0.85f);
        tween.onComplete += () =>
        {
            transform.SetParent(intendedParent);
        };

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

    private void CastCardType(string cardName, int ownerID)
    {
        if (card.GetType() == typeof(AbstractSpellCard))
        {
            this.AbstractCard = CardGeneratorManager.instance.availableCards[cardName];

            AbstractSpellCard spellCard = (cardName)AbstractCard;

            this.AbstractCard.ownerID = ownerID;
            damage.text = (AbstractSpellCard) this.AbstractCard.SpellPower.ToString();
            health.text = " ";

            

        }

        if (card.GetType() == typeof(AbstractItemCard))
        {
            AbstractItemCard itemCard = (AbstractItemCard)card;

            damage.text = itemCard.healAmount.ToString();
            health.text = " ";

            itemCard.ownerID = ownerID;

            
        }



        if (card.GetType() == typeof(AbstractMonsterCard))
        {
            AbstractMonsterCard monsterCard = (AbstractMonsterCard)card;

            damage.text = monsterCard.damage.ToString();
            health.text = monsterCard.health.ToString();

            monsterCard.ownerID = ownerID;

            return monsterCard;
        }

        
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
