using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    public TMPro.TextMeshPro ButtonText;
    public bool isToggle = false, status = false;
    public KeyCode key = KeyCode.E;

    private StageButtonHandler handler;
    private Collider2D coll;
    private bool buttonActive = false, initStatus;

    private void Awake() => handler = transform.parent.GetComponent<StageButtonHandler>();

    private void Start()
    {
        ButtonText.gameObject.SetActive(false);
        initStatus = status;
    }

    // Update is called once per frame
    public void Update()
    {
        if (buttonActive && (isToggle || status == initStatus))
            if (Input.GetKeyDown(key) || AndroidController.E)
            {
                Debug.Log("Toggle");
                handler.Toggle(status = !status, coll);
            }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            coll = c;
            buttonActive = true;
            if (ButtonText) ButtonText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            coll = null;
            buttonActive = false;
            if (ButtonText) ButtonText.gameObject.SetActive(false);
        }
    }
}