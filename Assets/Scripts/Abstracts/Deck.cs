using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Deck
{

    //public List<Card> cardList = new List<Card>();
    public Stack<Card> cardStack = new Stack<Card>();

    public Deck(List<Card> cardList)
    {
       
        var shuffledDeck = ShuffleDeck(cardList);
        CreateDeck(shuffledDeck);
    }


   /* // Use this for initialization
    void OnEnable()
    {
        ShuffleDeck();
        CreateDeck();
        DrawCard();
    }*/

    public void CreateDeck(List<Card> cardList)
    {
        foreach (Card card in cardList)
        {
            cardStack.Push(card);
        }
    }
    public void ReShuffleCards(List<CardController> cards)
    {
        cards.ForEach((card) =>
        {
            cardStack.Push(card.card);
        });
        List<Card> cardList = cardStack.ToList();
        ShuffleDeck(cardList);
        foreach (Card card in cardList)
        {
            cardStack.Push(card);
        }
    }
    public List<Card> ShuffleDeck(List<Card> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            int randomIndex = Random.Range(i, cardList.Count);
            Card tempCard = cardList[i];
            cardList[i] = cardList[randomIndex];
            cardList[randomIndex] = tempCard;
        }
        return cardList;
    }

    public Card DrawCard()
    {
        return cardStack.Pop();
    }

    public List<Card> StartHand()
    {
        List<Card> cards = new List<Card>
        {
            cardStack.Pop(),
            cardStack.Pop(),
            cardStack.Pop(),
            cardStack.Pop()
        };

        return cards;
    }

}
