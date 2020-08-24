using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    public static string textDisplay = "";
    private Text textObj;

    void Start()
    {
        textObj = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        textObj.text = textDisplay;
    }
}
