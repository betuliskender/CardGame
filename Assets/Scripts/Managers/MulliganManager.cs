using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MulliganManager : MonoBehaviour
{
    public bool mulliganPhase = true;
    public List<Card> mulliganedCards = new List<Card>();
    public Transform player1HandArea, player2HandArea;
    public Button player1Button, player2Button;



    // Start is called before the first frame update
    void Start()
    {
        CardManager.instance.GenerateCards(player1HandArea, player2HandArea);
        SetupButton(player1Button, CardManager.instance.player1Hand, CardManager.instance.player2Hand);
    }
    private void SetupButton(Button button, List<CardController> cards1, List<CardController> cards2)
    {
        List<CardController> tempCardsPlayer1 = new();
        List<CardController> tempCardsPlayer2 = new();

        tempCardsPlayer1.AddRange(cards1);
        tempCardsPlayer2.AddRange(cards2);

        button.onClick.AddListener(() =>
        {
            foreach (Transform child in player1HandArea.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            foreach (Transform child in player2HandArea.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            CardManager.instance.GenerateCards(player1HandArea, player2HandArea);
            CardManager.instance.player1Hand = tempCardsPlayer1;
            CardManager.instance.player2Hand = tempCardsPlayer2;
            Debug.Log(CardManager.instance.player1Deck.cardStack.Count + " " + CardManager.instance.player2Deck.cardStack.Count);
            CardManager.instance.player1Deck.ReShuffleCards(tempCardsPlayer1);
            CardManager.instance.player2Deck.ReShuffleCards(tempCardsPlayer2);
            Debug.Log(tempCardsPlayer1.Count + " " + tempCardsPlayer2.Count);
            Debug.Log(CardManager.instance.player1Deck.cardStack.Count + " " + CardManager.instance.player2Deck.cardStack.Count);
            player1HandArea.transform.SetParent(CardManager.instance.player1HandArea);
            player2HandArea.transform.SetParent(CardManager.instance.player2HandArea);
        });

    }

 

        // Update is called once per frame
        void Update()
    {
        
    }

}
