using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.UI;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI player1Health, player2Health, player1Mana, player2Mana, player1Sanity, player2Sanity, boardHealth;
    public GameObject playGameUI;
    public GameObject endGameUI;


    private void Awake()
    {
        instance = this;
    }

    public void GameFinished(int winner)
    {
        endGameUI.SetActive(true);
         if (winner == 2)
    {
        GameEndUIController.instance.InitializeBoardWin();
    }
    else
    {
        GameEndUIController.instance.Initialize(winner);
    }

    }

    public void UpdateValues(Player player1, Player player2, Player board) 
    { 
        UpdateHealthValues(player1.health, player2.health, board.health);
        UpdateManaValues(player1.currentMana, player2.currentMana);
        UpdateSanityValues(player1.sanity, player2.sanity);
    }

    public void UpdateHealthValues(int player1Health, int player2Health, int boardHealth)
    {
        this.player1Health.text = player1Health.ToString();
        this.player2Health.text = player2Health.ToString();
        this.boardHealth.text = boardHealth.ToString(); 
    }

    public void UpdateManaValues(int player1Mana, int player2Mana) 
    {
        this.player1Mana.text = player1Mana.ToString();
        this.player2Mana.text = player2Mana.ToString();
    }

    public void UpdateSanityValues(int player1Sanity, int player2Sanity)
    {
        this.player1Sanity.text = player1Sanity.ToString();
        this.player2Sanity.text = player2Sanity.ToString();
    }
}
