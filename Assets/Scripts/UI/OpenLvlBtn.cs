using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenLvlBtn : MonoBehaviour
{

    public int LevelToOpen;
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
        SoundHandler.PlayClick();
        /*if (StageManager.isEnabled(choosenStage))
        {*/
            PlayerPrefs.SetInt("choosenlvl", LevelToOpen);
        Debug.Log(LevelToOpen);
        Debug.Log("PlayerPref: " + PlayerPrefs.GetInt("choosenlvl"));
            SceneManager.LoadScene("main");
            //StageManager.LoadStage(int.Parse(tmpro.text));
            GameObject.Find("LvlChooser").GetComponent<LvlChoosePanel>().btnPressed_CloseChooseLvl();
        /*}*/
    }

    public void SetLvlToOpen(int i)
    {
        this.LevelToOpen = i;
    }

}
