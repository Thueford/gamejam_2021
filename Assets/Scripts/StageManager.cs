using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static Stage curStage;
    public static StageManager self;

    public GameObject stageContainer;

    public static GameObject GetStageContainer() => self.stageContainer;

    public static void NextStage()
    {
        Stage next = Instantiate(curStage.nextStage);
        Destroy(curStage);
        next.gameObject.SetActive(true);
        next.Load();
    }

    private void Awake() => self = this;
}
