using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D coll;

    [SerializeField]
    LayerMask pfLayerMask;

    [Range(0, 100)]
    public float moveMult = 20f;

    [Range(0, 100)]
    public float jumpMult = 20f;

    [Range(0, 100)]
    public float maxHSpeed = 10;

    private int jumpCounter = 0;
    private int jumpsLeft = 0;
    private List<bool> groundedFrames = new List<bool>();

    private bool grounded => groundedFrames[0] || groundedFrames[1] || groundedFrames[2];

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        for (int i = 0; i < 3; i++)
            groundedFrames.Add(true);
    }

    private void Update()
    {
        if (grounded && jumpCounter == 0)
        {
            jumpsLeft = 1;
            Vector2 jumpForce = KeyHandler.ReadJumpInput() * jumpMult;
            if (jumpForce.y > 0)
            {
                jumpCounter = 3;
                Vector2 vel = rb.velocity;
                vel.y = 0;
                rb.velocity = vel;
                rb.AddForce(jumpForce, ForceMode2D.Impulse);
            }
        }
        else if (!grounded && jumpsLeft > 0)
        {
            Vector2 jumpForce = KeyHandler.ReadJumpInput() * jumpMult;
            if (jumpForce.y > 0)
            {
                jumpsLeft--;
                Vector2 vel = rb.velocity;
                vel.y = 0;
                rb.velocity = vel;
                rb.AddForce(jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(KeyHandler.ReadDirInput() * moveMult);

        Vector2 vel = rb.velocity;
        if (Mathf.Abs(vel.x) > maxHSpeed) 
        { 
            vel.x = Mathf.Sign(vel.x) * maxHSpeed;
            rb.velocity = vel;
        }

        bool gr = IsGrounded();

        jumpCounter = Mathf.Max(0, jumpCounter-1);
        groundedFrames.RemoveAt(0);
        groundedFrames.Add(gr);
    }

    private bool IsGrounded()
    {
        // TODO: Change Cast direction on Gravity Change
        RaycastHit2D rcHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.down, 0.15f, pfLayerMask);
        return rcHit.collider != null;
    }

}
