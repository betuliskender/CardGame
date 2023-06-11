using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardGeneratorManager : MonoBehaviour
{
    public static CardGeneratorManager instance;
    public Sprite[] sprites;
    public Dictionary<string, AbstractCard> availableCards = new Dictionary<string, AbstractCard> ();

    private void Awake()
    {
        instance = this;
        availableCards.Add("Wither", new Wither(-1, sprites[0]));
        availableCards.Add("Whiskey", new Whiskey(-1, sprites[1]));
        availableCards.Add("Aberration", new Aberration(-1, sprites[2]));

    }

    /*public Wither getWither()
    {
        return (Wither) availableCards[1];
    }

    public Whiskey GetWhiskey()
    {
        return (Whiskey) availableCards[2];
    }

    public Aberration GetAberration()
    {
        return (Aberration)availableCards[3];
    }*/



    public List<string> getAvailableCards()
    {
        return availableCards.Keys.ToList();
    }
}
