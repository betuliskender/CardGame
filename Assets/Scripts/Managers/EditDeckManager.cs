using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class EditDeckManager : MonoBehaviour
{

    public static EditDeckManager instance;
   
    public Dictionary<Card, int> playerOneChosenCards = new Dictionary<Card, int>();
    public Dictionary<Card, int> playerTwoChosenCards = new Dictionary<Card, int>();

    public List<Card> cards = new List<Card>();
    public List<SpellCard> spellCards = new List<SpellCard>();
    public List<ItemCard> itemCards = new List<ItemCard>();
    public CardPreviewController cardControllerPrefab;
    private bool customDeck = false;
    private bool playerSelect = false;
    public Button confirmbutton;
    public Button switchPlayerButton;
    public TextMeshProUGUI playerText;


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

        switchPlayerButton.onClick.AddListener(() =>
        {
            playerSelect = !playerSelect;
            ClearChosenCards();

            if(!playerSelect)
            {
                SetupChosenCardsView(playerOneChosenCards);
                playerText.text = "Player 1";
            }
            else
            {
                SetupChosenCardsView(playerTwoChosenCards);
                playerText.text = "Player 2";

            }
        });
    }

    private void SetupAvailableCards()
    {

        foreach (Card card in cards)
        {
            AddCardToGrid(card);

        }

        foreach (Card card in spellCards)
        {

            AddCardToGrid(card);

        }

        foreach (Card card in itemCards)
        {

            AddCardToGrid(card);

        }

    }

    private void AddCardToGrid(Card card)
    {
        var gridContent = GameObject.Find("AvailableCardsGrid");
       
            CardPreviewController newCard = Instantiate(cardControllerPrefab, gridContent.transform.root);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, 1, gridContent.transform);
    }

    public void AddChosenCard(Card card)
    {
        if (!playerSelect)
        {

        if(playerOneChosenCards.ContainsKey(card))
        {
                playerOneChosenCards[card] ++;
        }
        else
        {
                playerOneChosenCards.Add(card, 1);    
        }

        SetupChosenCardsView(playerOneChosenCards);

        }
        else
        {
            if (playerTwoChosenCards.ContainsKey(card))
            {
                playerTwoChosenCards[card]++;
            }
            else
            {
                playerTwoChosenCards.Add(card, 1);
            }
            SetupChosenCardsView(playerTwoChosenCards);

        }
    }

    


    public void RemoveChosenCard(Card card)
    {
        if (!playerSelect)
        {

            if (playerOneChosenCards.ContainsKey(card))
            {
                playerOneChosenCards[card]--;

                if (playerOneChosenCards[card] <= 0)
                {
                    playerOneChosenCards.Remove(card);
                    Destroy(GameObject.Find(card.cardName));
                }
            }
        SetupChosenCardsView(playerOneChosenCards);
        }
        else
        {
            if (playerTwoChosenCards.ContainsKey(card))
            {
                playerTwoChosenCards[card]--;

                if (playerTwoChosenCards[card] <= 0)
                {
                    playerTwoChosenCards.Remove(card);
                    Destroy(GameObject.Find(card.cardName));
                }
            }
            SetupChosenCardsView(playerTwoChosenCards);
        }

        
    }

    private void SetupChosenCardsView(Dictionary<Card, int> chosenCards)
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
        defaultCards.Add(cards[3]);
        defaultCards.Add(cards[3]);
        defaultCards.Add(cards[4]);
        defaultCards.Add(cards[4]);

        return defaultCards;
    }


    public List<Card> RetrieveCardDeck(int player)
    {
        if (customDeck && player == 1)
        {
            if(CheckDeckSize(playerOneChosenCards))
            return ChosenCardsToList(playerOneChosenCards);
        }

        if (customDeck && player == 2)
        {
            if (CheckDeckSize(playerTwoChosenCards))
                return ChosenCardsToList(playerTwoChosenCards);
        }
        return DefaultDeck();
    }

    private void ClearChosenCards()
    {
        var image = GameObject.Find("ChosenCardGrid");

        for (int i = 0; i < image.transform.childCount; i++)
            Destroy(image.transform.GetChild(i).gameObject);
    }

    private bool CheckDeckSize(Dictionary<Card, int> chosenCards)
    {
        int cardAmount = 0;

        foreach (KeyValuePair<Card, int> card in chosenCards)
        {
            cardAmount += card.Value;
        }

        if(cardAmount >= 10)
        {
            return true;
        }

        return false;
    }

    private void Update()
    {
        
    }

}
