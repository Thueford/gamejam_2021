using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateLaser : MonoBehaviour
{
    public bool activated = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!activated)
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

    public void Activate(bool value)
    {
        GameObject blocker = transform.Find("Blocker").gameObject;
        blocker.SetActive(value);

        Animator laserAnimation = transform.Find("laser_animated").GetComponent<Animator>();
        laserAnimation.SetBool("isOff", !laserAnimation.GetBool("isOff"));

        activated = !activated;


        /*GameObject b = Lock.transform.Find("Blocker").gameObject;
        b.SetActive(!b.activeSelf);
        GameObject gc = Lock.transform.Find("laser_aus_complete_0").gameObject;
        bool aaa = Lock.transform.Find("laser_animated").GetComponent<Animator>().GetBool("isOff");
        gc.SetActive(!aaa);
        Lock.transform.Find("laser_animated").GetComponent<Animator>().SetBool("isOff", !aaa);*/
    }
}
