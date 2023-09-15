using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserButton : ButtonControll
{
    public DeactivateLaser[] Locks;

    protected override string helpText
    {
        get { return "(de)activate Lasers"; }
    }

    override public void Toggle()
    {
        Debug.Log("ToggleLaser");
        base.Toggle();
        foreach (DeactivateLaser Lock in Locks)
        {
            Lock.Activate(!Lock.activated);
        }
    }
}
