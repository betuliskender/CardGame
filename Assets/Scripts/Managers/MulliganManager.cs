using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MulliganManager : MonoBehaviour
{
    public Transform player1HandArea, player2HandArea;
    public Button player1Button, player2Button, endMulligan;
    private static MulliganManager _instance;
    public static bool isMulliganActive = true;
    public List<CardController> selectedCards = new List<CardController>();
    public bool player2HasMulliganed = false;
    public TextMeshProUGUI assignText;
    public TextMeshProUGUI notYourTurnText;

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
            TurnManager.instance.currentPlayerTurn = 0;
            transformParent();
        });
    }

    private void SetupButton(Button button, Transform playerHandArea, int player)
    {
        button.onClick.AddListener(() =>
        {
        if (TurnManager.instance.CurrentPlayerTurn == player && !player2HasMulliganed)
        {
            List<CardController> activePlayerCards = player == 0 ? CardManager.instance.player1Hand : CardManager.instance.player2Hand;
            SwapCards(activePlayerCards, playerHandArea, player);
            TurnManager.instance.ChangeActivePlayer();
            selectedCards.Clear();
            }
            else if(player2HasMulliganed)
            {
                Debug.Log("Button clicked, but you can only mulligan once. Hit end mulligan to start the game");
            }
            else
            {
                StartCoroutine(NotYourTurnText());
            }
        });
    }

    private List<CardController> SwapCards(List<CardController> cards, Transform playerHandArea, int player)
    {
        List<CardController> tempCardsPlayer = new List<CardController>(selectedCards);
        selectedCards.Clear();

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
            player2HasMulliganed = true;
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

    public IEnumerator ShowPlayerTurnText(int ID)
    {
        assignText.gameObject.SetActive(true);
        assignText.text = $"Player{ID + 1}'s turn!";

        yield return new WaitForSeconds(5f);
        assignText.gameObject.SetActive(false);
    }
    public IEnumerator NotYourTurnText()
    {
        assignText.gameObject.SetActive(true);

        assignText.text = $"Oops! That's not your button!";

        yield return new WaitForSeconds(4f);
        assignText.gameObject.SetActive(false);
    }
}
