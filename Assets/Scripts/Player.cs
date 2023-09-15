using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


[RequireComponent(typeof(Player_controll), typeof(PlayerPhysics))]
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

    public SpriteRenderer spriteRenderer;
    internal Collider coll;

    public int maxHealth = 100;
    public float health = 100;
    public static int jumps => PlayerPhysics.jumps;
    public static int deaths = 0;

    public GameObject statusPanelValue;
    public GameObject healthBar;

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
        Respawn();
        ResetStatistic();

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
            Die();
        }
        UpdateUI();
    }

    public void OnResume()
    {
        
    }

    public void OnPause()
    {
        
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
    }

    public void ResetCamera()
    {
        //playerCamera.transform.position = spawn.transform.position;
        playerCamera.Initialize();
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
