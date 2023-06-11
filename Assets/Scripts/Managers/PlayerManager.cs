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

    internal void SpendSanity(int ownerID, int sanityCost)
    {
        FindPlayerByID(ownerID).sanity -= sanityCost;
        UIManager.instance.UpdateSanityValues(playerList[0].sanity, playerList[1].sanity);
    }

    public void RegainPlayerSanity(int ID, int sanityAmount)
    {
        Player player = FindPlayerByID(ID);
        player.sanity += sanityAmount;
        UIManager.instance.UpdateSanityValues(playerList[0].health, playerList[1].health);
    }

    public void SpendCardCost(CardController card)
    {
        SpendMana(card.card.ownerID, card.card.manaCost);
        SpendSanity(card.card.ownerID, card.card.sanityCost);
    }


}
