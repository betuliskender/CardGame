using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public int currentPlayerTurn;
    private int currentTurn = 1;
    public bool mulliganPhase = true;
    public int CurrentPlayerTurn => currentPlayerTurn;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        //mulliganPhase = true;
        StartTurnGamePlay(0);
    }

    public void StartTurnGamePlay(int playerID)
    {
        
            currentPlayerTurn = playerID;
            StartTurn();
    }

    public void StartTurn()
    {
        GamePlayUIController.instance.UpdateCurrentPlayerTurn(currentPlayerTurn);
        PlayerManager.instance.AssignTurn(currentPlayerTurn, currentTurn);
        //CardManager.instance.ProcessStartTurn(currentPlayerTurn);
        CardManager.instance.ProcessCardsAtStartTurn(CardManager.instance.player1ActiveCards, CardManager.instance.player2ActiveCards);

    }

    public void EndTurn()
    {
        if (currentPlayerTurn == 0 )
        {
        CardManager.instance.ProcessEndTurn(CardManager.instance.player1ActiveCards, CardManager.instance.player2ActiveCards);
        }
        else
        {
            CardManager.instance.ProcessEndTurn(CardManager.instance.player2ActiveCards, CardManager.instance.player1ActiveCards);
        }
        StartCoroutine(WaitForAttacks(currentPlayerTurn == 0 ? CardManager.instance.player1ActiveCards.Count: CardManager.instance.player2ActiveCards.Count));

        currentPlayerTurn = currentPlayerTurn == 0 ? 1 : 0;

        if (currentPlayerTurn == 0)
        {
            currentTurn++;
        }
        UpdateCardVisibility();


    }

    public void ChangeActivePlayer()
    {
        currentPlayerTurn = (currentPlayerTurn + 1) % 2;
        Debug.Log("Active player changed to: " + currentPlayerTurn);
        UpdateCardVisibility();
    }

    public void UpdateCardVisibility()
    {
        int currentPlayerID = currentPlayerTurn;
        CardController[] allCards = FindObjectsOfType<CardController>();

        foreach (CardController card in allCards)
        {
            card.UpdateVisibility(currentPlayerID);
        }
    }

    private IEnumerator WaitForAttacks(float cards)
    {
        yield return new WaitForSeconds(cards * 0.35f);
        StartTurn();
    }
}