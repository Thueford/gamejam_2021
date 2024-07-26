using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChapterStarter : MonoBehaviour
{
    public int currentLevel = 0;
    private List<GameObject> levels = new List<GameObject>();
    public static Player player => Player.player;
    public static GameObject currStage;
    public static ChapterStarter chapterStarter;
    public static PlayerCamera playerCamera => PlayerCamera.self;

    private void Awake()
    {
        //TODO: This can get tricky 
        //if (!chapterStarter) chapterStarter = this;
        chapterStarter = this;

        foreach (Transform child in transform)
        {
            levels.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        currentLevel = PlayerPrefs.GetInt("CurrentLevel");

        if (currentLevel + 1 > levels.Count)
        {
            currentLevel = 0;
        }
    }

    void Start()
    {
        //GameObject level = levels[currentLevel];
        //currLevel = Instantiate(level);
        //currLevel.transform.position = Vector3.zero;

        //currStage.SetActive(true);
        //player.spawn = currStage.transform.Find("Spawn").gameObject.GetComponent<Door>();
        //player.physics.maxJumps = currStage.GetComponent<LevelController>().maxJumps;
        GameObject level = CreateNewLevel();

        Vector3 pos = player.spawn.transform.position;
        pos.z -= .1f;
        player.transform.position = pos;

        playerCamera.Initialize(level);
        pos = player.transform.position;
        pos.z = -10;
        playerCamera.transform.position = pos;

    }

    private GameObject CreateNewLevel()
    {
        currStage = Instantiate(levels[currentLevel]);
        currStage.SetActive(true);
        currStage.transform.position = Vector3.zero;

        player.spawn = currStage.transform.Find("Spawn").gameObject.GetComponent<Door>();
        player.physics.maxJumps = currStage.GetComponent<LevelController>().maxJumps;

        return currStage;
    }

    public static void ReloadStage()
    {
        Destroy(currStage);
        chapterStarter.CreateNewLevel();
    }

    private void FinishLevel()
    {
        Destroy(currStage);
    }

    private void FinishChapter()
    {
        ChangeScene.ChangeToScene("StartScene");
    }
    public void NextLevel()
    {
        Debug.LogError("Next Level");
        Debug.Log(currentLevel);
        if (currentLevel >= transform.childCount - 1)
        {
            // finished chapter
            FinishChapter();
        }

        //levels[currentLevel].SetActive(false);
        FinishLevel();
        currentLevel++;
        Debug.Log("You dead");

        GameObject level = CreateNewLevel();
        
        player.Respawn();
        player.ResetStatistic();
        //player.ResetCamera(level);

        // todo back to chapters and mark as solveds
    }
}
