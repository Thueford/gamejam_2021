using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [Range(0, 20)]
    public float force;

    [Range(0, 20)]
    public float maxSpeed = 8;

    HashSet<Rigidbody2D> affected = new HashSet<Rigidbody2D>();

    private void FixedUpdate()
    {
        Vector2 dir = transform.rotation * Vector2.up;
        foreach (Rigidbody2D c in affected)
        {
            //if (!PlayerMovement.self.reachedTerminal(dir * force * Time.fixedDeltaTime))
            c.AddForce(dir * force);

            Vector3 max_velocity = Vector3.ClampMagnitude(c.velocity, maxSpeed);
            max_velocity.y = c.velocity.y;
            c.velocity = max_velocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player")) 
        { 
            affected.Add(c.GetComponent<Rigidbody2D>());
            //SoundHandler.PlayClip("fanenter");
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            affected.Remove(c.GetComponent<Rigidbody2D>());
            //SoundHandler.PlayClip("fanexit");
        }
    }
}
