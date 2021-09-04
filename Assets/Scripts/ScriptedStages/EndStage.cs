using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStage : ScriptedStage
{
    public GameObject alphons;
    public int numAlphons = 10;
    public float alphonsSpeed = 5, spawnDelay = 0.5f, maxDist = 2;
    private List<GameObject> alphonsos = new List<GameObject>();
    private bool moving = false;

    private void Start() => alphons.SetActive(false);
    public void ev_freeze() => KeyHandler.enableMovement = false;
    public void ev_end() => StartCoroutine(EndScene());

    private void Update()
    {
        if (!moving) return;

        foreach (GameObject a in alphonsos)
            a.transform.position += Vector3.right * alphonsSpeed * Time.deltaTime;

        if (Vector2.Distance(alphonsos[0].transform.position, PlayerMovement.self.transform.position) < maxDist)
        {
            foreach (GameObject a in alphonsos)
                a.GetComponent<Animator>().SetBool("isWalking", false);

            moving = false;
            StageManager.EndGame();
        }
    }

    IEnumerator EndScene()
    {
        for(int i = 0; i < numAlphons; i++)
        {
            if (i > 0) yield return new WaitForSeconds(spawnDelay + Random.value * 0.2f);
            else moving = true;
            GameObject a = Instantiate(alphons);
            a.SetActive(true);
            a.transform.position += Vector3.forward * i / 1e3f;
            a.GetComponent<Animator>().speed *= 1.5f + (Random.value - 0.5f) * 0.2f;
            alphonsos.Add(a);
        }
    }
}
