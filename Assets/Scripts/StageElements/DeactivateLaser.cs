using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateLaser : MonoBehaviour
{
    public bool aus;
    // Start is called before the first frame update
    void Start()
    {
        if (aus)
        {
            GameObject b = transform.Find("Blocker").gameObject;
            b.SetActive(false);
            GameObject gc = transform.Find("laser_aus_complete_0").gameObject;

            gc.SetActive(true);
            transform.Find("laser_animated").GetComponent<Animator>().SetBool("isOff", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
