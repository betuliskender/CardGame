using UnityEngine;

[System.Serializable]
public class Card
{
    public string cardName;
    public int health, damage, manaCost, ownerID;
    public Sprite illustration;


    public Card(Card card)
    {
        cardName = card.cardName;
        health = card.health;
        damage = card.damage;
        manaCost = card.manaCost;
        illustration = card.illustration;
    }

    public Card(string cardName, int health, int damage, int manaCost, int ownerID, Sprite illustration)
    {
        this.cardName = cardName;
        this.health = health;
        this.damage = damage;
        this.manaCost = manaCost;
        this.ownerID = ownerID;
        this.illustration = illustration;
    }

    public void CardActionTest()
    {
    }

}
