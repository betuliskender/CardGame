using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemCard : Card, IHeal, IInstant, IRegainSanity
{
    public int healAmount;
    public int sanityAmount;

    public ItemCard(Card card, int healAmount, int sanityAmount) : base(card)
    {
        this.healAmount = healAmount;
        this.sanityAmount = sanityAmount;
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
        PlayerManager.instance.RegainPlayerSanity(ownerID, RegainSanity());
    }

    public int RegainSanity()
    {
        return sanityAmount;
    }
}
