using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityButtonHandler : MonoBehaviour
{
    public GameObject ButtonImage;
    public GameObject ButtonText;
    public static float GravityTimer = 2f;
    public static bool isGravityTimer = false;
    private Rigidbody2D rb;
    private bool buttonActive = false;
    private KeyCode gravityButtonKey = KeyCode.E;

    // Start is called before the first frame update
    void Start()
    {
        ButtonImage.SetActive(false);
        isGravityTimer = false;
    }

    // Update is called once per frame
    void Update()
    {

        /*
         * Debug.Log(GravityTimer);
        if (GravityTimer > 0 && isGravityTimer)
        {
            GravityTimer -= Time.deltaTime;
        } else if (GravityTimer < 0 && isGravityTimer)
        {
            PlayerController.InvertGravity();
            isGravityTimer = false;
            if (rb != null) rb.gravityScale = rb.gravityScale * -1;
        } */
        

        if (buttonActive)
        {
            if (Input.GetKeyDown(gravityButtonKey))
            {
                //ButtonImage
                Debug.Log("changeGravity");
                isGravityTimer = true;
                GravityTimer = 2f;
                rb.gravityScale = rb.gravityScale * -1;
                PlayerController.InvertGravity();
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
