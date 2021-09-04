using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidController : MonoBehaviour
{
    public static bool moveright;
    public static bool moveleft;
    public static bool jump;
    public static bool E;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeftArrow()
    {
        moveright = false;
        moveleft = true;
    }
    public void RightArrow()
    {
        moveright = true;
        moveleft = false;
    }
    public void ReleaseLeftArrow()
    {
        moveleft = false;
    }
    public void ReleaseRightArrow()
    {
        moveright = false;

    }
    public void UpArrow()
    {
        jump = true;
    }
    public void ReleaseUpArrow()
    {
        jump = false;
    }

    public void EButton()
    {
        E = true;
    }

    public void EButton_Release()
    {
        E = false;
    }
}
