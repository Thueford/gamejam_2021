using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    public float force;
    HashSet<Rigidbody2D> affected = new HashSet<Rigidbody2D>();

    private void FixedUpdate()
    {
        Vector2 dir = transform.rotation * Vector2.up;
        foreach (Rigidbody2D c in affected)
            c.AddForce(dir * force);
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player")) affected.Add(c.GetComponent<Rigidbody2D>());
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.CompareTag("Player")) affected.Remove(c.GetComponent<Rigidbody2D>());
    }
}
