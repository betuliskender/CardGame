using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
   public static PlayerManager instance;
   public List<Player> playerList = new List<Player>();

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UIManager.instance.UpdateValues(playerList[0], playerList[1]);
    }

    internal void AssignTurn(int currentPlayerTurn, int currentTurn)
    {
        foreach (Player player in playerList)
        {
            player.myTurn = player.ID == currentPlayerTurn;
            if (player.myTurn && player.ID == 0 || player.ID == 1)
            {
                player.mana = currentTurn;
                UIManager.instance.UpdateManaValues(playerList[0].mana, playerList[1].mana);
            }
        }
    }

    public void DamagePlayer(int ID, int damage)
    {
        Player player = FindPlayerByID(ID);
        player.health -= damage;
        UIManager.instance.UpdateHealthValues(playerList[0].health, playerList[1].health);


        if (player.health <= 0)
        {
            PlayerLost(ID);
        }
    }

    public void HealPlayer(int ID, int healAmount)
    {
        Player player = FindPlayerByID(ID);
        player.health += healAmount;
        UIManager.instance.UpdateHealthValues(playerList[0].health, playerList[1].health);


    }

    private void PlayerLost(int ID)
    {
        Debug.Log(ID);
        UIManager.instance.GameFinished(ID == 0 ? 1 : 0);
    }

    public Player FindPlayerByID(int ID)
    {
        Player foundPlayer = null;

        foreach (Player player in playerList)
        {
            if (player.ID == ID)
            foundPlayer = player;
        }
        return foundPlayer;
    }

    internal void SpendMana(int ownerID, int manaCost)
    {
        FindPlayerByID(ownerID).mana -= manaCost;
        UIManager.instance.UpdateManaValues(playerList[0].mana, playerList[1].mana);
    }


}
