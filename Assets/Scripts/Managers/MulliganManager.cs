using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MulliganManager : MonoBehaviour
{
    public Transform player1HandArea, player2HandArea;
    public Button player1Button, player2Button, endMulligan;
    private static MulliganManager _instance;
    public static bool isMulliganActive = true;
    public List<CardController> selectedCards = new List<CardController>();

    public static MulliganManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MulliganManager>();
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        CardManager.instance.GenerateCards(player1HandArea, CardManager.instance.player1Hand, CardManager.instance.player1Deck, 0);
        CardManager.instance.GenerateCards(player2HandArea, CardManager.instance.player2Hand, CardManager.instance.player2Deck, 1);
        SetupButton(player1Button, player1HandArea, 0);
        SetupButton(player2Button, player2HandArea, 1);
        ChangeScene(endMulligan);
    }

    private void ChangeScene(Button button)
    {
        button.onClick.AddListener(() =>
        {
            isMulliganActive = false;
            Debug.Log("MulliganPhase is now over");
            TurnManager.instance.mulliganPhase = false;
            transformParent();
        });
    }

    private void SetupButton(Button button, Transform playerHandArea, int player)
    {
        button.onClick.AddListener(() =>
        {
        if (TurnManager.instance.CurrentPlayerTurn == player)
        {
            List<CardController> activePlayerCards = player == 0 ? CardManager.instance.player1Hand : CardManager.instance.player2Hand;
            SwapCards(activePlayerCards, playerHandArea, player);
            TurnManager.instance.ChangeActivePlayer();
            Debug.Log("Active player is now: " + TurnManager.instance.CurrentPlayerTurn);
            selectedCards.Clear();
            CardController.instance.selectedCards.Clear();
            }
            else
            {
                Debug.Log("Button clicked, but it's not the current player's turn. Current player: " + TurnManager.instance.CurrentPlayerTurn);
            }
        });
    }

    private List<CardController> SwapCards(List<CardController> cards, Transform playerHandArea, int player)
    {
        List<CardController> tempCardsPlayer = new List<CardController>(selectedCards);
        Debug.Log("Amount of cards in the temp list: " + tempCardsPlayer.Count);
        selectedCards.Clear(); // Clear selectedCards before destroying the old cards

        //foreach (Transform child in playerHandArea.transform)
        //{
        //    GameObject.Destroy(child.gameObject);
       // }

        foreach (CardController selectedCard in tempCardsPlayer)
        {
            Destroy(selectedCard.gameObject);
            cards.Remove(selectedCard);
        }

        if (player == 0)
        {
            CardManager.instance.DrawMulliganedCards(playerHandArea, cards, CardManager.instance.player1Deck, 0, tempCardsPlayer.Count);
            CardManager.instance.player1Deck.ReShuffleCards(tempCardsPlayer);
        }
        else if (player == 1)
        {
            CardManager.instance.DrawMulliganedCards(playerHandArea, cards, CardManager.instance.player2Deck, 1, tempCardsPlayer.Count);
            CardManager.instance.player2Deck.ReShuffleCards(tempCardsPlayer);
        }

        return tempCardsPlayer;
    }

    private void transformParent()
    {
        foreach (CardController card in CardManager.instance.player1Hand)
        {
            card.transform.SetParent(CardManager.instance.player1HandArea.root);
            card.transform.localPosition = Vector3.zero;
            card.Initialize(card.card, 0, CardManager.instance.player1HandArea);
        }

        foreach (CardController card in CardManager.instance.player2Hand)
        {
            card.transform.SetParent(CardManager.instance.player2HandArea.root);
            card.transform.localPosition = Vector3.zero;
            card.Initialize(card.card, 1, CardManager.instance.player2HandArea);
        }
    }




    // Update is called once per frame
    void Update()
    {
        
    }

}
