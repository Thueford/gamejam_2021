using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenLvlBtn : MonoBehaviour
{
    public TextMeshProUGUI tmpro;

    public void btnPressed_LoadLvl()
    {

        SoundHandler.PlayClick();

        int choosenStage = int.Parse(tmpro.text)-1;
        if (StageManager.isEnabled(choosenStage))
        {
            PlayerPrefs.SetInt("choosenlvl", choosenStage);
            SceneManager.LoadScene("main");
            // StageManager.LoadStage(int.Parse(tmpro.text));
            GameObject.Find("LvlChooser").GetComponent<LvlChoosePanel>().btnPressed_CloseChooseLvl();
        }
    }

}
