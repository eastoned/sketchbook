using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuFaceManager : MonoBehaviour
{
    public FaceController fc;
    public SpeechController sc;

    public CharacterData currentChar;
    public CharacterData writeableData;

    public string[] convo;

    public CharacterData[] characterSet;
/*
    public IEnumerator Start(){
        for(;;){
            yield return new WaitForSeconds(5f);
            writeableData.CopyData(currentChar);
            if(Random.Range(0f, 1f) < 0.5f){
                yield return new WaitForSeconds(1f);
                fc.BlendCharacter(writeableData, characterSet[1], 2f);
                yield return new WaitForSeconds(3f);
                sc.SpeakText("Hi bitch.", 2f);
                yield return new WaitForSeconds(3f);
                sc.SpeakText("Fine.", 1f);
                yield return new WaitForSeconds(0.5f);
                sc.SpeakText("I'll go.", 1f);
                yield return new WaitForSeconds(1.5f);
                sc.SpeakText("No really.", 1f);
                yield return new WaitForSeconds(1f);
                sc.SpeakText("It's fine.", 1f);
                yield return new WaitForSeconds(3f);
                sc.SpeakText("Byeeeee.", 2f);
                yield return new WaitForSeconds(1f);
                fc.BlendCharacter(characterSet[1], writeableData, 2f);
            }else{
                yield return new WaitForSeconds(1f);
                fc.BlendCharacter(writeableData, characterSet[2], 3f);
                yield return new WaitForSeconds(3.5f);
                sc.SpeakText("Hi beautiful.", 2f);
                yield return new WaitForSeconds(4f);
                sc.SpeakText("You're looking gooooooood. :D", 3f);
                yield return new WaitForSeconds(4f);
                sc.SpeakText("See U Around. <3", 2f);
                yield return new WaitForSeconds(1f);
                fc.BlendCharacter(characterSet[2], writeableData, 4f);
            }
        }
       // StartCoroutine(BlendRoutine());
    }*/

    [ContextMenu("Sheep Score")]
    public void CheckSheepAmount(){
        //float sheepScore = 0;
        //Debug.Log("Sheep score: " + sheepScore);
        fc.GetCharacterDifference(currentChar, characterSet[1]);
    }

    [ContextMenu("Wolf Score")]
    public void CheckWolfAmount(){
        fc.GetCharacterDifference(currentChar, characterSet[2]);
    }

    //public IEnumerator ResponseRoutine(){
      //  yield return null;
    //}
}
