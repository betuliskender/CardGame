using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public int currentPlayerTurn;
    public int currentTurn = 1;
    public bool mulliganPhase = true;
    public int CurrentPlayerTurn => currentPlayerTurn;
    public bool isBoardActive = false;
    public TextMeshProUGUI boardText;
    private bool isBoardDeadTextShown = false;
    private bool isShowBoardTextShown = false;

    private void Awake()
    {
        if (instance == null)
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
        CardManager.instance.ProcessCardsAtStartTurn(CardManager.instance.player1ActiveCards, CardManager.instance.player2ActiveCards);

        if (currentTurn == 5 && !isShowBoardTextShown)
        {
            isShowBoardTextShown = true;
            isBoardActive = true;
            StartCoroutine(ShowBoardText());
        }

        if (PlayerManager.instance.playerList[2].health <= 0 && !isBoardDeadTextShown)
        {
            isBoardDeadTextShown = true;
            StartCoroutine(BoardDeadText());
        }

    }

    private IEnumerator ShowBoardText()
    {
        boardText.gameObject.SetActive(true);
        boardText.text = "Prepare, mortals, for your feeble grasp on sanity shall be shattered!";

        yield return new WaitForSeconds(5f);
        boardText.gameObject.SetActive(false);
        UIManager.instance.boardHealth.gameObject.SetActive(true);
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

        if(isBoardActive)
        {
            PlayerManager.instance.BoardLogic();
        }
        UpdateCardVisibility();


    }

    private IEnumerator BoardDeadText()
    {
        UIManager.instance.boardHealth.gameObject.SetActive(false);
        boardText.gameObject.SetActive(true);
        boardText.text = "The ancient one falls but the fight still continues. Kill your oponent to win the game!";

        yield return new WaitForSeconds(5f);
        boardText.gameObject.SetActive(false);
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