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
    public Transform player1Hand, player2Hand;
    public Button player1Button, player2Button;
    public CardController cardControllerPrefab;
    public List<CardController> player1Cards = new List<CardController>(),
        player2Cards = new List<CardController>();
    public bool mulliganPhase = true;


    private void Awake()
    {
        instance = this;
        player1Deck = new Deck(cards);
        player2Deck = new Deck(cards);
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

        Debug.Log("Entering game mode ...");
        GenerateCards();
    }

    
    private void GenerateCards()
    {
        foreach (Card card in player1Deck.StartHand())
        {
            CardController newCard = Instantiate(cardControllerPrefab, player1Hand.root);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, 0, player1Hand);
            player1Cards.Add(newCard);
        }
        foreach (Card card in player2Deck.StartHand())
        {
            CardController newCard = Instantiate(cardControllerPrefab, player2Hand.root);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, 1, player2Hand);
            player2Cards.Add(newCard);
        }

        if(mulliganPhase)
        {

            Debug.Log("Mulligan phase");
            //her fjerner player 1 de kort han ikke gider have på starthånden
            player1Button.onClick.AddListener(() =>
            {
                foreach (CardController card in player1Cards)
                {
                    Destroy(card.gameObject);
                }
                //skal nok ikke bruge clear her, det fjerner jo alle kort???? :( 
                player1Cards.Clear();

                //træk nye cards:
                for (int i = 0; i < player1Deck.StartHand().Count; i++)
                {
                    Card newCard = player1Deck.DrawCard();
                    CardController newCardController = Instantiate(cardControllerPrefab, player1Hand.root);
                    newCardController.transform.localPosition = Vector3.zero;
                    newCardController.Initialize(newCard, 0, player1Hand);
                    player1Cards.Add(newCardController);
                }
                //skal der tjekkes om begge spillere har mulliganet her?
                if (player2Cards.Count == player2Deck.StartHand().Count)
                {
                    mulliganPhase = false;
                    Debug.Log("Mulligan phase ended");
                    //StartGame(); ???????!!!?!?!?!?!?!?!?!?!?!
                }
            });
            //player 2 fjerner uønskede kort fra starthånden:
            player2Button.onClick.AddListener(() =>
            {
                foreach (CardController card in player2Cards)
                {
                    Destroy(card.gameObject);
                }
                //skal nok ikke bruge clear her, det fjerner jo alle kort??? :( 
                player2Cards.Clear();

                //træk nye kort
                for (int i = 0; i < player1Deck.StartHand().Count; i++)
                {
                    Card newCard = player1Deck.DrawCard();
                    CardController newCardController = Instantiate(cardControllerPrefab, player1Hand.root);
                    newCardController.transform.localPosition = Vector3.zero;
                    newCardController.Initialize(newCard, 0, player1Hand);
                    player1Cards.Add(newCardController);
                }
                // tjek om begge har mulliganet igen?
                if(player1Cards.Count == player1Deck.StartHand().Count)
                {
                    mulliganPhase=false;
                    Debug.Log("Mulligan phase ended");
                    //startgame ... how the fuck do I do this?
                }

            });
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
    }

    public void ProcessStartTurn(int ID)
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
        foreach (CardController card in cards)
        {
            if (card == null) continue;
            if (card.card.health <= 0)
            {
                Destroy(card.gameObject);
            }
        }
        foreach (CardController card in enemyCards)
        {
            if (card.card.health <= 0)
            {
                Destroy(card.gameObject);
            }
        }

        player1Cards.Clear();
        player2Cards.Clear();

        foreach (CardController card in cards)
        {
            if (card != null)
            {
                if (ID == 0)
                {
                    player1Cards.Add(card);
                }
                else
                {
                    player2Cards.Add(card);
                }
            }
        }
        foreach (CardController card in enemyCards)
        {
            if (card != null)
            {
                if (ID == 1)
                {
                    player1Cards.Add(card);
                }
                else
                {
                    player2Cards.Add(card);
                }
            }
        }
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
