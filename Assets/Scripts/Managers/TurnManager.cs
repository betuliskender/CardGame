using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public int currentPlayerTurn;
    private int currentTurn = 1;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
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
        CardManager.instance.ProcessCardsAtStartTurn(CardManager.instance.player1Cards, CardManager.instance.player2Cards);

    }

    public void EndTurn()
    {
        CardManager.instance.ProcessEndTurn(currentPlayerTurn);
        StartCoroutine(WaitForAttacks(currentPlayerTurn == 0 ? CardManager.instance.player1Cards.Count: CardManager.instance.player2Cards.Count));
        currentPlayerTurn = currentPlayerTurn == 0 ? 1 : 0;

        if (currentPlayerTurn == 0)
        {
            currentTurn++;
        }

    }

    private IEnumerator WaitForAttacks(float cards)
    {
        yield return new WaitForSeconds(cards * 0.35f);
        StartTurn();
    }
}