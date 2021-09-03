using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButtonHandler : StageButtonHandler
{
    //public GameObject Lock;
    public PlatformMover[] platforms;

    override public void Toggle(bool status, Collider2D c)
    {
        foreach (PlatformMover p in platforms)
            p.Activate(!p.activated);
    }
}
