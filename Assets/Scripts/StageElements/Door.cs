using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool open = false;

    Collider2D coll;

    public static Player player => Player.player;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (open)
        {
            openDoor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openDoor()
    {
        GameObject op = transform.Find("DoorOpened").gameObject;
        GameObject cl = transform.Find("DoorClosed").gameObject;

        coll.enabled = true;
        op.SetActive(false);
        cl.SetActive(true);
    }

    public void closeDoor()
    {
        GameObject op = transform.Find("DoorOpened").gameObject;
        GameObject cl = transform.Find("DoorClosed").gameObject;

        coll.enabled = false;
        op.SetActive(true);
        cl.SetActive(false);
    }

    public void switchDoor()
    {
        if (open)
        {
            open = false;
            closeDoor();
        }
        else
        {
            open = true;
            openDoor();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && open)
        {
            player.OnGoal();
        }
    }
}
