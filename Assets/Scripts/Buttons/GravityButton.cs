using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityButton : ButtonControll
{
    protected override string helpText
    {
        get { return "change gravity"; }
    }

    override public void Toggle()
    {
        base.Toggle();
        player.physics.InvertJump();
        player.physics.InvertGravity();
    }
}
