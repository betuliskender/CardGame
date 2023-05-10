using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellCard : Card, IInstant
{
    public int SpellPower;

    public SpellCard()
    {

    }

    public SpellCard(SpellCard spellCard) : base(spellCard)
    {

    }

    public SpellCard(Card card, int spellPower) : base(card)
    {
        SpellPower = spellPower;
    }

    public void Instant() 
    {
        
        Debug.Log($"Dette er fra SpellCard med SpellPower: {this.SpellPower} og OwnerId {this.ownerID}");
    }


}
