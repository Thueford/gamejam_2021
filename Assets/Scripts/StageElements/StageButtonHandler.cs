using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButtonHandler : MonoBehaviour
{
    public virtual void Toggle(bool status, Collider2D c) 
    {
        SoundHandler.PlayClip("button");
        Animator anim = transform.Find("Button").GetComponent<Animator>();
        bool b = anim.GetBool("isDown");
        Debug.Log("btn pressed: " + b);
        anim.SetBool("isDown", !b);
    }
    
}
