
///////////////////////
//Do not touch this!!//
///////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public static Player player => Player.player;

    public static void ChangeToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public static void Exit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        player.Pause();
    }

    public void Resume()
    {
        player.Resume();
    }
}
