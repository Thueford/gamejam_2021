using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMud : DamageElementBase
{
    protected override float damage
    {
        get { return .5f; }
    }

    public override float repeatingTime
    {
        get { return 1; }
    }

    public override void DoHit()
    {
        base.DoHit();
        player.physics.moveModifier = 0.2f;
        player.physics.maxSpeedModifier = 0.2f;
    }

    public override void DoExit()
    {
        base.DoExit();
        player.physics.moveModifier = 1;
        player.physics.maxSpeedModifier = 1;
    }
}
