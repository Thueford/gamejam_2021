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
            StageManager.LoadStage(PlayerPrefs.GetInt("choosenlvl"));
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
