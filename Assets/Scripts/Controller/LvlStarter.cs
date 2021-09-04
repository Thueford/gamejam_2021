using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlStarter : MonoBehaviour
{
    void Start()
    {
        try { StageManager.LoadStage(PlayerPrefs.GetInt("choosenlvl")); }
        catch { StageManager.LoadStage(0); }
    }
}
