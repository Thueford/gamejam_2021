using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


[RequireComponent(typeof(PlayerPhysics))]
public class Player : MonoBehaviour
{
    public static Player player { get; private set; }
    public static PlayerCamera playerCamera
    {
        get { return PlayerCamera.self; }
    }
    public Door spawn;
    public GameObject LevelContainer;

    public PlayerPhysics physics;
    /*public static PlayerPhysics physics
    {
        get { return PlayerPhysics.self; }
    }*/

    public bool paused = false;

    public SpriteRenderer spriteRenderer;
    internal Collider coll;

    public int maxHealth = 100;
    public float health = 100;
    public static int jumps => PlayerPhysics.jumps;
    public static int deaths = 0;

    public GameObject statusPanelValue;
    public GameObject healthBar;
    public GameObject PauseButton;
    public GameObject ResumeButton;

    public List<BaseElement> elements = new List<BaseElement>();

    private void Awake()
    {
        physics = GetComponent<PlayerPhysics>();
        coll = GetComponentInChildren<Collider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = spawn.transform.position;
        pos.z -= .1f;
        transform.position = pos;

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Goal()
    {
        if (LevelContainer)
        {
            LevelContainer.GetComponent<ChapterStarter>().NextLevel();
        }
    }

    public void ResetStatistic()
    {
        deaths = 0;
    }

    public void Die()
    {
        SoundHandler.PlayClip("death");
        Respawn();
    }

    public void UpdateUI ()
    {
        healthBar.GetComponent<Slider>().value = health / maxHealth;
        statusPanelValue.GetComponent<TMPro.TextMeshProUGUI>().SetText(jumps + "\n" + deaths);
    }

    public void TakeHit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            //maybe special sound
            Die();
        }
        UpdateUI();
    }

    public void OnResume()
    {
        ResumeButton.GetComponent<Button>().onClick.Invoke();
        //Resume();
    }

    public void Resume()
    {
        PlayerPhysics.self.EnableInput();

        paused = false;
    }

    public void OnPause()
    {
        if (paused)
        {
            OnResume();
            return;
        }

        PauseButton.GetComponent<Button>().onClick.Invoke();
        //Pause();
    }

    public void Pause()
    {
        PlayerPhysics.self.DisableInput();

        paused = true;
    }

    public void OnRespawn()
    {
        Die();
    }

    public void Respawn()
    {
        if (physics.jump_inverted < 0)
        {
            physics.InvertGravity();
        }
        physics.jump_inverted = 1;

        Vector3 pos = spawn.transform.position;
        pos.z -= .1f;
        transform.position = pos;

        health = maxHealth;
        deaths++;
        physics.ResetPhysics();
        UpdateUI();

        Debug.Log(elements.Count);

        foreach(BaseElement element in elements)
        {
            element.Reset();
            element.gameObject.SetActive(true);
        }

        // reset platforms

        ChapterStarter.ReloadStage();   

        player.ResetCamera();
    }

    public void ResetCamera()
    {
        
        playerCamera.Initialize();
        Vector3 pos = player.transform.position;
        pos.z = -10;
        playerCamera.transform.position = pos;
    }

    public static void GlobalRespawn()
    {
        player.Respawn();
    }

    public void OnCameraZoom()
    {
        playerCamera.OnCameraZoom();
    }


}
