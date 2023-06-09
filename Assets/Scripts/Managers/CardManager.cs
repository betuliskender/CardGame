using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public List<Card> cards = new List<Card>();
    
    public Deck player1Deck, player2Deck;
    public Stack<CardController> player1DiscardPile = new Stack<CardController>(), player2DiscardPile = new Stack<CardController>();
    public Transform player1Discard, player2Discard;
    public Transform player1HandArea, player2HandArea;
    public Button player1Button, player2Button;
    public CardController cardControllerPrefab;
    public List<CardController> player1ActiveCards = new List<CardController>(),
        player2ActiveCards = new List<CardController>();
    public List<CardController> player1Hand= new List<CardController>(),
        player2Hand = new List<CardController>();


    private void Awake()
    {
        instance = this;

    }

    private void SetupButton(Button button, Transform hand, int ID, Deck deck)
    {
        button.onClick.AddListener(() =>
        {
            Debug.Log(deck.cardStack.Count);
            var card = deck.DrawCard();
            CardController newCard = Instantiate(cardControllerPrefab, hand.root);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, ID, ID == 0 ? player1HandArea : player2HandArea);
        });

      
    }

    private void Start()
    {
        instance = this;
        //EditDeckManager.instance.ChosenCardsToList(EditDeckManager.instance.chosenCards);
        player1Deck = new Deck(EditDeckManager.instance.RetrieveCardDeck(1));
        player2Deck = new Deck(EditDeckManager.instance.RetrieveCardDeck(2));
        SetupButton(player1Button, player1HandArea, 0, player1Deck);
        SetupButton(player2Button, player2HandArea, 1, player2Deck);
    }

    public void GenerateCards(Transform playerMulliganArea, List<CardController> playerHand, Deck deck, int playerID)
    {
            playerHand.Clear();
           
        foreach (Card card in deck.StartHand())
        {
            CardController newCard = Instantiate(cardControllerPrefab, playerMulliganArea.root);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, playerID, playerMulliganArea);
            playerHand.Add(newCard);
            //Debug.Log(playerHand.Count);
        }
      
    }

    public void DrawMulliganedCards(Transform playerMulliganArea, List<CardController> playerHand, Deck deck, int playerID, int amount)
    {
        List<Card> cards = deck.DrawXAmountOfCards(amount);
        foreach (Card card in cards) 
        {
                CardController newCard = Instantiate(cardControllerPrefab, playerMulliganArea.root);
                newCard.transform.localPosition = Vector3.zero;
                newCard.Initialize(card, playerID, playerMulliganArea);
                playerHand.Add(newCard);
                //Debug.Log(playerHand.Count);
            
        }

    }

    public void PlayCard(CardController card, int ID)
    {
        if (ID == 0)
        {
            
               
            player1ActiveCards.Add(card);
        }
        else
        {
            player2ActiveCards.Add(card);
        }

        card.ActionCall();
    }

    public void DiscardCard(CardController card)
    {
        
            CardController newCard = Instantiate(cardControllerPrefab, card.card.ownerID == 0 ? player1Discard.root : player1Discard.root);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card.card, card.card.ownerID, card.card.ownerID == 0 ? player1Discard : player2Discard);
            card.isDiscarded = true;
            Debug.Log("CARD DISCARDED");
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

        this.player1ActiveCards.Clear();
        this.player2ActiveCards.Clear();

        this.player1ActiveCards = Player1RemainingCards;
        this.player2ActiveCards = Player2RemainingCards;

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
                //Destroy(card.gameObject);
            }

            if(card != null)
            {
                remainingCards.Add(card);
            }
        }

        return remainingCards;
    }


    public void ProcessEndTurn(List<CardController> attackingPlayerCards, List<CardController> enemyPlayerCards)
    {
        List<CardController> cards = new List<CardController>();
        List<CardController> enemyCards = new List<CardController>();
        
            cards.AddRange(attackingPlayerCards);
            enemyCards.AddRange(enemyPlayerCards);
        
        foreach (CardController cardController in cards)
        {
            if (cardController == null) continue;
            AttackEnemyPlayer(enemyCards, cardController);
        }
    }

    private void AttackEnemyPlayer(List<CardController> enemyCards, CardController cardController)
    {
        if (AreThereCardsWithHealth(enemyCards))
        {
            AttackCards(enemyCards, cardController);
        }
        else
        {
            AttackPlayer(cardController);
        }
    }

    private void AttackPlayer(CardController cardController)
    {
        int enemyID = cardController.card.ownerID == 0 ? 1 : 0;
        cardController.transform.SetParent(cardController.transform.root);
        cardController.transform.DOMove(cardController.card.ownerID == 0 ? player2HandArea.transform.position : player1HandArea.transform.position, 0.35f, true).onComplete += () =>
        {
            cardController.ReturnToHand();
        };
        PlayerManager.instance.DamagePlayer(enemyID, cardController.card.damage);
    }

    private static void AttackCards(List<CardController> enemyCards, CardController cardController)
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
