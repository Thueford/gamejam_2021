using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [Range(1,50)]
    public float lazyness = 10;
    public GameObject target;

    private void LateUpdate()
    {
        if (!target) return;
        Vector2 dir = target.transform.position - transform.position;
        transform.position += (Vector3)dir / lazyness;
    }
}