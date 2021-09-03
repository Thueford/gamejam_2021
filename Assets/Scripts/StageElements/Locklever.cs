using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locklever : StageButtonHandler
{
    //public GameObject Lock;
    public GameObject[] Locks;
    public bool leverStatus = false;

    override public void Toggle(bool status, Collider2D c)
    {
        foreach(GameObject Lock in Locks)
        {

            bool isActiv = Lock.transform.Find("Blocker").gameObject.active;
            Lock.transform.Find("Blocker").gameObject.SetActive(!isActiv);
        }

        
    }
}
