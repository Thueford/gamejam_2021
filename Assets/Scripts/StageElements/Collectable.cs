using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum Type { NONE };

    public Type type;

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (!c.CompareTag("Player")) return;

        SoundHandler.PlayClip("collect");

        // Collectable Handling
        switch(type)
        {
            case Type.NONE: break;
        }

        c.gameObject.GetComponent<PlayerMovement>().AddJumps(1);
        Destroy(gameObject);
    }
}
