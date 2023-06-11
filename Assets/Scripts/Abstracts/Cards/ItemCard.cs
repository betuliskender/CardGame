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

    

    public ItemCard(ItemCard itemCard): base(itemCard)
    {

    }

    public int Heal()
    {
        return healAmount;
    }

    public void Instant()
    {
        base.CardActionTest();
        PlayerManager.instance.HealPlayer(ownerID, Heal());
    }
}
