using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera self;
    public static Player player => Player.player;
    public static Camera camera;

    [Range(1, 50)]
    public float lazyness = 10;
    public float baseOffset = 2;
    private float offset = 4;
    
    public int cameraZoom = 2;
    public int cameraZoomSteps = 3;
    public int cameraZoomMax = 8;
    public int cameraZoomMin = 2;

    public float left, top, right, bottom;

    private void Awake()
    {
        self = this;
        camera = GetComponent<Camera>();
    }

    void Start()
    {
        Transform
            l = FindChildWithTag(GameObject.Find("Borders"), "LeftWall"),
            t = FindChildWithTag(GameObject.Find("Borders"), "TopWall"),
            r = FindChildWithTag(GameObject.Find("Borders"), "RightWall"),
            b = FindChildWithTag(GameObject.Find("Borders"), "BottomWall");

        left = l ? l.transform.position.x : 0;
        top = t ? t.transform.position.y : 0;
        right = r ? r.transform.position.x : 0;
        bottom = b ? b.transform.position.y : 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 dir = player.transform.position - transform.position;
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
        Debug.Log(cameraZoom);
        Debug.Log(camera.fieldOfView);
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
