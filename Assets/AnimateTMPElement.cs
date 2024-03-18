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
        Shader.SetGlobalFloat("_CharacterVisibility", characterVisibility);
    }

    public void SetOriginalText(string text){
        textOriginal = text;
    }

    public void UpdateTextVisibility(float textToShow){
        int currentChar = Mathf.Clamp((int)textToShow, 0, textOriginal.Length-1);
        //currentChar = currentChar < 1 ? 1);
        Debug.Log(currentChar);
        
        textMesh.text = textOriginal.Substring(0, currentChar);
        textMesh.text += "<color=red>";
        textMesh.text += textOriginal[currentChar];
        characterVisibility = textToShow%1f;
        //Debug.Log(characterVisibility);
        if(currentChar < textOriginal.Length-1){
            Shader.SetGlobalFloat("_CharacterVisibility", textToShow%1f);
        }else{
            Shader.SetGlobalFloat("_CharacterVisibility", 1f);
        }
        
    }
}
