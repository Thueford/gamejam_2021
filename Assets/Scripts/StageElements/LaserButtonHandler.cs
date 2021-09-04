using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserButtonHandler : StageButtonHandler
{
    public GameObject[] Locks;

    override public void Toggle(bool status, Collider2D c)
    {
        base.Toggle(status, c);
        foreach(GameObject Lock in Locks)
        {
            GameObject b = Lock.transform.Find("Blocker").gameObject;
            b.SetActive(!b.activeSelf);
            GameObject gc = Lock.transform.Find("laser_aus_complete_0").gameObject;
            bool aaa = Lock.transform.Find("laser_animated").GetComponent<Animator>().GetBool("isOff");
            gc.SetActive(!aaa);
            Lock.transform.Find("laser_animated").GetComponent<Animator>().SetBool("isOff", !aaa);
        }

    }
}
