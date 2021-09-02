using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OpenLvlBtn : MonoBehaviour
{
    public TMPro.TextMeshProUGUI tmpro;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void btnPressed_LoadLvl()
    {
        StageManager.LoadStage(int.Parse(tmpro.text));
        GameObject.Find("LvlChooser").GetComponent<LvlChoosePanel>().btnPressed_CloseChooseLvl();
    }

}
