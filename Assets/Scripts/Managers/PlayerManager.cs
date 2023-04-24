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

    internal void AssignTurn(int currentPlayerTurn)
    {
        foreach (Player player in playerList)
        {
            player.myTurn = player.ID == currentPlayerTurn;
            if (player.myTurn) player.mana = 10;
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

    private void PlayerLost(int ID)
    {
        UIManager.instance.GameFinished(ID == 0 ? FindPlayerByID(1) : FindPlayerByID(0));
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
