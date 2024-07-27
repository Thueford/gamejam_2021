using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera self;
    public static Player player => Player.player;
    public Camera camera;

    [Range(1, 50)]
    public float lazyness = 10;
    public float baseOffset = 2;
    private float offset = 4;
    
    public int cameraZoom = 3;
    public int cameraZoomSteps = 3;
    public int cameraZoomMax = 8;
    public int cameraZoomMin = 3;

    public static GameObject currStage => ChapterStarter.currStage;

    public float left, top, right, bottom;
    public GameObject target;

    private void Awake()
    {
        self = this;
        camera = GetComponent<Camera>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        GameObject border = currStage.transform.Find("Borders").gameObject;
        Transform
            l = FindChildWithTag(border, "LeftWall"),
            t = FindChildWithTag(border, "TopWall"),
            r = FindChildWithTag(border, "RightWall"),
            b = FindChildWithTag(border, "BottomWall");


        left = l ? l.transform.position.x : 0;
        top = t ? t.transform.position.y : 0;
        right = r ? r.transform.position.x : 0;
        bottom = b ? b.transform.position.y : 0;
    }

    public void Initialize(GameObject stage)
    {
        GameObject border = stage.transform.Find("Borders").gameObject;
        Transform
            l = FindChildWithTag(border, "LeftWall"),
            t = FindChildWithTag(border, "TopWall"),
            r = FindChildWithTag(border, "RightWall"),
            b = FindChildWithTag(border, "BottomWall");


        left = l ? l.transform.position.x : 0;
        top = t ? t.transform.position.y : 0;
        right = r ? r.transform.position.x : 0;
        bottom = b ? b.transform.position.y : 0;
    }

    public void StartLevelPreview()
    {
        StartCoroutine(LevelPreview());
    }

    IEnumerator LevelPreview()
    {
        lazyness *= 8;
        //allowMoving = false;
        player.Pause();
        yield return new WaitForSeconds(1f);
        target = FindChildWithTag(currStage, "Goal").gameObject;
        yield return new WaitForSeconds(4.5f);
        target = player.gameObject;
        yield return new WaitForSeconds(1);
        player.Resume();
        lazyness /= 8;
    }


    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 dir = target.transform.position - transform.position;
        Vector3 pos = transform.position += (Vector3)dir / lazyness;

        if (top != 0 && pos.y + offset/2 > top) pos.y = top + offset/2;
        if (bottom != 0 && pos.y - offset/2 < bottom) pos.y = bottom + offset/2;
        if (left != 0 && pos.x - offset < left) pos.x = left + offset;
        if (right != 0 && pos.x + offset > right) pos.x = right - offset;

        transform.position = pos;
    }

    public void OnCameraZoom()
    {
        cameraZoom += cameraZoomSteps;
        if (cameraZoom > cameraZoomMax) cameraZoom = cameraZoomMin;
        camera.orthographicSize = cameraZoom;
        offset = baseOffset + cameraZoom;
    }

    private static Transform FindChildWithTag(GameObject t, string tag, bool recursive = false)
    {
        foreach (Transform child in t.transform)
        {
            if (child.CompareTag(tag)) return child;
            else FindChildWithTag(child.gameObject, tag, recursive);
        }
        return null;
    }

}
