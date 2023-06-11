using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Whiskey : AbstractItemCard
{


    public Whiskey(AbstractItemCard itemCard) : base(itemCard)
    {

    }

    public Whiskey(int ownerId, Sprite illustration) : base("Whiskey", 2, ownerId, illustration, 2)
    {

    }

    public new void Instant()
    {
        base.CardActionTest();
        PlayerManager.instance.HealPlayer(ownerID, Heal());
    }


}
