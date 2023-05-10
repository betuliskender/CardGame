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

    public int Heal()
    {
        return healAmount;
    }

    public void Instant()
    {
        base.CardActionTest();
        Debug.Log($"HEAL: {healAmount} cardName: {cardName}");
        PlayerManager.instance.HealPlayer(ownerID, Heal());
    }
}
