using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterStarter : MonoBehaviour
{
    public int currentLevel = 0;
    private List<GameObject> levels = new List<GameObject>();
    public static Player player => Player.player;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            levels.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
    }

    void Start()
    {
        GameObject level = levels[currentLevel];
        level.SetActive(true);
        player.spawn = level.transform.Find("Spawn").gameObject.GetComponent<Door>();
        player.physics.maxJumps = level.GetComponent<LevelController>().maxJumps;
    }

    private void FinishLevel()
    {

    }

    private void FinishChapter()
    {
        ChangeScene.ChangeToScene("StartScene");
    }
    public void NextLevel()
    {
        if (currentLevel >= transform.childCount - 1)
        {
            // finished chapter
            FinishChapter();
        }

        levels[currentLevel].SetActive(false);
        FinishLevel();
        currentLevel++;
        GameObject level = levels[currentLevel];
        level.SetActive(level);
        GameObject spawn = level.transform.Find("Spawn").gameObject;
        player.spawn = spawn.GetComponent<Door>();
        player.physics.maxJumps = level.GetComponent<LevelController>().maxJumps;
        
        player.Respawn();
        player.ResetStatistic();
        player.ResetCamera();

        // todo back to chapters and mark as solveds
    }
}
