using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public float speed = 1;
    public bool startOnContact = true, activated = true;
    public Transform[] points;
    public float checkpointDistance;

    private int pnum = 0, pnext = 1, pdir = 1;
    private Rigidbody2D rb;
    private bool active = true;
    private int lastGravity;
    public SpriteRenderer sr;

    private void Awake()
    {
        if (points.Length == 0) Destroy(this);
        if (points.Length >= 1) transform.position = points[0].transform.position;
        rb = GetComponent<Rigidbody2D>();
        foreach(SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>())
        {
            if (s.gameObject != gameObject) sr = s;
        }
        Activate(activated);
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
        lastGravity = PlayerController.inverted;

        for (int i = 0; i < points.Length; i++)
        {
            LineRenderer lr = points[i].GetComponent<LineRenderer>();

            // copy checkpoint if already used. Why is drawing lines so hard...
            if (lr.GetPosition(0) != Vector3.zero)
            {
                points[i] = Instantiate(points[i]);
                lr = points[i].GetComponent<LineRenderer>();
            }

            lr.startColor = lr.endColor = new Color(0, 0, 0, 0.5f);
            lr.SetPosition(0, points[i].position);
            if (i == 0) lr.enabled = false;
            else lr.SetPosition(1, points[i - 1].position);
        }
    }

    private void Update()
    {
        if (lastGravity != PlayerController.inverted)
        {
            transform.rotation *= Quaternion.Euler(Vector3.forward * 180);
            lastGravity = PlayerController.inverted;
        }
    }

    private void NextCheckpoint()
    {
        if (points.Length <= 1) return;

        /*
        if (points.Length-1 == pnum+1) {
            pnum = pnext;
            pnext = ((pnum + 1) % points.Length) -1;
            Array.Reverse(points);
        } else {
            pnum = pnext;
            pnext = pnum + 1;
        } */


        pnum += pdir;
        if (pnum == 0 || pnum == points.Length - 1) pdir = -pdir;
        pnext = pnum + pdir;
    }

    void FixedUpdate()
    {
        // skip if no checkpoints
        if (!active || points.Length <= 1) return;

        Vector2 dir = (points[pnext].position - points[pnum].position).normalized;
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);

        checkpointDistance = Vector2.Distance(rb.position, points[pnext].position);
        if (checkpointDistance < 2 * speed * Time.fixedDeltaTime)
            NextCheckpoint();
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (activated && !active && c.gameObject.CompareTag("Player")) active = true;
    }
}
