using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI player1Health, player2Health, player1Mana, player2Mana;
    public GameObject playGameUI;
    public GameObject endGameUI;


    private void Awake()
    {
        instance = this;
    }

    public void GameFinished(int winner)
    {
        Debug.Log($"HE DED {winner}");

        endGameUI.SetActive(true);
        GameEndUIController.instance.Initialize(winner);

    }

    public void UpdateValues(Player player1, Player player2) 
    { 
        UpdateHealthValues(player1.health, player2.health);
        UpdateManaValues(player1.mana, player2.mana);
    }

    public void UpdateHealthValues(int player1Health, int player2Health)
    {
        this.player1Health.text = player1Health.ToString();
        this.player2Health.text = player2Health.ToString();
    }

    public void UpdateManaValues(int player1Mana, int player2Mana) 
    {
        this.player1Mana.text = player1Mana.ToString();
        this.player2Mana.text = player2Mana.ToString();
    }
}
