using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPhysics : MonoBehaviour
{
    
    public static Player player => Player.player;
    public static PlayerPhysics self;
    public Rigidbody2D rb { get; private set; }
    public Collider2D playerCollider;
    //[SerializeField]
    //LayerMask pfLayerMask;

    [Range(0, 100)]
    public float moveMult = 80;
    public float moveModifier = 1;

    [Range(0, 100)]
    public float jumpMult = 15f;

    [Range(0, 100)]
    public float maxHSpeed = 9;
    public float maxSpeedModifier = 1;


    private PlayerInput playerInput;
    private Vector2 playerVelocity = new Vector2();


    public static int jumps = 10000;
    public int maxJumps = 100;
    public bool airjump = true;
    public int jump_inverted = 1;

    private List<bool> groundedFrames = new List<bool>();
    //private bool grounded => groundedFrames[0] || groundedFrames[1] || groundedFrames[2];
    private bool grounded = false;

    private void Awake()
    {
        self = this;
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D >();

        playerInput = player.GetComponent<PlayerInput>();
        


    }


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
            groundedFrames.Add(true);

        playerInput.actions.FindAction("Move").canceled += ctx => 
        {
            playerVelocity = Vector2.zero;
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.AddForce(playerVelocity);
        Vector3 max_velocity = Vector3.ClampMagnitude(rb.velocity, maxHSpeed * maxSpeedModifier);
        max_velocity.y = rb.velocity.y;
        rb.velocity = max_velocity;

        if (player.transform.position.y < PlayerCamera.self.bottom)
        {
            player.Die();
        }
    }

    public void InvertJump()
    {
        if (jump_inverted > 0)
        {
            jump_inverted = -1;
        } else
        {
            jump_inverted = 1;
        }
    }

    public void InvertGravity()
    {
        player.spriteRenderer.flipY = !player.spriteRenderer.flipY;
        rb.gravityScale = rb.gravityScale * -1;
    }
    public void AddJump()
    {
        jumps++;
    }

    public void ResetPhysics()
    {
        jumps = maxJumps;
        maxSpeedModifier = 1;
        moveModifier = 1;
    }

    public void OnJump(InputValue value)
    {
        if (jumps <= 0) return;
        if (!grounded && !airjump) return;

        Vector2 vel = rb.velocity;
        vel.y = 0;
        rb.velocity = vel;
        rb.AddForce(transform.up * jump_inverted * jumpMult * moveModifier, ForceMode2D.Impulse);

        jumps--;
        if (!grounded) airjump = false;
        player.UpdateUI();

        //Vector2 jumpForce = jumpMult * PlayerController.inverted * KeyHandler.ReadJumpInput();
        //SoundHandler.PlayClip("jump");
    }

    public void OnMove(InputValue value)
    {
        playerVelocity = value.Get<Vector2>() * moveMult;
        //Debug.Log(context);
        //Vector2 mov2 = new Vector2(20, 0);

        //Debug.Log(mov);
        //Debug.Log(value.isPressed);
        //rb.AddForce(mov);
        //Vector3.ClampMagnitude(rb.velocity, 10);


    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        /*if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Floor"))
        {
            RaycastHit2D rcHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0, Vector2.down * jump_inverted, 0.15f);
            if (rcHit.collider)
            {
                Debug.Log("Hit");
                grounded = true;
                airjump = true;
            }

            //todo: Fix when player jumps from the bottom against a wall and gets a free jump
        }
        else */if (collision.gameObject.CompareTag("Kill"))
        {
            player.Die();
        }
    }
    /*
    void OnCollisionExit2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Floor"))
        {
            grounded = false;
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Spikes") || collider.CompareTag("Kill")) player.Die();
        else if (collider.CompareTag("Platform") || collider.CompareTag("Floor"))
        {
            grounded = true;
            airjump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Platform") || collider.CompareTag("Floor")) grounded = false;
    }
}
