using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageButtonHandler : MonoBehaviour
{
    public abstract void Toggle(bool status, Collider2D c);
}
