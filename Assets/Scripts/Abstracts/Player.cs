using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Player
{
    public int health, currentMana, maxMana, sanity;
    public int ID;
    public bool myTurn;

    public Player(int health, int mana, int ID)
    {
        this.health = health;
        this.currentMana = mana;
        this.ID = ID;
    }

    public Player(int health, int ID)
    {
        this.health = health;
        this.ID = ID;
    }

    public void IncreaseMana()
    {
        if(maxMana < 10)
        {
            maxMana++;
        }
    }

}
