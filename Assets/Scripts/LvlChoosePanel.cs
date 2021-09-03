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
            Image btn = Instantiate(btnPrefab, new Vector3(200 + (Screen (i%5),Screen.height - 250*(i/5),0), Quaternion.identity, panel.rectTransform);
            btn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = i.ToString();
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
