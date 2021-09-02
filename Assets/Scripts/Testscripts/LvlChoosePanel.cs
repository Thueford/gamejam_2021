using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlChoosePanel : MonoBehaviour
{
    public Image panel;

    public GameObject btnPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GameObject stageContainer = StageManager.GetStageContainer();
        
        for(int i = 0; i < stageContainer.transform.childCount; i++)
        {
            GameObject btn = Instantiate(btnPrefab, panel.transform);
            btn.GetComponent<TMPro.TextMeshPro>().text = i.ToString();
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
}
