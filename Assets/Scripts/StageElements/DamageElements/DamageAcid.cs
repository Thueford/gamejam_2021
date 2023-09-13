using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAcid : DamageElementBase
{
    protected override float damage
    {
        get { return 2.5f; }
    }

    public override float repeatingTime
    {
        get { return .05f; }
    }

    public override void DoHit()
    {
        base.DoHit();
        player.physics.moveModifier = 0.3f;
        player.physics.maxSpeedModifier = 0.3f; 
    }

    public override void DoExit()
    {
        base.DoExit();
        player.physics.moveModifier = 1;
        player.physics.maxSpeedModifier = 1;
    }
}
