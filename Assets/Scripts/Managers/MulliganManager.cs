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
        SetupButton(player1Button, CardManager.instance.player1Hand);
    }
    private void SetupButton(Button button, List<CardController> cards)
    {
        List<CardController> tempCards = new();
        tempCards.AddRange(cards);
        Debug.Log("BEFORE SHUFFLE" + CardManager.instance.player1Deck.cardStack.Count);

        button.onClick.AddListener(() =>
        {
            Debug.Log("CLICKED ON SWAP");
            foreach (Transform child in player1HandArea.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            // tempCards.ForEach((card) =>
            //{
            //   Destroy(card.gameObject);
            //});
            CardManager.instance.GenerateCards(player1HandArea, player2HandArea);
            CardManager.instance.player1Hand.Clear();
            CardManager.instance.player1Hand = tempCards;
            CardManager.instance.player1Deck.ReShuffleCards(tempCards);
            Debug.Log("AFTER SHUFFLE" + CardManager.instance.player1Deck.cardStack.Count);
        });

    }

 

        // Update is called once per frame
        void Update()
    {
        
    }

}
