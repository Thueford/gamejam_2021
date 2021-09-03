using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        PlayerPrefs.SetInt("choosenlvl", int.Parse(tmpro.text));
        SceneManager.LoadScene("main");
        //StageManager.LoadStage(int.Parse(tmpro.text));
        GameObject.Find("LvlChooser").GetComponent<LvlChoosePanel>().btnPressed_CloseChooseLvl();
    }

}
