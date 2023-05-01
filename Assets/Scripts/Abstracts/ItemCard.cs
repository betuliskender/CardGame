using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemCard : Card, IHeal, IInstant
{
    public int healAmount;

    public ItemCard(Card card, int healAmount) : base(card)
    {
        this.healAmount = healAmount;
    }

    public ItemCard ()
    {

    }

    public int Heal(int playerHealth)
    {
        return playerHealth + healAmount;
    }

    public void Instant()
    {
        Debug.Log("HEAL");
    }
}
