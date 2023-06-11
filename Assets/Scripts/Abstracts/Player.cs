using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Player
{
    public int health, mana, sanity;
    public int ID;
    public bool myTurn;

    public Player(int health, int mana, int ID)
    {
        this.health = health;
        this.mana = mana;
        this.ID = ID;
    }

    public Player(int health, int ID)
    {
        this.health = health;
        this.ID = ID;
    }

    public void IncreaseMana()
    {
        if(mana < 10)
        {
            mana++;
        }
    }

}
