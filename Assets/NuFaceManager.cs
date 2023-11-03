using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuFaceManager : MonoBehaviour
{
    public FaceController fc;
    public SpeechController sc;

    public CharacterData currentChar;

    public string[] convo;

    public CharacterData[] characterSet;

    public IEnumerator Start(){
        yield return new WaitForSeconds(1f);
        sc.SpeakText(convo[0], 1f);
        yield return new WaitForSeconds(3f);
        fc.BlendCharacter(characterSet[0], characterSet[1], 5f);
        sc.SpeakText(convo[1], 3f);
        yield return new WaitForSeconds(6f);
        fc.BlendCharacter(characterSet[1], characterSet[2], 2f);
        yield return new WaitForSeconds(2f);
        sc.SpeakText(convo[2], 2f);
       // StartCoroutine(SpeakRoutine(convo[2]));
        yield return new WaitForSeconds(6f);
        fc.BlendCharacter(characterSet[2], characterSet[0], 1f);
       // StartCoroutine(BlendRoutine());
    }

    public IEnumerator ResponseRoutine(){
        yield return null;
    }
}
