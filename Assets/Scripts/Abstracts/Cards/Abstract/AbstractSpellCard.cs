using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbstractSpellCard : AbstractCard, IInstant
{
    public int SpellPower;



    public AbstractSpellCard(AbstractSpellCard spellCard) : base(spellCard)
    {
        spellCard.SpellPower = SpellPower;
    }

    public AbstractSpellCard(AbstractCard card, int spellPower) : base(card)
    {
        SpellPower = spellPower;

    }

    public AbstractSpellCard(string cardName, int manaCost, int ownerID, Sprite illustration, int spellPower) : base(cardName, manaCost, ownerID, illustration)
    {
        SpellPower = spellPower;
    }


    public void Instant()
    {

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
