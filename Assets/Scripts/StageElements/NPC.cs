using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : StageButtonHandler
{
    public TMPro.TextMeshPro text;
    private string[] s;
    private int iText, iChar;

    private void Start()
    {
        if (!text)
        {
            Destroy(this);
            return;
        }

        text.gameObject.SetActive(false);
        s = text.text.Trim().Split('\n');
    }

    public override void Toggle(bool status, Collider2D c)
    {
        // start talking
        if (!text.IsActive())
        {
            text.gameObject.SetActive(true);
            iText = iChar = 0;
            InvokeRepeating(nameof(DisplayText), 0, 0.03f);
        }
        // skip talking
        else if (iChar < s[iText].Length)
        {
            iChar = s[iText].Length;
            text.text = s[iText];
            CancelInvoke();
        }
        // continue talking
        else if (++iText == s.Length)
        {
            text.text = "";
            text.gameObject.SetActive(false);
        }
        else
        {
            iChar = 0;
            InvokeRepeating(nameof(DisplayText), 0, 0.03f);
        }
    }

    private void DisplayText()
    {
        text.text = s[iText].Substring(0, ++iChar);
        if (iChar == s[iText].Length) CancelInvoke();
    }
}
