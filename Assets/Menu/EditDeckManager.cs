using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class EditDeckManager : MonoBehaviour
{

    public static EditDeckManager instance;
   
    public Dictionary<Card, int> chosenCards = new Dictionary<Card, int>();
    public List<Card> cards = new List<Card>();
    public CardPreviewController cardControllerPrefab;
    private bool customDeck = false;
    public Button confirmbutton;


    private void Awake()
    {
        instance = this;   
        SetupAvailableCards();
        SetupButton();
    }

    private void SetupButton()
    {
        confirmbutton.onClick.AddListener(() =>
        {
           customDeck = true;
        });
    }

    private void SetupAvailableCards()
    {
        var gridContent = GameObject.Find("AvailableCardsGrid");

        foreach (Card card in cards)
        {

            CardPreviewController newCard = Instantiate(cardControllerPrefab, gridContent.transform.root);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, 1, gridContent.transform);

        }

    }

    public void AddChosenCard(Card card)
    {
        

        if(chosenCards.ContainsKey(card))
        {
            chosenCards[card] ++;
        }
        else
        {
            chosenCards.Add(card, 1);    
        }
        SetupChosenCardsView();
    }

    public void RemoveChosenCard(Card card)
    {

        if (chosenCards.ContainsKey(card))
        {
            chosenCards[card] --;

            if (chosenCards[card] <= 0)
            {
                chosenCards.Remove(card);
                Destroy(GameObject.Find(card.cardName));
            }
        }
        
        SetupChosenCardsView();
    }

    private void SetupChosenCardsView()
    {
        var image = GameObject.Find("ChosenCardGrid");

        foreach (KeyValuePair<Card, int> card in chosenCards)
        {
            var GameObjectFound = GameObject.Find(card.Key.cardName);
            if (GameObjectFound != null)
            {
                TextMeshProUGUI textMeshProUGUI = GameObjectFound.GetComponent<TextMeshProUGUI>();
                textMeshProUGUI.text = $"{card.Key.cardName} x {card.Value}";
            }
            else
            {

            GameObject gameObject = new GameObject(card.Key.cardName);
            TextMeshProUGUI textMeshProUGUI = gameObject.AddComponent<TextMeshProUGUI>();
            textMeshProUGUI.text = $"{card.Key.cardName} x {card.Value}";
            

            gameObject.transform.SetParent(image.transform);

            }

        }
    }

    private List<Card> ChosenCardsToList(Dictionary<Card, int> chosenCards)
    {
        List<Card> cards = new List<Card>();
        foreach (KeyValuePair<Card, int> card in chosenCards)
        {
            for(int i = 0; i < card.Value; i++)
            {
                cards.Add(card.Key);
            }
        }

            return cards;
    }

    private List<Card> DefaultDeck ()
    {
        List<Card> defaultCards = new List<Card>();

        defaultCards.Add(cards[0]);
        defaultCards.Add(cards[0]);
        defaultCards.Add(cards[1]);
        defaultCards.Add(cards[1]);
        defaultCards.Add(cards[2]);
        defaultCards.Add(cards[2]);

        return defaultCards;
    }


    public List<Card> RetrieveCardDeck()
    {
        if (customDeck)
        {
            return ChosenCardsToList(chosenCards);
        }
        return DefaultDeck();
    }
   
}
