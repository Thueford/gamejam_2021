using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void btnPressed_Credits()
    {
        SceneManager.LoadScene("CreditScene");
    }

    public void btnPressed_Start()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void btnPressed_Close()
    {
        Debug.Log("Goodbye");
        Application.Quit();
    }
}
