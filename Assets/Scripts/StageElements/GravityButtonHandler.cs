using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityButtonHandler : Leverable
{
    public static float GravityTimer = 2f;
    [Range(1, 50)]
    public float GravityDuration = 2f;
    public static bool isGravityTimer = false;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        isGravityTimer = false;
    }

    // Update is called once per frame
    public void Update()
    {
        /*
        Debug.Log(GravityTimer);
        if (GravityTimer > 0 && isGravityTimer)
        {
            GravityTimer -= Time.deltaTime;
        }
        else if (GravityTimer < 0 && isGravityTimer)
        {
            PlayerController.InvertGravity();
            isGravityTimer = false;
            if (rb != null) rb.gravityScale = rb.gravityScale * -1;
        }
        */
    }

    override public void Toggle(bool status, Collider2D c)
    {
        Debug.Log("changeGravity");
        rb = c.GetComponent<Rigidbody2D>();
        isGravityTimer = true;
        GravityTimer = 2f;
        GravityTimer = GravityDuration;
        rb.gravityScale = rb.gravityScale * -1;
        PlayerController.InvertGravity();
    }
}
