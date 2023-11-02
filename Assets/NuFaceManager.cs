using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuFaceManager : MonoBehaviour
{
    public FaceController fc;
    public SpeechController sc;

    public CharacterData currentChar;

    public string[] convo;

    public void Start(){
        //StartCoroutine(SpeakRoutine(convo[0]));
        //yield return new WaitForSeconds(6f);
        //StartCoroutine(SpeakRoutine(convo[1]));
        //yield return new WaitForSeconds(6f);
        //StartCoroutine(BlendRoutine());
       // yield return new WaitForSeconds(4f);
       // StartCoroutine(SpeakRoutine(convo[2]));
       // yield return new WaitForSeconds(6f);
       // StartCoroutine(BlendRoutine());
    }

    public IEnumerator SpeakRoutine(string text){
        sc.SpeakText(text, 5f);
        yield return null;
    }

    public IEnumerator BlendRoutine(){
        yield return null;
    }

    public IEnumerator ResponseRoutine(){
        yield return null;
    }
}
