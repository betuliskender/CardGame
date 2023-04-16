using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
   public static PlayerManager instance;
   public List<Player> PlayerList = new List<Player>();

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UIManager.instance.UpdateValues(PlayerList[0], PlayerList[1]);
    }

    internal void AssignTurn(int currentPlayerTurn)
    {
        foreach (Player player in PlayerList)
        {
            player.myTurn = player.ID == currentPlayerTurn;
            if (player.myTurn) player.mana = 5;
        }
    }

    public void DamagePlayer(int ID, int damage)
    {
        Player player = FindPlayerByID(ID);
        player.health -= damage;
        if(player.health <= 0 )
        {
            PlayerLost(ID);
        }
    }

    private void PlayerLost(int ID)
    {
        UIManager.instance.GameFinished(ID ==0?FindPlayerByID(1) : FindPlayerByID(0));
    }

    public Player FindPlayerByID(int ID)
    {
        Player foundPlayer = null;

        foreach (Player player in PlayerList)
        {
            foundPlayer = player;
        }
        return foundPlayer;
    }

   
}
