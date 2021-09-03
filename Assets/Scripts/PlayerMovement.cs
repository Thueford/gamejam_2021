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
    [Range(0, 100)]
    public float terminalVelocity = 10;

    public int jumps { get; private set; } = 10;

    private int jumpCounter = 0;
    private int airJumpsLeft = 0;
    private List<bool> groundedFrames = new List<bool>();

    private Animator anim;
    public bool gaaaaaaa;

    private bool grounded => groundedFrames[0] || groundedFrames[1] || groundedFrames[2];

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        for (int i = 0; i < 3; i++)
            groundedFrames.Add(true);
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        gaaaaaaa = grounded;
        if (jumps <= 0) return;

        //Debug.Log(grounded);

        if (grounded && jumpCounter == 0)
        {
            airJumpsLeft = 1;
            Jump();
        }
        else if (!grounded && airJumpsLeft > 0)
        {
            if (Jump()) airJumpsLeft--;
        }

        
    }

    void FixedUpdate()
    {
        Vector2 moveForce = KeyHandler.ReadDirInput() * moveMult;
        if (moveForce.x != 0)
        {
            anim.SetBool("isWalking", true);
            rb.AddForce(moveForce);
            PlayerController.self.sr.flipX = moveForce.x < 0;
        } else
        {
            anim.SetBool("isWalking", false);
        }

        Vector2 vel = rb.velocity;
        if (Mathf.Abs(vel.x) > maxHSpeed) 
        { 
            vel.x = Mathf.Sign(vel.x) * maxHSpeed;
        }

        if (-vel.y * PlayerController.inverted > terminalVelocity)
        {
            vel.y = Mathf.Sign(vel.y) * terminalVelocity;
        }

        if (!grounded) {
            if (-vel.y * PlayerController.inverted >= 0)
            {
                anim.SetBool("isFalling", false);
            } else if (-vel.y * PlayerController.inverted < 0) {
                anim.SetBool("isFalling", true);
                //anim.SetBool("isJumping", false);
            } 
        }

        rb.velocity = vel;

        bool gr = IsGrounded();

        jumpCounter = Mathf.Max(0, jumpCounter-1);
        groundedFrames.RemoveAt(0);
        groundedFrames.Add(gr);
    }

    private bool Jump()
    {
        Vector2 jumpForce = jumpMult * PlayerController.inverted * KeyHandler.ReadJumpInput();
        if (jumpForce.y != 0)
        {
            anim.SetBool("isJumping", true);
            Invoke("SetJumpingToFalse", 0.283f);
            jumpCounter = 3;
            jumps--;
            Vector2 vel = rb.velocity;
            vel.y = 0;
            rb.velocity = vel;
            rb.AddForce(jumpForce, ForceMode2D.Impulse);
        }
        return jumpForce.y > 0;
    }

    private void SetJumpingToFalse()
    {
        anim.SetBool("isJumping", false);
    }

    private bool IsGrounded()
    {
        // TODO: Change Cast direction on Gravity Change
        RaycastHit2D rcHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.down * PlayerController.inverted, 0.15f, pfLayerMask);
        return rcHit.collider != null;
    }

    public void AddJumps(int n)
    {
        if (n > 0) jumps += n;
    }

    public void SetJumps(int n)
    {
        jumps = n;
    }
}
