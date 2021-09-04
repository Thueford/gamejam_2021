using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButtonHandler : MonoBehaviour
{
    public virtual void Toggle(bool status, Collider2D c) 
    {
        SoundHandler.PlayClip("button");
    }
    
}
