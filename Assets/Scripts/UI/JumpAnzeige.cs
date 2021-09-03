using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAnzeige : MonoBehaviour
{
    public TMPro.TextMeshProUGUI tmpro;
    // Start is called before the first frame update
    void Start()
    {
        tmpro.text = GameObject.Find("Player").GetComponent<PlayerMovement>().jumps.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        tmpro.text = GameObject.Find("Player").GetComponent<PlayerMovement>().jumps.ToString();
    }
}
