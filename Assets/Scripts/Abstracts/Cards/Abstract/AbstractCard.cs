using UnityEngine;

[System.Serializable]
public abstract class AbstractCard
{
    public string cardName;
    public int manaCost, ownerID;
    public Sprite illustration;


    public AbstractCard(AbstractCard card)
    {
        cardName = card.cardName;
        manaCost = card.manaCost;
        illustration = card.illustration;
    }

    public AbstractCard(string cardName, int manaCost, int ownerID, Sprite illustration)
    {
        this.cardName = cardName;
        this.manaCost = manaCost;
        this.ownerID = ownerID;
        this.illustration = illustration;
    }

    public void CardActionTest()
    {
    }

}
