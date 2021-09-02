using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController self;

    public GameObject InvertGravityPanel;
    public bool triggerRespawn = false;

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
    }

    public void Respawn(Vector3 position)
    {
        transform.position = position;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
