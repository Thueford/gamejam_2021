using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            int a = PlayerPrefs.GetInt("choosenlvl");
            Debug.Log("lvl:" + a);
            StageManager.LoadStage(a);
        } catch
        {
            StageManager.LoadStage(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
