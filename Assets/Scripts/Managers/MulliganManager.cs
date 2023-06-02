using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MulliganManager : MonoBehaviour
{
    public Transform player1HandArea, player2HandArea;
    public Button player1Button, player2Button, endMulligan;
    public static bool isMulliganActive = true;

    // Start is called before the first frame update
    void Start()
    {
        CardManager.instance.GenerateCards(player1HandArea, CardManager.instance.player1Hand, CardManager.instance.player1Deck);
        CardManager.instance.GenerateCards(player2HandArea, CardManager.instance.player2Hand, CardManager.instance.player2Deck);
        SetupButton(player1Button, CardManager.instance.player1Hand, player1HandArea, 1);
        SetupButton(player2Button, CardManager.instance.player2Hand, player2HandArea, 2);
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

    private void SetupButton(Button button, List<CardController> cards, Transform playerHandArea, int player)
    {
        

        button.onClick.AddListener(() =>
        {
            SwapCards(cards, playerHandArea, player);
        });

    }

    private void SwapCards(List<CardController> cards, Transform playerHandArea, int player)
    {

        List<CardController> tempCardsPlayer = new();

        tempCardsPlayer.AddRange(cards);

        foreach (Transform child in playerHandArea.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        if(player == 1)
        {
        CardManager.instance.GenerateCards(playerHandArea, CardManager.instance.player1Hand, CardManager.instance.player1Deck);
        CardManager.instance.player1Deck.ReShuffleCards(tempCardsPlayer);
        }
        if (player == 2)
        {
            CardManager.instance.GenerateCards(playerHandArea, CardManager.instance.player2Hand, CardManager.instance.player2Deck);
            CardManager.instance.player2Deck.ReShuffleCards(tempCardsPlayer);
        }
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
