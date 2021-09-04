using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlChoosePanel : MonoBehaviour
{
    public Image panel;

    public Image btnPrefab;
    // Start is called before the first frame update
    void Start()
    {


        panel.rectTransform.sizeDelta = new Vector2(Screen.width-150, Screen.height-150);
        Transform stageContainer = StageManager.GetStageContainer();

        for(int i = 0; i < stageContainer.childCount; i++)
        {
            Image btn = Instantiate(btnPrefab, panel.rectTransform);
            btn.rectTransform.position = new Vector2(/*Screen.width / 6 + */2.4f * (i%5)-4.8f, /*4*Screen.height/6 + */-2*(i/5)+2);
            if (StageManager.isEnabled(i))
            {
                btn.color = Color.green;
            } else
            {
                btn.color = Color.red;
            }
            btn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = (i+1).ToString();
        }
        panel.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void btnPressed_chooseLvl()
    {
        panel.gameObject.SetActive(true);
    }

    public void btnPressed_CloseChooseLvl()
    {
        Debug.Log("aaaaaaaaaaaaaaaaaaaa");
        panel.gameObject.SetActive(false);
    }
}
