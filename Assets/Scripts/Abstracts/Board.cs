using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Board
{
    public int health;
    public int damage;

    public bool myTurn;

    public Board(int health, int damage)
    {
        this.health = health;
        this.damage = damage;
    }
}