using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlChoosePanel : MonoBehaviour
{
    public Image panel, btnPrefab;

    // verschiedene buttons müssen hier gespeichert werden um gelöscht zu werden oder geprüft ob die noch da sind

    // Start is called before the first frame update
    void Start()
    {
        generate_panel();
        panel.gameObject.SetActive(false);
    }

    public void generate_panel()
    {
        panel.rectTransform.sizeDelta = new Vector2(Screen.width - 150, Screen.height - 150);
        Transform stageContainer = StageManager.GetStageContainer();
        int levelNumber = stageContainer.childCount;

        for (int i = 0; i < levelNumber; i++)
        {
            Image btn = Instantiate(btnPrefab, panel.rectTransform);

            float scale = panel.rectTransform.sizeDelta.x / btn.rectTransform.sizeDelta.x * 0.165f;
            float multi = 1.13f; // for scalling
            float offsetX = (btn.rectTransform.rect.width / 2 * 0.85f) * (scale + 1);
            float offsetY = btn.rectTransform.rect.height / 2 * 1.4f * (scale + 1);

            // Debug.Log(new Vector2(offsetX - (panel.rectTransform.sizeDelta.x / 2) + (i % Mathf.Round(levelNumber) / 2) * multi, i * multi));


            btn.rectTransform.localScale = new Vector3(btn.rectTransform.localScale.x * scale, btn.rectTransform.localScale.y * scale, btn.rectTransform.localScale.z);

            float row = Mathf.Floor(i / ((levelNumber + 1) / 2)) + 1;
            btn.transform.localPosition = new Vector2(offsetX - (panel.rectTransform.sizeDelta.x / 2) + ((i % (Mathf.Floor((levelNumber + 1) / 2))) * btn.rectTransform.rect.width) * multi * btn.rectTransform.localScale.x,
                                                      offsetY - row * btn.rectTransform.rect.height / 2 * 2f * scale * multi);


            // btn.rectTransform.position = new Vector2(/*Screen.width / 6 + */2.4f * (i%5)-4.8f * multi, /*4*Screen.height/6 + */-2*(i/5)+2) * multi;
            if (StageManager.isEnabled(i))
            {
                btn.color = Color.green;
            }
            else
            {
                btn.color = Color.red;
            }
            btn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = (i + 1).ToString();
        }
        panel.gameObject.SetActive(false);
    }

    public void btnPressed_chooseLvl() {

        generate_panel();
        panel.gameObject.SetActive(true);
        
    }

    public void btnPressed_CloseChooseLvl() => panel.gameObject.SetActive(false);
}
