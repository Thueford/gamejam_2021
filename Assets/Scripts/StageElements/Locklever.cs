using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locklever : Leverable
{
    public GameObject Lock;

    override public void Toggle(bool status, Collider2D c)
    {
        Lock.transform.Find("Lock").gameObject.SetActive(!status);
    }
}
