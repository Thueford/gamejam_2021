
///////////////////////
//Do not touch this!!//
///////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public static void ChangeToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public static void Exit()
    {
        Application.Quit();
    }
}
