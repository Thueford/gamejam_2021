using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public GameObject spawn, walls;
    public int DefaulJumps = 10;
    public int index;

    private void Start() => CamController.ResetBorders();
}
