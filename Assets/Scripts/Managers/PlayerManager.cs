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
        UIManager.instance.UpdateValues(playerList[0], playerList[1], playerList[2]);
    }

    internal void AssignTurn(int currentPlayerTurn, int currentTurn)
    {
        foreach (Player player in playerList)
        {
            player.myTurn = player.ID == currentPlayerTurn;

            if(currentPlayerTurn == 0 && currentPlayerTurn != 1)
            {
                player.IncreaseMana();
            }
            if (player.myTurn && player.ID == 0 || player.ID == 1)
            {
                player.currentMana = player.maxMana;
                UIManager.instance.UpdateManaValues(playerList[0].currentMana, playerList[1].currentMana);
            }
            
        }

    }

    public void BoardLogic()
    {
        if (CardManager.instance.AreThereCardsWithHealth(CardManager.instance.player1ActiveCards))
        {
                int randomCardIndex = UnityEngine.Random.Range(0, CardManager.instance.player1ActiveCards.Count);
                CardController randomCard = CardManager.instance.player1ActiveCards[randomCardIndex];
                randomCard.Damage(5);
        }
        else
        {
            DamagePlayer(0, 2);
        }

        if (CardManager.instance.AreThereCardsWithHealth(CardManager.instance.player2ActiveCards))
        {
            
            if (CardManager.instance.player2Hand.Count > 0)
            {
                int randomCardIndex = UnityEngine.Random.Range(0, CardManager.instance.player2ActiveCards.Count);
                CardController randomCard = CardManager.instance.player2ActiveCards[randomCardIndex];
                randomCard.Damage(5);
            }
        }
        else
        {
            DamagePlayer(1, 2);
        }

       
    }

    public void DamagePlayer(int ID, int damage)
    {
        Player player = FindPlayerByID(ID);
        player.health -= damage;
        UIManager.instance.UpdateHealthValues(playerList[0].health, playerList[1].health, playerList[2].health);


        if (playerList[0].health <= 0 || playerList[1].health <= 0)
        {
            PlayerLost(ID);
        }
    }

    public void HealPlayer(int ID, int healAmount)
    {
        Player player = FindPlayerByID(ID);
        player.health += healAmount;
        UIManager.instance.UpdateHealthValues(playerList[0].health, playerList[1].health, playerList[2].health);
    }

    private void PlayerLost(int playerID)
    {

        if (playerID == 0 || playerID == 1)
        {
            if (TurnManager.instance.isBoardActive)
            {
                UIManager.instance.GameFinished(2);
            }
            else
            {
                UIManager.instance.GameFinished(playerID == 0 ? 1: 0);
            }
        }
        
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
        FindPlayerByID(ownerID).currentMana -= manaCost;
        UIManager.instance.UpdateManaValues(playerList[0].currentMana, playerList[1].currentMana);
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
