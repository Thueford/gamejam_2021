using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum Type { NONE };
    public static Player player => Player.player;

    public Type type;

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (!c.CompareTag("Player")) return;

        //SoundHandler.PlayClip("collect");

        player.physics.AddJump();
        player.UpdateUI();
        Destroy(gameObject);
    }
}
