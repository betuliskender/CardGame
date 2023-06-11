using UnityEngine;

[System.Serializable]
public class Card
{
    public string cardName;
    public int health, damage, manaCost, ownerID, sanityCost;
    public Sprite illustration;


    public Card(Card card)
    {
        cardName = card.cardName;
        health = card.health;
        damage = card.damage;
        manaCost = card.manaCost;
        illustration = card.illustration;
        sanityCost = card.sanityCost;
    }

    public void CardActionTest()
    {
    }

}
