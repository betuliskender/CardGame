using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Aberration : AbstractMonsterCard
{


    public Aberration(AbstractMonsterCard monsterCard) : base(monsterCard)
    {

    }

    public Aberration(int ownerId, Sprite illustration) : base("Aberration", 5, ownerId, illustration, 4, 3)
    {

    }


}
