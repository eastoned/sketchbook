using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class NuFaceManager : MonoBehaviour
{
    public FaceController fc;
    public SpeechController sc;

    public CharacterData currentChar;
    public CharacterData writeableData;

    public string[] convo;

    public PartController[] parts;

    public CharacterData[] characterSet;
    private Coroutine skippableWait;
    public List<RequestChange> requestList;

    public IEnumerator Start(){
        writeableData.CopyData(characterSet[0]);
        foreach(PartController pc in parts){
            pc.rend.enabled = false;
            pc.colid.enabled = false;
        }

        yield return BirthRoutine();
        yield return sc.SpeakText("Birth initiated.", 3f);
        yield return new WaitForSeconds(0.5f);
        //yield return WaitForMouse();
        
        yield return new WaitForSeconds(0.5f);
        //yield return WaitForMouse();
        yield return BloomRoutine();
        yield return new WaitForSeconds(0.5f);
        yield return EarRoutine();
        yield return new WaitForSeconds(1f);
        yield return sc.SpeakText("Can you \nmake me \nin your image?", 5f);
        //yield return HairRoutine();
        foreach(PartController pc in parts){
            pc.rend.enabled = true;
            pc.colid.enabled = true;
        }
        yield return new WaitForSeconds(1f);

        yield return WaitForRequest(requestList[0]);

        yield return WaitForRequest(requestList[1]);

        yield return WaitForRequest(requestList[2]);

        fc.BlendCharacter(writeableData, characterSet[1], 3f);

        yield return WaitForRequest(requestList[3]);
        writeableData.CopyData(characterSet[1]);

        fc.BlendCharacter(writeableData, characterSet[2], 3f);

        yield return WaitForRequest(requestList[4]);
        writeableData.CopyData(characterSet[2]);

        fc.BlendCharacter(writeableData, characterSet[0], 2f);
        //parts[10].colid.enabled = true;
        //parts[11].colid.enabled = true;
    }

    IEnumerator BirthRoutine(){
        parts[9].rend.enabled = true;
        yield return TransformAnimation(parts[9].transform, new Vector3(0f,-2f,0.2f), new Vector3(0f,-1f,0.2f), new Vector3(0f, 0f, 1f), new Vector3(2f, 2f, 1f), 5f);
        parts[9].colid.enabled = true;
        yield return null;
    }

    IEnumerator BloomRoutine(){
        parts[4].rend.enabled = true;
        yield return TransformAnimation(parts[4].transform, new Vector3(0f, 0f, 0.1f), parts[4].pd.GetAbsolutePosition(), new Vector3(0f, 0f, 1f), parts[4].pd.GetAbsoluteScale(), 3f);
        parts[4].UpdateDependencies();
        parts[4].colid.enabled = true;
        yield return null;
    }

    IEnumerator EarRoutine(){
        parts[7].rend.enabled = true;
        parts[8].rend.enabled = true;
        StartCoroutine(TransformAnimation(parts[7].transform, new Vector3(0, 0, 0.15f), parts[7].pd.GetAbsolutePosition(), new Vector3(0, 0, 1f), parts[7].pd.GetAbsoluteScale(), 2f));
        yield return TransformAnimation(
            parts[8].transform,
            new Vector3(0, 0, 0.15f),
            new Vector3(-parts[7].pd.GetAbsolutePosition().x,parts[7].pd.GetAbsolutePosition().y,parts[7].pd.GetAbsolutePosition().z),
            new Vector3(0, 0, 1f),
            new Vector3(-parts[7].pd.GetAbsoluteScale().x,parts[7].pd.GetAbsoluteScale().y,parts[7].pd.GetAbsoluteScale().z),
            2f);
        parts[7].colid.enabled = true;
        parts[8].colid.enabled = true;
        yield return null;
    }

    IEnumerator HairRoutine(){
        parts[10].rend.enabled = true;
        yield return TransformAnimation(parts[10].transform, new Vector3(0, 0, 0.3f), new Vector3(0, 0, 0.3f), new Vector3(0, 0, 1f), new Vector3(2.5f, 2.5f, 1f), 3f);
        parts[10].colid.enabled = true;
        parts[11].rend.enabled = true;
        yield return TransformAnimation(parts[11].transform, new Vector3(0, 0, 0.3f), new Vector3(0, 1f, 0.3f), new Vector3(0, 0, 1f), new Vector3(2.5f, -1f, 1f), 3f);
        yield return TransformAnimation(parts[11].transform, new Vector3(0, 1f, 0.3f), new Vector3(0, 1.25f, -0.1f), new Vector3(2.5f, -1f, 1f), new Vector3(2.5f, 0f, 1f), 2f);
        yield return TransformAnimation(parts[11].transform, new Vector3(0, 1.25f, -0.1f), new Vector3(0, 1f, -0.1f), new Vector3(2.5f, 0f, 1f), new Vector3(2.5f, 1f, 1f), 3f);
        parts[11].colid.enabled = true;
        yield return null;
    }

    IEnumerator TransformAnimation(Transform transformToAnimate, Vector3 startPosition, Vector3 endPosition, Vector3 startScale, Vector3 endScale, float timeToAnimate){
        float counter = 0f;
        while(counter <= timeToAnimate){
            counter += Time.deltaTime;
            float frame = Mathf.Clamp01(counter/timeToAnimate);
            transformToAnimate.position = Vector3.Lerp(startPosition, endPosition, frame);
            transformToAnimate.localScale = Vector3.Lerp(startScale, endScale, frame);
            yield return null;
        }

        transformToAnimate.position = endPosition;
        transformToAnimate.localScale = endScale;
    }

    IEnumerator WaitForRequest(RequestChange rc){
        yield return sc.SpeakText(rc.requestMessage, 2f);
        rc.SetCache(rc.partToChange.pd);
        rc.SetListenersForCorrectEvent();
        while(!rc.CheckTotalRequestFulfilled()){
            yield return rc.partToChange.ShakeRoutine(new Vector3(.05f, 0.01f, 0f), .5f);
            if(rc.partToChange.mirroredPart != null){
                yield return rc.partToChange.mirroredPart.ShakeRoutine(new Vector3(.05f, 0.01f, 0f), .5f);
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return sc.SpeakText(rc.successMessage, 2f);
    }

    IEnumerator WaitForMouse(){
        while(!Input.GetMouseButtonDown(0)){
            yield return null;
        }
    }

    IEnumerator WaitForMouseOrTime(float length){
        while(!Input.GetMouseButtonDown(0) || length > 0){
            yield return null;
            length -= Time.deltaTime;
        }
    }
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

    void OnValidate(){
        foreach(RequestChange rc in requestList){
            if(rc.shaderRequests.Count > 0){
                for(int i = 0; i < rc.shaderRequests.Count; i++){ 
                    rc.shaderRequests[i].valueName = rc.partToChange.pd.shaderProperties[Mathf.Clamp(rc.shaderRequests[i].shaderVariable, 0, rc.partToChange.pd.shaderProperties.Count)].propertyName;
                }
            }
        }
    }

    //public IEnumerator ResponseRoutine(){
      //  yield return null;
    //}
}
