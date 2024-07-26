using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [Range(0, 100)]
    public float force;

    [Range(0, 100)]
    public float maxSpeed = 8;

    HashSet<Rigidbody2D> affected = new HashSet<Rigidbody2D>();

    private void FixedUpdate()
    {
        Vector2 dir = transform.rotation * Vector2.up;
        foreach (Rigidbody2D c in affected)
        {
            //if (!PlayerMovement.self.reachedTerminal(dir * force * Time.fixedDeltaTime))
            

            PlayerPhysics pph = c.gameObject.GetComponent<PlayerPhysics>();
            //Debug.Log(transform.rotation.eulerAngles.z - 1 * pph.jump_inverted < 0);
            if (!pph.interactSwitch || !((transform.rotation.eulerAngles.z - 1) * pph.jump_inverted < 0))
            {
                c.AddForce(dir * force);

                Vector3 max_velocity = Vector3.ClampMagnitude(c.velocity, maxSpeed);
                max_velocity.y = c.velocity.y;
                c.velocity = max_velocity;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player")) 
        { 
            affected.Add(c.GetComponent<Rigidbody2D>());
            PlayerPhysics pph = c.gameObject.GetComponent<PlayerPhysics>();

            //pph.inFan = true;
            if ((transform.rotation.eulerAngles.z - 1) * pph.jump_inverted < 0) pph.inFan = true;
            else pph.inFan = false;
            // just if fan is faced down

            SoundHandler.PlayClip("fanenter");
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            affected.Remove(c.GetComponent<Rigidbody2D>());
            PlayerPhysics pph = c.gameObject.GetComponent<PlayerPhysics>();
            pph.inFan = false;
            SoundHandler.PlayClip("fanexit");
        }
    }
}
