using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class StageManager : MonoBehaviour
{
    public static StageManager self;

    public static Stage curStage;
    private static int curStageNo = -1, lastStageNo = -1;

    public static int maxStage = 0;
    public bool autoStart = false, triggerNextStage = false;
    public GameObject stageContainer_story;
    public GameObject stageContainer_custom;

    public enum LevelContainerType
    {
        STORY,
        CUSTOM
    };

    private LevelContainerType curLevel = LevelContainerType.STORY;

    public Dictionary<LevelContainerType, GameObject> LevelContainer = new Dictionary<LevelContainerType, GameObject>();

    public static Transform GetStageContainer() => self.LevelContainer[self.curLevel].transform; // self.stageContainer_story.transform;/

    public static bool isEnabled(int index) => index <= maxStage;


    private void Awake()
    {
        self = this;
        this.init();
    }

    private void Start()
    {
        if (autoStart) NextStage();
    }

    private void Update()
    {
        if (triggerNextStage)
        {
            triggerNextStage = false;
            NextStage();
        }

        Debug.Log(self.curLevel);
    }

    public void init()
    {
        maxStage = PlayerPrefs.GetInt("maxStage", 0);

        self.LevelContainer.Add(LevelContainerType.STORY, this.stageContainer_story);
        self.LevelContainer.Add(LevelContainerType.CUSTOM, this.stageContainer_custom);
    }

    public static void NextStage()
    {
        if (++curStageNo == GetStageContainer().childCount)
        {
            SceneManager.LoadScene("StartScene");
            return;
        }

        PlayerPrefs.SetInt("maxStage", ++maxStage);
        LoadStage(curStageNo);
    }

    public static void EndGame()
    {
        SceneManager.LoadScene("CreditScene");
    }

    public static void LoadStage(int index)
    {
        if (curStage) Destroy(curStage.gameObject);

        Debug.Log("Load Stage " + index);
        curStageNo = index;
        curStage = GetStageContainer().GetChild(curStageNo).GetComponent<Stage>();

        curStage = Instantiate(curStage);
        curStage.index = index;
        curStage.restarted = index == lastStageNo;
        lastStageNo = index;
        curStage.transform.position = Vector3.zero;
        PlayerController.self.Respawn(curStage.spawn.transform.position);
    }

    public static void RestartStage()
    {
        LoadStage(curStageNo);
    }

    public static void set_story() => self.curLevel = LevelContainerType.STORY;
    public static void set_custom() => self.curLevel = LevelContainerType.CUSTOM;
}
