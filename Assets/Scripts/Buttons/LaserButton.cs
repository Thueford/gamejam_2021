using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserButton : ButtonControll
{
    public LaserElement[] Locks;

    protected override string helpText
    {
        get { return "(de)activate Lasers"; }
    }

    override public void Toggle()
    {
        Debug.Log("ToggleLaser");
        base.Toggle();
        foreach (LaserElement Lock in Locks)
        {
            Lock.Activate(!Lock._activated);
        }
    }
}
