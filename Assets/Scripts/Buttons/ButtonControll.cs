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
        buttonText = GetComponentInChildren<TMPro.TextMeshPro>();
    }

    // Start is called before the first frame update
    void Start()
    {
        doInit();
    }

    public void doInit()
    {
        buttonActive = true;
        playerInput = Player.player.GetComponent<PlayerInput>();

        buttonText.SetText("Press [" + playerInput.actions.FindAction("Interact").GetBindingDisplayString(0) + "] to " + helpText);

        /*
        foreach (InputBinding binding in playerInput.actions.FindAction("Interact").bindings)
        {
            Debug.Log(playerInput.actions.FindAction("Interact").GetBindingDisplayString(0));
            buttonText.SetText("Press [" + binding.path + "] to " + helpText);
            break;
        }*/


        buttonText.gameObject.SetActive(false);
    }

    public virtual void Toggle()
    {
        if (buttonActive)
        {
            animator.SetBool("isDown", !animator.GetBool("isDown"));
        }
    }
    public void OnEnter()
    {
        buttonText.gameObject.SetActive(true);
        buttonActive = true;
    }

    public void OnExit()
    {
        buttonActive = false;
        buttonText.gameObject.SetActive(false);
    }
}
