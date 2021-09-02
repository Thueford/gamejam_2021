using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D coll;
    public bool isAirborne = false;

    [Range(0, 100)]
    public float moveMult = 20f;

    [Range(0, 100)]
    public float jumpMult = 20f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        rb.AddForce(KeyHandler.ReadDirInput() * moveMult);

        if (!isAirborne)
        {
            Vector2 jumpForce = KeyHandler.ReadJumpInput() * jumpMult;
            if (jumpForce.y > 0) 
            { 
                rb.AddForce(jumpForce, ForceMode2D.Impulse);
                isAirborne = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isAirborne = false;
    }
}
