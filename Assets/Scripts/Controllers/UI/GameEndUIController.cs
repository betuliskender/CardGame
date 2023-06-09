using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameEndUIController : MonoBehaviour
{
    public static GameEndUIController instance;
    public TextMeshProUGUI winnerName;
    public Button restart, quit;
 

    private void Awake()
    {
        instance = this;
        SetupButtons();
    }

    private void SetupButtons()
    {
        restart.onClick.AddListener(() =>
        {
            MulliganManager.isMulliganActive = true;
            RestartMatch();
        });
        quit.onClick.AddListener(() =>
        {
            QuitGame();
        });
    }

    private void QuitGame()
    {
        //if(Application.isEditor)
        //{
        //    EditorApplication.ExitPlaymode();
        //} else
        //{
        //}
            Application.Quit();
    }

    private void RestartMatch()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Initialize(int winner)
    {
        winnerName.text = $"Player: {winner+1} has won!";
    }
    
}
