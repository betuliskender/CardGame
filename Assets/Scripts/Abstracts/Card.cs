using UnityEngine;

[System.Serializable]
public class Card
{
    public string cardName;
    public int health, damage, manaCost, ownerID;
    public Sprite illustration;

    public Card()
    {

    }

    public Card(Card card)
    {
        cardName = card.cardName;
        health = card.health;
        damage = card.damage;
        manaCost = card.manaCost;
        illustration = card.illustration;
    }

}
