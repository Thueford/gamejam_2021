using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlAnzeige : MonoBehaviour
{
    public TMPro.TextMeshProUGUI tmpro;

    void Start() => SetText();
    void Update() => SetText();

    private void SetText()
    {
        if (StageManager.curStage) tmpro.text = "Level: " + (StageManager.curStage.index+1).ToString();
    }
}
