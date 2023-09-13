using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFire : DamageElementBase
{
    protected override float damage
    {
        get { return 30; }
    }

    public override float repeatingTime
    {
        get { return .03f; }
    }
}
