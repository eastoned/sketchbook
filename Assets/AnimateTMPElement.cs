using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimateTMPElement : MonoBehaviour
{
    [Range(0f, 1f)]
    public float characterVisibility;

    [Range(0, 100)]
    public int currentCharacter;
    public TextMeshProUGUI textMesh;

    [TextArea(4, 10)]
    public string textOriginal;
    private string textVisible;
    void OnValidate()
    {
        textMesh.text = textOriginal.Substring(0, currentCharacter-1);
        //textMesh.text += 
        textMesh.text += "<color=red>";
        textMesh.text += textOriginal[currentCharacter-1];
    }
}
