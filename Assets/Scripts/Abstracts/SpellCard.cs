using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellCard : Card, IInstant
{
    public int SpellPower;

    

    public SpellCard(SpellCard spellCard) : base(spellCard)
    {
        spellCard.SpellPower = SpellPower;
    }

    public SpellCard(Card card, int spellPower) : base(card)
    {
        SpellPower = spellPower;
    }

    public void Instant() 
    {

        if(ownerID == 0)
        {
            PlayerManager.instance.DamagePlayer(1, SpellPower);
        }
        else
        {
            PlayerManager.instance.DamagePlayer(0, SpellPower);

        }
    }


}
