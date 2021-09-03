using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController self;
    public static int inverted = 1;

    public GameObject InvertGravityPanel;
    public bool triggerRespawn = false;
    public SpriteRenderer sr;

    void Awake() => self = this;

    // Update is called once per frame
    void Update()
    {
        if (KeyHandler.ReadRespawnButtonDown()) StageManager.RestartStage();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Goal"))
            StageManager.NextStage();
        else if (collider.CompareTag("Spikes"))
            Die();
    }

    public void Die()
    {
        StageManager.RestartStage();
    }

    public void Respawn(Vector3 position)
    {
        transform.position = position;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public static void InvertGravity()
    {
        if (inverted > 0) inverted = -1;
        else inverted = 1;
        self.sr.flipY = !self.sr.flipY;
    }
}
