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
        {
            if (inverted < 0)
            {
                InvertGravity();
                GetComponent<Rigidbody2D>().gravityScale = GetComponent<Rigidbody2D>().gravityScale * -1;
            }
            StageManager.NextStage();
        }
        else if (collider.CompareTag("Spikes"))
            Die();
    }

    public void Die()
    {
        if(inverted < 0)
        {
            InvertGravity();
            GetComponent<Rigidbody2D>().gravityScale = GetComponent<Rigidbody2D>().gravityScale * -1;
        }
        StageManager.RestartStage();
        int dfj = StageManager.curStage.GetComponent<Stage>().DefaulJumps;
        GetComponent<PlayerMovement>().SetJumps(dfj);
        KeyHandler.enableMovement = false;
    }

    public void Respawn(Vector3 position)
    {
        transform.position = position;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        int dfj = StageManager.curStage.GetComponent<Stage>().DefaulJumps;
        GetComponent<PlayerMovement>().SetJumps(dfj);
        Invoke("ReactivateMovement", 1);
    }

    private void ReactivateMovement()
    {
        KeyHandler.enableMovement = true;
    }

    public static void InvertGravity()
    {
        if (inverted > 0) inverted = -1;
        else inverted = 1;
        self.sr.flipY = !self.sr.flipY;
    }
}
