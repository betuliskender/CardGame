using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameEndUIController : MonoBehaviour
{
    public TextMeshProUGUI winnerName;
    public Button restart, quit;

    private void Awake()
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        throw new NotImplementedException();
    }

    public void Initialize(Player winner)
    {
        winnerName.text = "Player: " + winner.ID + " has won!";
    }
    
}
