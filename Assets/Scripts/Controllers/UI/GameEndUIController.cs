using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Reflection;

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

    internal void InitializeBoardWin()
    {
        winnerName.text = $"You lost against the evil darkness and are now lost in time and space";
    }
}
