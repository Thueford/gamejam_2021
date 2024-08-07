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

    [SerializeField]
    LayerMask pfLayerMask;
    //[SerializeField]
    //LayerMask pfLayerMask;

    [Range(0, 30)]
    public float moveMult = 18f;
    public float moveModifier = 1;

    [Range(0, 100)]
    public float jumpMult = 1.5f;

    [Range(0, 20)]
    public float maxHSpeed = 6;
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
    private bool jumped = false;
    PlatformMover pm;
    private Vector2 platformVel;

    public float groundedTimerTime = 10.2f;
    private float groundedTimer;

    public ButtonControll currentInteraction = null;
    public List<DamageElementBase> damageElementBases = new List<DamageElementBase>();

    public bool interactSwitch = false;
    public bool inFan = false;

    public Animator anim;


    //sound
    private bool moving = false;

    private void Awake()
    {
        self = this;
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D >();
        playerGroundedCollider = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();

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

        SoundHandler.StopWalk();
        // player.Respawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        //calculate if is inverted

        /*if (jump_inverted < 0)
        {
            Vector2 tmpVel = playerVelocity;
            tmpVel.y *= -1;
            rb.AddForce(tmpVel);

        } else rb.AddForce(playerVelocity, ForceMode2D.Impulse);
        //*/

        Vector2 vel = new Vector2(playerVelocity.x, rb.velocity.y + playerVelocity.y);

        /*if (playerVelocity == Vector2.zero)
        {
            Vector2 tmpVel = rb.velocity;
            tmpVel.x /= 2;
            rb.velocity = tmpVel;
        }*/

        

        Vector2 max_velocity = Vector2.ClampMagnitude(vel, maxHSpeed);
        max_velocity.y = rb.velocity.y;
        vel = max_velocity;

        vel *= moveModifier;

        if (player.transform.position.y < PlayerCamera.self.bottom)
        {
            player.Die();
        }

        //IsGrounded();
        if (pm != null)
        {
            vel.x += pm.customVelocity.x;//(vel.x - platformVel.x) * 0.9f + platformVel.x;
            /*Vector3 tempPos = transform.position;
            tempPos.x += platformVel.x;
            transform.position = tempPos;*/
        }

        
        if (rb.velocity.y * jump_inverted < -0.3)
        {
            if (anim.GetBool("isJumping")) anim.SetBool("isJumping", false);
            if (!anim.GetBool("isFalling")) anim.SetBool("isFalling", true);
        } else
        {
            anim.SetBool("isFalling", false);
        }//*/



        rb.velocity = vel;

        groundedTimer -= Time.deltaTime;
        
        if (rb.velocity == Vector2.zero)
        {
            //SoundHandler.StopWalk();
        }

        //TODO
        //if (groundedTimer >= 1) RaycastDoubleCheck();
    }

    private bool IsGrounded()
    {
        // TODO: Change Cast direction on Gravity Change
        //RaycastHit2D rcHit = Physics2D.BoxCast(playerGroundedCollider.bounds.center, playerGroundedCollider.bounds.size, 0, (jump_inverted < 0 ? Vector2.down : Vector2.up) * PlayerController.inverted, 0.15f, pfLayerMask);
        RaycastHit2D rcHit = Physics2D.BoxCast(playerGroundedCollider.bounds.center, playerGroundedCollider.bounds.size, 0, Vector2.down * jump_inverted, 0.15f, pfLayerMask);
        if (rcHit.collider && rcHit.collider.gameObject.CompareTag("Platform"))
        {
            Rigidbody2D r = rcHit.collider.GetComponent<Rigidbody2D>();
            if (r)
            {
                platformVel = new Vector2(r.velocity.x, r.velocity.y);
                pm = rcHit.collider.GetComponent<PlatformMover>();
                return true;
            }
        }
        platformVel = Vector2.zero;
        return rcHit.collider != null;


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
        Vector3 tmp = playerGroundedCollider.offset;
        tmp.y *= -1;
        playerGroundedCollider.offset = tmp;


    }
    public void AddJump()
    {
        jumps++;
        airjump = true;
    }

    public void ResetPhysics()
    {
        

        foreach (DamageElementBase damageElementBase in damageElementBases) {
            damageElementBase.Reset();

        }
        damageElementBases.Clear();

        jumps = maxJumps;
        maxSpeedModifier = 1;
        moveModifier = 1;
    }

    public void OnJump(InputValue value)
    {
        if (jumps <= 0) return;
        if (!grounded && !airjump && groundedTimer < 0) return;
        if (groundedTimer >= 0 && !airjump) return;

        anim.SetBool("isJumping", true);
        SoundHandler.PlayClip("jump");

        Vector2 vel = rb.velocity;
        vel.y = 0;
        rb.velocity = vel;

        rb.AddForce(transform.up * jump_inverted * jumpMult * moveModifier, ForceMode2D.Impulse);

        jumps--;

        //if (!grounded && groundedTimer < 0) airjump = false;

        if (!grounded && groundedTimer < 0 || jumped) airjump = false;
        groundedTimer -= 1;
        player.UpdateUI();
        jumped = true;

        RaycastDoubleCheck();

        //Vector2 jumpForce = jumpMult * PlayerController.inverted * KeyHandler.ReadJumpInput();
        //SoundHandler.PlayClip("jump");
    }

    public void OnMove(InputValue value)
    {
        playerVelocity = value.Get<Vector2>() * moveMult;
        playerVelocity.y = 0;

        if (playerVelocity.x < 0 && transform.localScale.x > 0)
        {
            Vector3 tmpScale = transform.localScale;
            tmpScale.x *= -1;
            transform.localScale = tmpScale;
        } else if (playerVelocity.x > 0 && transform.localScale.x < 0)
        {
            Vector3 tmpScale = transform.localScale;
            tmpScale.x *= -1;
            transform.localScale = tmpScale;
        }

        if (playerVelocity.x != Vector2.zero.x)
        {
            anim.SetBool("isWalking", true);
            SoundHandler.StartWalk();

        } else
        {
            anim.SetBool("isWalking", false);
            SoundHandler.StopWalk();
        }
    }


    public void OnInteract(InputValue value)
    {
        if (currentInteraction)
        {
            currentInteraction.Toggle();
        }

        if (inFan)
        {
            if (interactSwitch)
            {
                //playerVelocity.y = -1 * jump_inverted;
                interactSwitch = false;
            }
            else
            {
                //playerVelocity.y = 0;
                interactSwitch = true;
            }
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
            jumped = false;

            if (collider.CompareTag("Platform")) {
                
                pm = collider.GetComponent<PlatformMover>();
                //transform.SetParent(collider.transform, false);
                //Debug.Log(pm.customVelocity);
                //platformVel = new Vector2(r.velocity.x, r.velocity.y);
                //platformVel = pm.customVelocity;
            } else
            {
                pm = null;
                //RaycastDoubleCheck();
            }
            
        }
        else if (collider.CompareTag("Interact")) {
            currentInteraction = collider.gameObject.GetComponent(typeof(ButtonControll)) as ButtonControll;
            currentInteraction.OnEnter();
        } else
        {
            //Debug.Log("Reset");
            //pm = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Platform") || collider.CompareTag("Floor"))
        {
            grounded = false;
            groundedTimer = groundedTimerTime;
            //platformVel = Vector2.zero;
            pm = null;

            RaycastDoubleCheck();
            

        }
        else if (collider.CompareTag("Interact"))
        {
            ButtonControll obj = collider.gameObject.GetComponent(typeof(ButtonControll)) as ButtonControll;
            obj.OnExit();

            currentInteraction = null;
        }
    }

    private void RaycastDoubleCheck()
    {
        RaycastHit2D rcHit = Physics2D.BoxCast(playerGroundedCollider.bounds.center, playerGroundedCollider.bounds.size, 0, Vector2.down * jump_inverted, 0.1f, pfLayerMask);
        if (rcHit)
        {
            
            Debug.Log(rcHit.transform.gameObject.name);
            pm = rcHit.collider.GetComponent<PlatformMover>();
            //transform.SetParent(null);
        }
        else
        {

            pm = null;

        }

        //*/
    }
}
