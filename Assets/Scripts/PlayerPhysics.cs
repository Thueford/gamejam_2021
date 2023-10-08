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
    public CircleCollider2D playerGroundedCollider;
    //[SerializeField]
    //LayerMask pfLayerMask;

    [Range(0, 30)]
    public float moveMult = 2.5f;
    public float moveModifier = 1;

    [Range(0, 100)]
    public float jumpMult = 15f;

    [Range(0, 20)]
    public float maxHSpeed = 8;
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

    public float groundedTimerTime = 10.2f;
    private float groundedTimer;

    public ButtonControll currentInteraction = null;

    private void Awake()
    {
        self = this;
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D >();
        playerGroundedCollider = GetComponent<CircleCollider2D>();

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
        //calculate if is inverted
        if (jump_inverted < 0)
        {
            Vector2 tmpVel = playerVelocity;
            tmpVel.y *= -1;
            rb.AddForce(tmpVel);

        } else rb.AddForce(playerVelocity, ForceMode2D.Impulse);

        if (playerVelocity == Vector2.zero)
        {
            Vector2 tmpVel = rb.velocity;
            tmpVel.x /= 2;
            rb.velocity = tmpVel;
        }

        Vector3 max_velocity = Vector3.ClampMagnitude(rb.velocity, maxHSpeed * maxSpeedModifier);
        max_velocity.y = rb.velocity.y;
        rb.velocity = max_velocity;

        if (player.transform.position.y < PlayerCamera.self.bottom)
        {
            player.Die();
        }

        groundedTimer -= Time.deltaTime;
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

        // invert the grounded collider
        playerGroundedCollider.offset *= -1;


    }
    public void AddJump()
    {
        jumps++;
        airjump = true;
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
        if (!grounded && !airjump && groundedTimer < 0) return;

        Vector2 vel = rb.velocity;
        vel.y = 0;
        rb.velocity = vel;
        rb.AddForce(transform.up * jump_inverted * jumpMult * moveModifier, ForceMode2D.Impulse);

        jumps--;
        //if (!grounded && groundedTimer < 0) airjump = false;
        if (!grounded && groundedTimer < 0) airjump = false;
        groundedTimer -= 1;
        player.UpdateUI();

        //Vector2 jumpForce = jumpMult * PlayerController.inverted * KeyHandler.ReadJumpInput();
        //SoundHandler.PlayClip("jump");
    }

    public void OnMove(InputValue value)
    {
        playerVelocity = value.Get<Vector2>() * moveMult;
        playerVelocity.y = 0;
    }

    public void OnInteract(InputValue value)
    {
        if (currentInteraction)
        {
            currentInteraction.Toggle();
        }
    }

    public void DisableInput()
    {
        playerInput.actions.Disable();
        playerInput.actions.FindAction("Pause").Enable();
    }

    public void EnableInput()
    {
        playerInput.actions.Enable();
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Kill"))
        {
            player.Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Spikes") || collider.CompareTag("Kill")) player.Die();
        else if (collider.CompareTag("Platform") || collider.CompareTag("Floor"))
        {
            grounded = true;
            airjump = true;
        }
        else if (collider.CompareTag("Interact")) {
            currentInteraction = collider.gameObject.GetComponent(typeof(ButtonControll)) as ButtonControll;
            currentInteraction.OnEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Platform") || collider.CompareTag("Floor"))
        {
            grounded = false;
            groundedTimer = groundedTimerTime;
        }
        else if (collider.CompareTag("Interact"))
        {
            ButtonControll obj = collider.gameObject.GetComponent(typeof(ButtonControll)) as ButtonControll;
            obj.OnExit();

            currentInteraction = null;
        }
    }
}
