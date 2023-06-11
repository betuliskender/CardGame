using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wither : AbstractSpellCard
{


    public Wither(AbstractSpellCard spellCard) : base(spellCard)
    {
        Debug.Log(spellCard.SpellPower);
        spellCard.SpellPower = SpellPower;
    }

    public Wither(int ownerId, Sprite illustration) : base("Wither", 2, ownerId, illustration, 2)
    {

    }

    public new void Instant()
    {
        Debug.Log("Denne er fra Wither Class");
        if (ownerID == 0)
        {
            PlayerManager.instance.DamagePlayer(1, SpellPower);
        }
        else
        {
            PlayerManager.instance.DamagePlayer(0, SpellPower);

        }
    }


}
