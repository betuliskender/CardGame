using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbstractItemCard : AbstractCard, IHeal, IInstant
{
    public int healAmount;

    public AbstractItemCard(AbstractCard card, int healAmount) : base(card)
    {
        this.healAmount = healAmount;
    }



    public AbstractItemCard(AbstractItemCard itemCard) : base(itemCard)
    {

    }

    public AbstractItemCard(string cardName, int manaCost, int ownerID, Sprite illustration, int healAmount) : base(cardName, manaCost, ownerID, illustration)
    {
        this.healAmount = healAmount;
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
