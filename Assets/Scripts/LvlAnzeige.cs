using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlAnzeige : MonoBehaviour
{
    public TMPro.TextMeshProUGUI tmpro;
    // Start is called before the first frame update
    void Start()
    {
        tmpro.text = StageManager.curStage.index.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        tmpro.text = StageManager.curStage.index.ToString();
    }
}
