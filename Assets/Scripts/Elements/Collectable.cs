using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : BaseElement
{
    public enum Type { NONE };
    public Type type;

    public static Player player => Player.player;

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (!c.CompareTag("Player")) return;

        //SoundHandler.PlayClip("collect");

        player.physics.AddJump();
        player.UpdateUI();

        player.elements.Add(gameObject.GetComponent<Collectable>());
        gameObject.SetActive(false);

        SoundHandler.PlayClip("collect");
    }
}
