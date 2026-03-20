    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<Button>().onClick.AddListener(delegate { SoundHandler.PlayClick(); });
        
        Button[] buttons = FindObjectsOfType<Button>(true); // parameter makes it include inactive UI elements with buttons
        foreach (Button b in buttons)
        {
            b.onClick.AddListener(delegate { SoundHandler.PlayClick(); });
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
