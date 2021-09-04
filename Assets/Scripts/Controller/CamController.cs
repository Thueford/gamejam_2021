using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [Range(1,50)]
    public float lazyness = 10;
    public GameObject target;
    public float offset = 7;

    private static float left, top, right, bottom;

    public static void Reset()
    {
        GameObject[]
            l = GameObject.FindGameObjectsWithTag("LeftWall"),
            t = GameObject.FindGameObjectsWithTag("TopWall"),
            r = GameObject.FindGameObjectsWithTag("RightWall"),
            b = GameObject.FindGameObjectsWithTag("BottomWall");

        left = l.Length > 0 ? l[0].transform.position.x : 0;
        top = l.Length > 0 ? t[0].transform.position.y : 0;
        right = l.Length > 0 ? r[0].transform.position.x : 0;
        bottom = l.Length > 0 ? b[0].transform.position.y : 0;

        if (left + top + right + bottom == 0)
            Debug.LogWarning("Possibly no stage borders set (Set with [Left,...]Wall tags)");
    }

    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 dir = target.transform.position - transform.position;
        Vector3 pos = transform.position += (Vector3)dir / lazyness;

        if (top != 0 && pos.y + offset > top) pos.y = top - offset;
        if (bottom != 0 && pos.y - offset < bottom) pos.y = bottom + offset;

        if (left != 0 && pos.x + offset > left) pos.x = left + offset;
        if (right != 0 && pos.x + offset > right) pos.x = right - offset;
        transform.position = pos;
    }
}