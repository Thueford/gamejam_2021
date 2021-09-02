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
        Transform stageContainer = StageManager.GetStageContainer();

        for(int i = 0; i < stageContainer.childCount; i++)
        {
            Image btn = Instantiate(btnPrefab, new Vector3(150 + 150*(i%5),320 - 100*(i/5),0), Quaternion.identity, panel.transform);
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
        panel.gameObject.SetActive(false);
    }
}
