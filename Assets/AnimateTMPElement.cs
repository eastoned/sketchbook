using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimateTMPElement : MonoBehaviour
{
    [Range(0f, 1f)]
    public float characterVisibility;

    [Range(1, 100)]
    public int currentCharacter;
    public string currentWord;
    public TextMeshProUGUI textMesh;

    [TextArea(4, 10)]
    public string textOriginal;
    private string textVisible;

    static byte floattoByte(float ro)
        {
            return (byte)(ro * 0x100);
        }
/*    void OnValidate()
    {
        textMesh.text = textOriginal;
        textMesh.ForceMeshUpdate();
        TMP_TextInfo textInfo = textMesh.textInfo;
        byte transparency = floattoByte(characterVisibility);
        
        TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
        Color32[][] originalColors = new Color32[textInfo.meshInfo.Length][];
        for (int i = 0; i < originalColors.Length; i++) {
            Color32[] theColors = textInfo.meshInfo[i].colors32;
            originalColors[i] = new Color32[theColors.Length];
            Array.Copy(theColors, originalColors[i], theColors.Length);
        }

        for(int j = 0; j < textInfo.characterCount; j++){
            TMP_CharacterInfo charInfo = textInfo.characterInfo[j];
            if(charInfo.isVisible){
                Color32[] destColors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;
                Color32 theColor = j < currentCharacter ? originalColors[charInfo.materialReferenceIndex][charInfo.vertexIndex] : new Color32(0,0,0,0);
                destColors[charInfo.vertexIndex + 0] = theColor;
                destColors[charInfo.vertexIndex + 1] = theColor;
                destColors[charInfo.vertexIndex + 2] = theColor;
                destColors[charInfo.vertexIndex + 3] = theColor;
            }
        }

        textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
       
        //textMesh.text += 
        //textMesh.text += "<color=red>";
        //textMesh.text += textOriginal[currentCharacter-1];
        //Shader.SetGlobalFloat("_CharacterVisibility", characterVisibility);
    }*/

    public void SetOriginalText(string text){
        textMesh.text = text;
    }

    public void UpdateTextVisibility(float textToShow){

        textMesh.ForceMeshUpdate();
        TMP_TextInfo textInfo = textMesh.textInfo;
        
        //TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
        Color32[][] originalColors = new Color32[textInfo.meshInfo.Length][];
        for (int i = 0; i < originalColors.Length; i++) {
            Color32[] theColors = textInfo.meshInfo[i].colors32;
            originalColors[i] = new Color32[theColors.Length];
            Array.Copy(theColors, originalColors[i], theColors.Length);
        }

        for(int j = 0; j < textInfo.characterCount; j++){
            TMP_CharacterInfo charInfo = textInfo.characterInfo[j];
            if(charInfo.isVisible){
                Color32[] destColors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;
                Color32 theColor = j < textToShow ? originalColors[charInfo.materialReferenceIndex][charInfo.vertexIndex] : new Color32(0,0,0,0);
                destColors[charInfo.vertexIndex + 0] = theColor;
                destColors[charInfo.vertexIndex + 1] = theColor;
                destColors[charInfo.vertexIndex + 2] = theColor;
                destColors[charInfo.vertexIndex + 3] = theColor;
            }
        }

        textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        //int currentChar = Mathf.Clamp((int)textToShow, 0, textOriginal.Length-1);
        //currentChar = currentChar < 1 ? 1);
        //Debug.Log(currentChar);
        //textMesh.textInfo
        
        //textMesh.text = textOriginal.Substring(0, currentChar);
        //textMesh.text += "<color=red>";
        //textMesh.text += textOriginal[currentChar];
        //characterVisibility = textToShow%1f;
        //Debug.Log(characterVisibility);
        //if(currentChar < textOriginal.Length-1){
        //    Shader.SetGlobalFloat("_CharacterVisibility", textToShow%1f);
        //}else{
        //    Shader.SetGlobalFloat("_CharacterVisibility", 1f);
        //}
        
    }
}
