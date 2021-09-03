using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserButtonHandler : StageButtonHandler
{
    public GameObject[] Locks;

    override public void Toggle(bool status, Collider2D c)
    {
        foreach(GameObject Lock in Locks)
        {
            GameObject b = Lock.transform.Find("Blocker").gameObject;
            b.SetActive(!b.activeSelf);
        }
    }
}
