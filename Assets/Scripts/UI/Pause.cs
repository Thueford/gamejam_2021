using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public Image panel;
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    // Start is called before the first frame update
    void Start()
    {
        //panel = GetComponent<Image>();
        panel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void enterPause()
    {
        panel.gameObject.SetActive(true);
        playClickSound();
        Time.timeScale = 0;
    }

    public void endPause()
    {
        playClickSound();
        panel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void Exit()
    {
        playClickSound();
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }

    private void playClickSound()
    {
        
    }

    public void restart()
    {
        StageManager.RestartStage();
        panel.gameObject.SetActive(false);
    }
}
