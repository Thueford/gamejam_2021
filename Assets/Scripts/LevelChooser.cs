using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelChooser : MonoBehaviour
{
    public GameObject chapterPrefab;
    public GameObject ButtonPrefab;

    public int itemsARow = 5;

    public int buttonDistanceX = 200;
    public int buttonDistanceY = 100;

    public string TargetScene;

    public List<GameObject> Buttons = new List<GameObject>();

    void Start()
    {
        ShowOverlay(TargetScene);
    }

    public void ShowOverlay(string chapterName)
    {
        Debug.Log(transform.name);
        if (chapterPrefab)
        {
            int counter = 0;
            
            GameObject container = transform.GetChild(2).GetChild(0).gameObject;
            RectTransform panelRect = container.GetComponent<RectTransform>();

            foreach (Transform child in chapterPrefab.transform)
            {
                //Debug.Log("Name Of Child " + child.transform.name);
                GameObject button = Instantiate(ButtonPrefab, transform.position, Quaternion.identity, container.transform) as GameObject;
                button.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(child.GetComponent<LevelController>().name); ;
                RectTransform buttonRect = button.GetComponent<RectTransform>();
                buttonRect.anchorMin = new Vector2(0, 1);
                buttonRect.anchorMax = new Vector2(0, 1);
                buttonRect.pivot = new Vector2(0, 1);
                button.layer = 5;
                button.name = counter.ToString();

                // todo: create position dynamically
                Vector3 pos = new Vector3();
                pos.x = buttonDistanceX * (counter % itemsARow);
                pos.y = Mathf.Floor(counter / itemsARow) * buttonDistanceY * -1;
                button.transform.localPosition = pos;

                Button btn = button.GetComponent<Button>();
                btn.onClick.AddListener(() => {
                    // todo counter is always max
                    PlayerPrefs.SetInt("CurrentLevel", int.Parse(button.name));
                    PlayerPrefs.SetInt("CurrentChapter", 1); // todo: fix dynamically
                    ChangeScene.ChangeToScene(chapterName);
                });

                Buttons.Add(button);

                counter++;
            }

            Buttons[0].GetComponent<Button>().Select();
        }
    }
}
