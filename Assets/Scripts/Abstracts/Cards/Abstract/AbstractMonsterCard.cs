using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbstractMonsterCard : AbstractCard
{
    public int health, damage;

    public AbstractMonsterCard(AbstractCard card, int health, int damage) : base(card)
    {
        this.health = health;
        this.damage = damage;
    }



    public AbstractMonsterCard(AbstractMonsterCard monsterCard) : base(monsterCard)
    {

    }

    public AbstractMonsterCard(string cardName, int manaCost, int ownerID, Sprite illustration, int health, int damage) : base(cardName, manaCost, ownerID, illustration)
    {
        this.health = health;
        this.damage = damage;
    }

    
}
