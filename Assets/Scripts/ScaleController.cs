using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleController : MonoBehaviour
{

    Collider2D coll;
    SpriteRenderer sp;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        sp = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        // 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
