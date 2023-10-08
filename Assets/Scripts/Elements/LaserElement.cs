using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserElement : BaseElement
{
    public bool baseActivated = false;
    public static Player player => Player.player;
    public bool _activated;
    // Start is called before the first frame update
    void Start()
    {
        player.elements.Add(GetComponent<LaserElement>());
        initStart();
    }

    public override void Reset()
    {
        base.Reset();

        initStart();
        Activate(baseActivated);
    }

    public void initStart()
    {
        _activated = baseActivated;
        if (!_activated)
        {
            GameObject b = transform.Find("Blocker").gameObject;
            b.SetActive(false);
            GameObject gc = transform.Find("laser_aus_complete_0").gameObject;

            gc.SetActive(true);
            transform.Find("laser_animated").GetComponent<Animator>().SetBool("isOff", true);
        }
    }

    public void Activate(bool value)
    {
        GameObject blocker = transform.Find("Blocker").gameObject;
        blocker.SetActive(value);
        
        Animator laserAnimation = transform.Find("laser_animated").GetComponent<Animator>();
        laserAnimation.SetBool("isOff", !value);
        _activated = value;
    }
}
