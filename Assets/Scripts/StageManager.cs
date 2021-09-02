using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager self;

    public static Stage curStage;
    private static int curStageNo = -1;

    public static int maxStage = 0;
    public bool triggerNextStage = false;
    public GameObject stageContainer;

    public static Transform GetStageContainer() => self.stageContainer.transform;
    public static bool isEnabled(int index) => index <= maxStage;


    private void Awake()
    {
        self = this;
        maxStage = PlayerPrefs.GetInt("maxStage", 0);
    }

    private void Start() => NextStage();
    private void Update()
    {
        if (triggerNextStage)
        {
            triggerNextStage = false;
            NextStage();
        }
    }


    public static void NextStage()
    {
        if (curStage) Destroy(curStage.gameObject);
        curStageNo++;


        if (curStageNo == GetStageContainer().childCount)
        {
            SceneManager.LoadScene("StartScene");
            return;
        }
        PlayerPrefs.SetInt("maxStage", ++maxStage);
        LoadStage(curStageNo);
    }

    public static void LoadStage(int index)
    {
        Debug.Log("Load Stage " + index);
        curStageNo = index;
        curStage = GetStageContainer().GetChild(curStageNo).GetComponent<Stage>();

        curStage = Instantiate(curStage);
        curStage.transform.position = Vector3.zero;
        curStage.gameObject.SetActive(true);
    }
}
