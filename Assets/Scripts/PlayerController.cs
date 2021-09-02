using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Tag_GravityButton")
        {
            // show context help -> press e to change gravity
            Debug.Log("press e to change gravity");
        }
            
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Tag_GravityButton")
        {
            // hide context help
        }
    }
}
