using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NuFaceManager : MonoBehaviour
{
    public FaceController[] rfc;
    public PlayerFaceController pfc;
    public SpeechController sc;
    public CharacterData[] writeableData;
    public CharacterData[] targetData;

    public string[] convo;

    public PartController[] parts;

    public CharacterData[] characterSet;
    private Coroutine skippableWait;
    public List<RequestChange> requestList;
    public int count = 0;
    public int charStage = 0;

    public int[] stageThresholds;

    public static float money = 0f;
    public TextMeshProUGUI scoreDebug, moneyDebug;

    public bool isGame = false;

    void OnEnable(){
        OnConfirmTransformPart.Instance.AddListener(UpdateTextAmount);
        OnDeselectedFacePartEvent.Instance.AddListener(DebugTouchBG);
    }

    void OnDisable(){
        OnConfirmTransformPart.Instance.RemoveListener(UpdateTextAmount);
        OnDeselectedFacePartEvent.Instance.RemoveListener(DebugTouchBG);
    }

    void DebugTouchBG(){
        scoreDebug.text = "Touched BG";
    }

    private IEnumerator Start(){
        if(isGame){

        
            targetData[0].RandomizeData(.1f);
            writeableData[0].CopyData(rfc[0].currentChar);
            rfc[0].BlendCharacter(writeableData[0], targetData[0], 1f);
            stageThresholds = new int[9];
            int scoreCount = 0;
            for(int i = 0; i < stageThresholds.Length; i++){
                scoreCount += 3;
                scoreCount += pfc.bodyData[i].shaderProperties.Count;
                stageThresholds[i] = scoreCount;
            }

            AffectStageCount(1);
        }else{

            for(;;){
                //yield return new WaitForSeconds(2f);
                //Compare();
                //writeableData.CopyData(rfc.currentChar);
                //targetData.RandomizeData();
                //targetData.RandomizeRandomPart();
                //rfc.BlendCharacter(writeableData, targetData, 1f);
                ///RandomizePlayer();
                yield return new WaitForSeconds(.2f);
            }
        }
    }

    [ContextMenu("Compare Faces")]
    public void Compare(){
        //money += GetCharacterDifference(rfc[0].currentChar, pfc.currentChar);
        //scoreDebug.text = "Compare Score: " + GetCharacterDifference(rfc[0].currentChar, pfc.currentChar).ToString();
        float result = GetDataDifference(rfc[0].bodyData, pfc.bodyData);
        scoreDebug.text = "Compare Score: " + result.ToString();
        UpdateTextAmount();
        if(result >= stageThresholds[charStage-1]){
            Debug.Log("Moving on to next stage");
            if(charStage < stageThresholds.Length){
                RandomizeFace();
                AffectStageCount(1); 
            }else{
                scoreDebug.text = "You win"!;
            }
        }else{
            scoreDebug.text = "You got: " + result + " components right. You need: " + stageThresholds[charStage-1];
            Debug.Log("Try Again");
        }
    }
    public void AffectStageCount(int diff){
        charStage += diff;

        for(int i = 0; i < pfc.bodyParts.Length; i++){
            pfc.bodyParts[i].gameObject.GetComponent<MeshRenderer>().enabled = i < charStage;
            pfc.bodyParts[i].gameObject.GetComponent<BoxCollider2D>().enabled = i < charStage;
            if(pfc.bodyParts[i].gameObject.GetComponent<PartController>().mirroredPart){
                pfc.bodyParts[i].gameObject.GetComponent<PartController>().mirroredPart.GetComponent<MeshRenderer>().enabled = i < charStage;
                pfc.bodyParts[i].gameObject.GetComponent<PartController>().mirroredPart.GetComponent<BoxCollider2D>().enabled = i < charStage;
            }
        }

        for(int i = 0; i < rfc[0].bodyParts.Length; i++){
            rfc[0].bodyParts[i].gameObject.GetComponent<MeshRenderer>().enabled = i < charStage;
            if(rfc[0].bodyParts[i].gameObject.GetComponent<PartController>().mirroredPart){
                rfc[0].bodyParts[i].gameObject.GetComponent<PartController>().mirroredPart.GetComponent<MeshRenderer>().enabled = i < charStage;
            }
        }
    }

    void UpdateTextAmount(){
        moneyDebug.text = "Money Amount: " + money.ToString();
    }

    public void Routine(){
        
            //yield return new WaitForSeconds(Random.Range(0.5f, 5f));
            //writeableData.CopyData(currentChar);
            //targetData.RandomizeData();
                //yield return fc.BlendPartSequence(writeableData, targetData, Random.Range(0, targetData.allParts.Length), Random.Range(.2f, 4f));
            
            
           // if(Random.Range(0f, 1f) < 0.5f){
            //    int randomChar = Random.Range(1, 3);
           //     yield return fc.BlendPartSequence(writeableData, characterSet[randomChar], Random.Range(0, characterSet[randomChar].allParts.Length), Random.Range(.2f, 4f));
           // }else{
                //if(Random.Range(0f, 1f) < 0.5f){
                    
                //}else{
                   //targetData.RandomizeData(); 
                //}
                
                //yield return fc.BlendCharacterSequence(writeableData, targetData, Random.Range(.2f, 4f));
            //}
            
        
        /*
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
        */
    }
    public float counter = 0;
    [ContextMenu("Randomize Player")]
    public void RandomizePlayer(){
        //Debug.Log("let random begin");
        //Debug.Log(targetData[rfc.Length - 1].name);
       // if(Random.Range(0f, 1f) < 0.5f){
            targetData[1].RandomizeData(Random.Range(0f, .5f));
            if(counter < .5f){
                counter += .05f;}
else{
    counter = 0f;
}        //}else{
           // targetData[0].RandomizeRandomPart();
        //}
        
        writeableData[1].CopyData(pfc.currentChar);
        pfc.BlendCharacter(writeableData[1], targetData[1], Random.Range(.05f, .2f));
    }
    [ContextMenu("Randomize Face")]
    public void RandomizeFace(){
        //Debug.Log("let random begin");
        //Debug.Log(targetData[rfc.Length - 1].name);
       // if(Random.Range(0f, 1f) < 0.5f){
            targetData[0].RandomizeData(.2f);
        //}else{
           // targetData[0].RandomizeRandomPart();
        //}
        
        writeableData[0].CopyData(rfc[0].currentChar);
        rfc[0].BlendCharacter(writeableData[0], targetData[0], 1f);

        /*
        if(Random.Range(0f,1f) < 0.5f){
            targetData[rfc.Length-1].RandomizeData();
            for(int i = 0; i < rfc.Length; i++){
                writeableData[i].CopyData(rfc[i].currentChar);
                float val = (float)i/((float)rfc.Length-1);
                Debug.Log("interval val is: " + val);
                rfc[i].Interpolate(val, targetData[i], targetData[0], targetData[rfc.Length-1]);
                rfc[i].BlendCharacter(writeableData[i], targetData[i], (val+1)*2);
            }
        }else{
            targetData[0].RandomizeData();
            for(int i = rfc.Length-1; i > -1; i--){
                writeableData[i].CopyData(rfc[i].currentChar);
                float val = (float)i/((float)rfc.Length-1);
                Debug.Log("interval val is: " + val);
                rfc[i].Interpolate(val, targetData[i], targetData[0], targetData[rfc.Length-1]);
                rfc[i].BlendCharacter(writeableData[i], targetData[i], (val+1)*2);
            }
        }*/
        
    }

    [ContextMenu("Set To Player")]
    public void SetPlayer(){
        targetData[0].CopyData(pfc.currentChar);
        writeableData[0].CopyData(rfc[0].currentChar);
        rfc[0].BlendCharacter(writeableData[0], targetData[0], 1f);
    }

    public void SetToReference(){
        targetData[1].CopyData(rfc[0].currentChar);
        writeableData[1].CopyData(pfc.currentChar);
        pfc.BlendCharacter(writeableData[1], targetData[1], 1f);
    }

    IEnumerator CountIncreaser(){
        yield return new WaitForSeconds(5f);
        RandomizeFace();
    }

    IEnumerator BirthRoutine(){
        parts[9].rend.enabled = true;
        yield return TransformAnimation(parts[9].transform, new Vector3(0f,-2f,0.2f), new Vector3(0f,-1f,0.2f), new Vector3(0f, 0f, 1f), new Vector3(2f, 2f, 1f), 1f);
        parts[9].colid.enabled = true;
        yield return null;
    }

    IEnumerator BloomRoutine(){
        parts[4].rend.enabled = true;
        yield return TransformAnimation(parts[4].transform, new Vector3(0f, 0f, 0.1f), parts[4].pd.GetAbsolutePosition(), new Vector3(0f, 0f, 1f), parts[4].pd.GetAbsoluteScale(), 1f);
        parts[4].UpdateDependencies();
        parts[4].colid.enabled = true;
        yield return null;
    }

    IEnumerator EarRoutine(){
        parts[7].rend.enabled = true;
        parts[8].rend.enabled = true;
        StartCoroutine(TransformAnimation(parts[7].transform, new Vector3(0, 0, 0.15f), parts[7].pd.GetAbsolutePosition(), new Vector3(0, 0, 1f), parts[7].pd.GetAbsoluteScale(), 1f));
        yield return TransformAnimation(
            parts[8].transform,
            new Vector3(0, 0, 0.15f),
            new Vector3(-parts[7].pd.GetAbsolutePosition().x,parts[7].pd.GetAbsolutePosition().y,parts[7].pd.GetAbsolutePosition().z),
            new Vector3(0, 0, 1f),
            new Vector3(-parts[7].pd.GetAbsoluteScale().x,parts[7].pd.GetAbsoluteScale().y,parts[7].pd.GetAbsoluteScale().z),
            1f);
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

    [ContextMenu("Debug Animation")]
    void CharacterExit(){
        foreach(PartController part in parts){
            part.transform.SetParent(transform);
        }
           
        //yield return null;
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

    public IEnumerator Consume(){
        foreach(PartController part in parts){
            part.StopAllCoroutines();
            part.transform.SetParent(transform);
        }
        yield return TransformAnimation(transform, new Vector3(0, -2.5f, 0), new Vector3(0, -85, 0), new Vector3(1, 1, 1), new Vector3(40f, 40f, 1f), .5f);
        //transform.position = new Vector3(0, -2.5f, 0);
        yield return null;
    }

    public float GetCharacterDifference(CharacterData gameData, CharacterData targetData){
        float score = 0;
        
        score += GetPartDifference(gameData.headData, targetData.headData);
        score += GetPartDifference(gameData.neckData, targetData.neckData);
        score += GetPartDifference(gameData.eyeData, targetData.eyeData);
        score += GetPartDifference(gameData.eyebrowData, targetData.eyebrowData);
        score += GetPartDifference(gameData.noseData, targetData.noseData);
        score += GetPartDifference(gameData.mouthData, targetData.mouthData);
        score += GetPartDifference(gameData.earData, targetData.earData);
        score += GetPartDifference(gameData.hairFrontData, targetData.hairFrontData);
        score += GetPartDifference(gameData.hairBackData, targetData.hairBackData);
       Debug.Log("Similarity score between current face and : " + targetData.name + " is : " + score);
       money += score;
       return score;
    }
    public float GetDataDifference(PartData[] data1, PartData[] data2){
        float score = 0;

        for(int i = 0; i < charStage; i++){
            score += GetPartDifference(data1[i], data2[i]);
        }
        
        money += score;
        return score;
    }

    public float GetFaceDifference(FaceController fc1, FaceController fc2){
        float score = 0;

        for(int i = 0; i < charStage; i++){
            //skip adding score if duplicate piece
            if(!fc1.bodyParts[i].GetComponent<PartController>().flippedXAxis){
                score += GetPartDifference(fc1.bodyParts[i].GetComponent<PartController>().pd, fc2.bodyParts[i].GetComponent<PartController>().pd);
            }
        }
        
        money += score;
        return score;
    }

    public float GetPartDifference(PartData gamePart, PartData characterPart)
    {
        string diffDebug = "";
        float score = 0;
 
        score += Vector3.Distance(gamePart.relativePosition, characterPart.relativePosition) < 0.2f ? 1 : 0;
        diffDebug += "Position is close enough? " + (Vector3.Distance(gamePart.relativePosition, characterPart.relativePosition) < 0.2f ? "Yes" : "No") + "\n";

        //Debug.Log(gamePart.name + " scale diff is: " + Vector3.Distance(gamePart.relativeScale, characterPart.relativeScale));
        score += Vector3.Distance(gamePart.relativeScale, characterPart.relativeScale) < 0.2f ? 1 : 0;
        diffDebug += "Scale is close enough? " + (Vector3.Distance(gamePart.relativeScale, characterPart.relativeScale) < 0.2f ? "Yes" : "No")+ "\n";

        //Debug.Log(gamePart.name + " angle diff is: " + Mathf.Abs(gamePart.currentAngle - characterPart.currentAngle)%360);
        score += ((Mathf.Abs(gamePart.currentAngle - characterPart.currentAngle)%360)/180f) < 0.05f ? 1 : 0;
        diffDebug += "Angle is close enough? " + (((Mathf.Abs(gamePart.currentAngle - characterPart.currentAngle)%360)/180f) < 0.05f ? "Yes" : "No")+ "\n";

        for(int i = 0; i < gamePart.shaderProperties.Count; i++){
            score += Mathf.Abs(gamePart.shaderProperties[i].propertyValue - characterPart.shaderProperties[i].propertyValue) < 0.2f ? 1 : 0;
            diffDebug += gamePart.shaderProperties[i].propertyName + " is close enough? " + (Mathf.Abs(gamePart.shaderProperties[i].propertyValue - characterPart.shaderProperties[i].propertyValue) < 0.2f ? "Yes" : "No")+ "\n";
        }

        for(int j = 0; j < gamePart.shaderColors.Count; j++){
            Vector3 currentColor = new Vector3(gamePart.shaderColors[j].GetValue(), gamePart.shaderColors[j].GetHue(), gamePart.shaderColors[j].GetSaturation()).normalized;
            Vector3 charColor = new Vector3(characterPart.shaderColors[j].GetValue(), characterPart.shaderColors[j].GetHue(), characterPart.shaderColors[j].GetSaturation()).normalized;
        }
        //Debug.Log("Similarity score between : " + gamePart.name + " is : " + score);
        Debug.Log("Graded: " + gamePart + "\n" + diffDebug);

        return score;
    }

}