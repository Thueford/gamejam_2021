using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public float speed = 1;
    public bool startOnContact = true;
    public Transform[] points;

    private int pnum = 0, pnext = 1, pdir = 1;
    private Rigidbody2D rb;
    private bool active = true;

    private void Awake()
    {
        if (points.Length == 0) Destroy(this);
        if (points.Length == 1) transform.position = points[0].transform.position;

        rb = GetComponent<Rigidbody2D>();
        active = !startOnContact;
    }

    private void NextCheckpoint()
    {
        if (points.Length <= 1) return;
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

        if (Vector3.Distance(rb.position, points[pnext].position) < 2 * speed * Time.fixedDeltaTime)
            NextCheckpoint();
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (!active && c.gameObject.CompareTag("Player")) active = true;
    }
}
