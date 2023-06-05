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
        CardManager.instance.GenerateCards(player1HandArea, CardManager.instance.player1Hand, CardManager.instance.player1Deck, 0, 4);
        CardManager.instance.GenerateCards(player2HandArea, CardManager.instance.player2Hand, CardManager.instance.player2Deck, 1, 4);
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
            Debug.Log("Selected Cards Count: " + selectedCards.Count);
            transformParent();
        });
    }

    private void SetupButton(Button button, Transform playerHandArea, int player)
    {
        button.onClick.AddListener(() =>
        {
            if (TurnManager.instance.CurrentPlayerTurn == player)
            {
                SwapCards(selectedCards, playerHandArea, player);
                TurnManager.instance.ChangeActivePlayer();
                Debug.Log("Active player is now: " + TurnManager.instance.CurrentPlayerTurn);
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
        cards.Clear();
        int selectedCardCount = selectedCards.Count;

        foreach (Transform child in playerHandArea.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (CardController selectedCard in selectedCards)
        {
            cards.Remove(selectedCard);
            Destroy(selectedCard.gameObject);
        }

        if (player == 0)
        {
            CardManager.instance.GenerateCards(playerHandArea, cards, CardManager.instance.player1Deck, 0, selectedCardCount);
            CardManager.instance.player1Deck.ReShuffleCards(tempCardsPlayer);
        }
        else if (player == 1)
        {
            CardManager.instance.GenerateCards(playerHandArea, cards, CardManager.instance.player2Deck, 1, selectedCardCount);
            CardManager.instance.player2Deck.ReShuffleCards(tempCardsPlayer);
        }
        selectedCards.Clear();
        return tempCardsPlayer;
    }

    private void transformParent()
    {
        foreach (CardController card in selectedCards)
        {
            if (card.card.ownerID == 0)
            {
                card.transform.SetParent(player1HandArea);
                card.transform.localPosition = Vector3.zero;
                card.Initialize(card.card, 0, player1HandArea);
            }
            else if (card.card.ownerID == 1)
            {
                card.transform.SetParent(player2HandArea);
                card.transform.localPosition = Vector3.zero;
                card.Initialize(card.card, 1, player2HandArea);
            }
        }

        // Clear the selected cards list
        selectedCards.Clear();
    }




    // Update is called once per frame
    void Update()
    {
        
    }

}
