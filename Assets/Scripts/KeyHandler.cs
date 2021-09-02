using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHandler : MonoBehaviour
{
    //public static Dictionary<KeyCode, string> keyCodes;

    public static bool enableSpace = true;
    public static bool enableNumbers = true;
    public static bool enableMovement = true;
    public static bool enableMouse = true;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        foreach (KeyCode key in keyCodes.Keys)
        {
            if (Input.GetKeyDown(key))
            {

            }
        }
        */
    }

    public static Vector2 ReadDirInput()
    {
        if (!enableMovement) return Vector3.zero;
        Vector2 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) dir.y -= 1;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) dir.x += 1;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) dir.y += 1;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) dir.x -= 1;
        return dir;
    }

    public static Vector2 ReadMousePos()
    {
        return Input.mousePosition;
    }

    public static bool ReadGravityButtonDown()
    {
        if (Input.GetKey(KeyCode.Q)) return true;
        return false;
    }

}
