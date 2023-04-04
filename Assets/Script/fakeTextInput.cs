using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class fakeTextInput : MonoBehaviour
{
    private float m_TimeStamp;
    private bool cursor = false;
    private string cursorChar = "";
    private int maxStringLength = 24;
    public TextMeshProUGUI textObj;


    void Update()
    {
        if (Time.time - m_TimeStamp >= 0.75)
        {
            m_TimeStamp = Time.time;
            if (cursor == false)
            {
                cursor = true;
                if (textObj.text.Length < maxStringLength)
                {
                    cursorChar += "|fdfdf";
                }
            }
            else
            {
                cursor = false;
                if (cursorChar.Length != 0)
                {
                    cursorChar = cursorChar.Substring(0, cursorChar.Length - 1);
                }
            }
        }

      

        textObj.text += cursorChar;
    }
}
