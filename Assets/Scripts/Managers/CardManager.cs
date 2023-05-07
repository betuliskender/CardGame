using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public List<Card> cards = new List<Card>();
    
    public Deck player1Deck, player2Deck;
    public Stack<CardController> player1DiscardPile = new Stack<CardController>(), player2DiscardPile = new Stack<CardController>();
    public Transform player1Discard, player2Discard;
    public Transform player1Hand, player2Hand;
    public Button player1Button, player2Button;
    public CardController cardControllerPrefab;
    public List<CardController> player1Cards = new List<CardController>(),
        player2Cards = new List<CardController>();


    private void Awake()
    {
        instance = this;
        //EditDeckManager.instance.ChosenCardsToList(EditDeckManager.instance.chosenCards);
        player1Deck = new Deck(EditDeckManager.instance.RetrieveCardDeck());
        player2Deck = new Deck(EditDeckManager.instance.RetrieveCardDeck());
        SetupButton(player1Button, player1Hand, 0, player1Deck);
        SetupButton(player2Button, player2Hand, 1, player2Deck);

    }

    private void SetupButton(Button button, Transform hand, int ID, Deck deck)
    {
        button.onClick.AddListener(() =>
        {
            var card = deck.DrawCard();
            CardController newCard = Instantiate(cardControllerPrefab, hand.root);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, ID, ID == 0 ? player1Hand : player2Hand);
        });

        
    }

    private void Start()
    {
        GenerateCards();
    }

    private void GenerateCards()
    {
        foreach (Card card in player1Deck.StartHand())
        {
            CardController newCard = Instantiate(cardControllerPrefab, player1Hand.root);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, 0, player1Hand);
        }
        foreach (Card card in player2Deck.StartHand())
        {
            CardController newCard = Instantiate(cardControllerPrefab, player2Hand.root);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, 1, player2Hand);
        }
    }

    public void PlayCard(CardController card, int ID)
    {
        if (ID == 0)
        {
            
               
            player1Cards.Add(card);
        }
        else
        {
            player2Cards.Add(card);
        }

        card.ActionCall();
    }

    public void DiscardCard(CardController card)
    {
        
            CardController newCard = Instantiate(cardControllerPrefab, card.card.ownerID == 0 ? player1Discard.root : player1Discard.root);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card.card, card.card.ownerID, card.card.ownerID == 0 ? player1Discard : player2Discard);

            player1DiscardPile.Push(newCard);
            Debug.Log(player1DiscardPile.Count);
            Destroy(card.gameObject);


       

    }

    public void ProcessCardsAtStartTurn(List<CardController> Player1Cards, List<CardController> Player2Cards)
    {
        List<CardController> Player1RemainingCards = new List<CardController>();
        List<CardController> Player2RemainingCards = new List<CardController>();

        Player1RemainingCards.AddRange(CullDeadCards(Player1Cards));
        Player2RemainingCards.AddRange(CullDeadCards(Player2Cards));

        this.player1Cards.Clear();
        this.player2Cards.Clear();

        this.player1Cards = Player1RemainingCards;
        this.player2Cards = Player2RemainingCards;

    }

    public List<CardController> CullDeadCards(List<CardController> cards)
    {
        List<CardController> remainingCards = new List<CardController>();

        foreach (CardController card in cards)
        {
            if (card == null) continue;
            if (card.card.health <= 0)
            {
                DiscardCard(card);
                Destroy(card.gameObject);
            }

            if(card != null)
            {
                remainingCards.Add(card);
            }
        }

        return remainingCards;
    }


    public void ProcessEndTurn(int ID)
    {
        List<CardController> cards = new List<CardController>();
        List<CardController> enemyCards = new List<CardController>();
        if (ID == 0)
        {
            cards.AddRange(player1Cards);
            enemyCards.AddRange(player2Cards);
        }
        else
        {
            cards.AddRange(player2Cards);
            enemyCards.AddRange(player1Cards);
        }
        foreach (CardController cardController in cards)
        {
            if (cardController == null) continue;
            if (AreThereCardsWithHealth(enemyCards))
            {
                int randomEnemyCard = Random.Range(0, enemyCards.Count);
                while (enemyCards[randomEnemyCard].card.health <= 0)
                {
                    randomEnemyCard = Random.Range(0, enemyCards.Count);
                }
                enemyCards[randomEnemyCard].Damage(cardController.card.damage);
                cardController.transform.SetParent(cardController.transform.root);
                cardController.transform.DOMove(enemyCards[randomEnemyCard].transform.position, 0.35f, true).onComplete += () =>
                {
                    cardController.ReturnToHand();
                };
                cardController.Damage(enemyCards[randomEnemyCard].card.damage);
            }
            else
            {
                int enemyID = ID == 0 ? 1 : 0;
                cardController.transform.SetParent(cardController.transform.root);
                cardController.transform.DOMove(ID == 0 ? player2Hand.transform.position : player1Hand.transform.position, 0.35f, true).onComplete += () =>
                {
                    cardController.ReturnToHand();
                };
                PlayerManager.instance.DamagePlayer(enemyID, cardController.card.damage);
            }

        }
    }

    private bool AreThereCardsWithHealth(List<CardController> cards)
    {
        bool cardHasHealth = false;
        foreach (CardController card in cards)
        {
            if (card.card.health > 0)
            {
                cardHasHealth = true;
            }
        }

        return cardHasHealth;
    }
}
