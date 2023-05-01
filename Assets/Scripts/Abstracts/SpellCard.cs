using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellCard : Card, IInstant
{
    public SpellCard()
    {
    }

    public SpellCard(Card card) : base(card)
    {
    }

    public void Instant()
    {
        Debug.Log("Iskrystaller is krystaller ");
    }


}
