using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public static CamController self;
    public static bool allowMoving = true;

    [Range(1,50)]
    public float lazyness = 10;
    public GameObject target;
    public float offset = 7;

    private float left, top, right, bottom;

    private void Awake() => self = this;
    private void Start() => left = top = right = bottom = 0;

    private static Transform FindChildWithTag(GameObject t, string tag, bool recursive = false)
    {
        foreach (Transform child in t.transform)
        {
            if (child.CompareTag(tag)) return child;
            else FindChildWithTag(child.gameObject, tag, recursive);
        }
        return null;
    }

    public void ResetBorders()
    {
        Transform
            l = FindChildWithTag(StageManager.curStage.walls, "LeftWall"),
            t = FindChildWithTag(StageManager.curStage.walls, "TopWall"),
            r = FindChildWithTag(StageManager.curStage.walls, "RightWall"),
            b = FindChildWithTag(StageManager.curStage.walls, "BottomWall");

        left = l ? l.transform.position.x : 0;
        top = t ? t.transform.position.y : 0;
        right = r ? r.transform.position.x : 0;
        bottom = b ? b.transform.position.y : 0;

        if (left + top + right + bottom == 0)
            Debug.LogWarning("Possibly no stage borders set (Set with [Left,...]Wall tags)");
        else
            StartCoroutine(LevelPreview());
    }

    IEnumerator LevelPreview()
    {
        lazyness *= 3;
        allowMoving = false;
        yield return new WaitForSeconds(0.7f);
        target = StageManager.curStage.goal;
        yield return new WaitForSeconds(2);
        target = PlayerMovement.self.gameObject;
        yield return new WaitForSeconds(1);
        KeyHandler.enableMovement = allowMoving = true;
        yield return new WaitForSeconds(1);
        lazyness /= 3;
    }

    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 dir = target.transform.position - transform.position;
        Vector3 pos = transform.position += (Vector3)dir / lazyness;

        if (top != 0 && pos.y + offset > top) pos.y = top - offset;
        if (bottom != 0 && pos.y - offset < bottom) pos.y = bottom + offset;
        if (left != 0 && pos.x - offset < left) pos.x = left + offset;
        if (right != 0 && pos.x + offset > right) pos.x = right - offset;

        transform.position = pos;
    }
}