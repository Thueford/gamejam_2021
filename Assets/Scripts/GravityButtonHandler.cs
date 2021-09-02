using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityButtonHandler : MonoBehaviour
{
    public GameObject ButtonImage;
    public GameObject ButtonText;
    private Rigidbody2D rb;
    private bool buttonActive = false;
    private KeyCode gravityButtonKey = KeyCode.E;

    // Start is called before the first frame update
    void Start()
    {
        ButtonImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonActive)
        {
            if (Input.GetKeyDown(gravityButtonKey))
            {
                //ButtonImage
                Debug.Log("changeGravity");
                rb.gravityScale = rb.gravityScale * -1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            // show context help -> press e to change gravity
            ButtonImage.SetActive(true);
            buttonActive = true;
            rb = collider.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            // hide context help
            ButtonImage.SetActive(false);
            buttonActive = false;
            

        }
    }
}
