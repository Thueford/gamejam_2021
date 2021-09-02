using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHandler : MonoBehaviour
{
    public static Dictionary<KeyCode, string> keyCodes;

    private void Awake()
    {
        keyCodes = new Dictionary<KeyCode, string>()
        {
            {KeyCode.A, "left" },
            {KeyCode.W, "up" },
            {KeyCode.S, "down" },
            {KeyCode.D, "right" },
            {KeyCode.E, "interact" }
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
