using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonControll : MonoBehaviour
{
    private bool buttonActive;
    private TMPro.TextMeshPro buttonText;
    private Animator animator;
    private PlayerInput playerInput;

    public static Player player => Player.player;

    //public abstract string helpText { get; set; };
    protected virtual string helpText
    {
        get { return "toggle"; }
    }


    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerInput = this.GetComponent<PlayerInput>();
        buttonText = this.GetComponentInChildren<TMPro.TextMeshPro>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonActive = true;

        // TODO: Concat string
        foreach(InputBinding binding in playerInput.actions.FindAction("Interact").bindings)
        {
            //Debug.Log(binding.path);
            buttonText.SetText("Press [" + binding.path + "] to " + helpText);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void Toggle()
    {
        
    }

    public void OnInteract()
    {
        if (buttonActive)
        {
            animator.SetBool("isDown", !animator.GetBool("isDown"));
            Toggle();
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            buttonText.gameObject.SetActive(true);
            buttonActive = true;
        }
    }
    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            buttonActive = false;
            buttonText.gameObject.SetActive(false);
        }
    }
}
