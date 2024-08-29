using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenSizeDebug : MonoBehaviour
{
    public TextMeshProUGUI txtMesh;

    // Update is called once per frame
    void Update()
    {
        txtMesh.text = Screen.width.ToString() + " : " + Screen.height.ToString();
    }
}
