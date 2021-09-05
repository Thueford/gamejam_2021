using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public GameObject spawn, goal, walls;
    public int DefaulJumps = 10, index;
    [HideInInspector]
    public bool restarted = false;

    private void Start()
    {
        if (!restarted) CamController.self.ResetBorders();
    }
}
