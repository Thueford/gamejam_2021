using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScript : MonoBehaviour
{
    public Image panel;
    // Start is called before the first frame update
    void Start()
    {
        panel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //restart from the last Checkpoint
    public void btnPressed_restart()
    {

    }

    //go back to the start scene
    public void btnPressed_BackToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
