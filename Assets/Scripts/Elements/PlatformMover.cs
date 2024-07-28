using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : BaseElement
{
    public static Player player => Player.player;
    public float speed = 1;
    public bool startOnContact = true, activated = true, baseActive = true;
    public Transform[] points;
    private Transform[] modifiedPoints;
    public float checkpointDistance;

    private int pnum = 0, pnext = 1, pdir = 1;
    private Rigidbody2D rb;
    private bool active = true;
    private int inverted;
    public SpriteRenderer sr;

    private Vector3 lastPosition = Vector3.zero;
    public Vector2 customVelocity = Vector2.zero;

    private void Awake()
    {
        init();
        baseActive = activated;
    }

    private void init()
    {
        if (points.Length == 0) Destroy(this);
        if (points.Length >= 1) transform.position = points[0].transform.position;
        rb = GetComponent<Rigidbody2D>();
        foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>())
        {
            if (s.gameObject != gameObject) sr = s;
        }
        Activate(activated);

        modifiedPoints = points;

        // Testing if teleporting bug is gone 
        //transform.position = points[0].transform.position;
    }

    public override void Reset()
    {
        base.Reset();

        init();
        initstart();
        Activate(baseActive);
        // TODO: Reset got some bugs
    }

    public void Activate(bool setActive)
    {
        active = (activated = setActive) && !startOnContact;
        if (sr)
        {
            if (activated)
                sr.color = Color.white;
            else
                sr.color = new Color(1, .7f, .7f, 1);
        }
    }

    private void Start()
    {
        initstart();
        player.elements.Add(GetComponent<PlatformMover>());
        lastPosition = transform.position;
    }

    private void initstart()
    {
        inverted = player.physics.jump_inverted;
        for (int i = 0; i < modifiedPoints.Length; i++)
        {
            LineRenderer lr = modifiedPoints[i].GetComponent<LineRenderer>();
            if (lr)
            {
                if (lr.GetPosition(0) != Vector3.zero)
                {
                    modifiedPoints[i] = Instantiate(modifiedPoints[i]);
                    lr = modifiedPoints[i].GetComponent<LineRenderer>();
                }

                lr.startColor = lr.endColor = new Color(0, 0, 0, 0.5f);
                lr.SetPosition(0, modifiedPoints[i].position);
                if (i == 0) lr.enabled = false;
                else lr.SetPosition(1, modifiedPoints[i - 1].position);
            }
        }
    }

    private void Update()
    {
        if (inverted != player.physics.jump_inverted)
        {
            transform.rotation *= Quaternion.Euler(Vector3.forward * 180);
            inverted = player.physics.jump_inverted;
        }
    }

    private void NextCheckpoint()
    {
        if (modifiedPoints.Length <= 1) return;


        pnum += pdir;
        if (pnum == 0 || pnum == modifiedPoints.Length - 1) pdir = -pdir;
        pnext = pnum + pdir;
    }

    void FixedUpdate()
    {
        // skip if no checkpoints
        if (!active || modifiedPoints.Length <= 1)
        {
            // otherwise platoform would have a custom velocity when deactivated
            customVelocity = Vector2.zero;
            return;
        }

        Vector2 dir = (modifiedPoints[pnext].position - modifiedPoints[pnum].position).normalized;
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);

        customVelocity = (transform.position - lastPosition) / Time.deltaTime;

        checkpointDistance = Vector2.Distance(rb.position, modifiedPoints[pnext].position);
        if (checkpointDistance < 2 * speed * Time.fixedDeltaTime)
            NextCheckpoint();

        lastPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (activated && !active && c.gameObject.CompareTag("Player")) active = true;


    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(transform, true);
            //player.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }//*/
}
