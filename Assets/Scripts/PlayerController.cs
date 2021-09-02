using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject respawnPoint;
    public GameObject InvertGravityPanel;
    public bool triggerRespawn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (KeyHandler.ReadRespawnButtonDown()) Respawn();
    }

    /*
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Tag_GravityButton")
        {
            // show context help -> press e to change gravity
            gravityButton = collider;
            //InvertGravityPanel.SetActive(true);
            //Debug.Log("press e to change gravity");
        }       
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Tag_GravityButton")
        {
            // hide context help
            gravityButton = null;
            //InvertGravityPanel.SetActive(false);
            
        }
    }*/

    private void Respawn()
    {
        //Debug.Log("move to respawn!");
        if (respawnPoint)
        {
            transform.position = respawnPoint.transform.position;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
