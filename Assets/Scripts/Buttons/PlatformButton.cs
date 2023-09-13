using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButton : ButtonControll
{
    public PlatformMover[] platforms;

    protected override string helpText
    {
        get { return "(de)activate Platforms"; }
    }

    override public void Toggle()
    {
        foreach (PlatformMover p in platforms)
            p.Activate(!p.activated);
    }
}
