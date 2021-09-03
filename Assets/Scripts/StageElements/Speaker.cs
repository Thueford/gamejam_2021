using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : StageButtonHandler
{
    public TMPro.TextMeshPro text;
    private string s;
    private int displayed;

    private void Start()
    {
        if (!text)
        {
            Destroy(this);
            return;
        }

        text.gameObject.SetActive(false);
        s = text.text;
    }

    public override void Toggle(bool status, Collider2D c)
    {
        text.gameObject.SetActive(status);
        displayed = 0;
        if (status) InvokeRepeating(nameof(DisplayText), 0, 0.1f);
    }

    private void DisplayText()
    {
        text.text = s.Substring(0, ++displayed);
        if (displayed == s.Length) CancelInvoke();
    }
}
