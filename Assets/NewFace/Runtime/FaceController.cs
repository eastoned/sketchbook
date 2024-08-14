using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    public BodyPartController leftEye, rightEye, mouth, nose, head, leftEyebrow, rightEyebrow, bangs, hair, neck, leftEar, rightEar;
    public BodyPartController[] partControllers;
    public Transform[] bodyParts;
    public PartData[] bodyData;
    public enum EyeTarget
    {
        MOUSE,
        PART,
        BLANK
    }
    
    public EyeTarget eyeTarget;

    public float leftPupilX;
    public float leftPupilY;
    public float rightPupilX;
    public float rightPupilY;

    public Vector2 eyeLookAtPos;
    public float clampVal;
    public CharacterData currentChar;
    public AnimationCurve propertyCurve;
    public AnimationCurve blendCurve; 
    public AnimationCurve blinkCurve;
    public AnimationCurve scalePopCurve;

    public bool currentlyBlending = false;

    private Coroutine blending;

    [Range(0f, 1f)]
    public float blinkOverride;
    private float eyelidTop, eyelidBottom;
    [Range(0f, 1f)]
    public float mouthOverride = 1f;
    public float mouthOpen;

    public bool animating = false;

    public virtual void OnEnable()
	{

    }

    public virtual void OnDisable(){

    }

    public void Start()
    {
        //InitializeControllers();
        mouth.RandomizeData();
        neck.RandomizeData();
        head.RandomizeData();
        leftEye.RandomizeData();
        rightEye.RandomizeData();
        mouthOpen = mouth.GetSingleShaderFloat("_MouthOpen");
    }

    private void InitializeControllers()
    {
        //InitializeDictionaries();
        UpdateAllControllers();
    }

    public void Update()
    {
        float mouthValue = Mathf.Lerp(0.05f, mouthOpen, mouthOverride);
        mouth.UpdateSingleShaderFloat("_MouthOpen", mouthValue);
        mouth.UpdateRenderPropBlock();

        float rightX = 0f;
        float leftX = 0f;
        float rightY = 0f;
        float leftY = 0f;

        switch(eyeTarget)
        {
            case EyeTarget.MOUSE:
                eyeLookAtPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                rightX = rightEye.transform.position.x - eyeLookAtPos.x;
                rightY = rightEye.transform.position.y - eyeLookAtPos.y;
                leftX = leftEye.transform.position.x - eyeLookAtPos.x;
                leftY = leftEye.transform.position.y - eyeLookAtPos.y;

            break;
            case EyeTarget.PART:
            
            //eyeLookAtPos = new Vector2(0, 0);
            break;
            case EyeTarget.BLANK:
                rightX = 0.5f;
                leftX = 0.5f;
                rightY = 0.5f;
                leftY = 0.5f;
            //eyeLookAtPos = new Vector2();
            break;
        }

        //Shader.SetGlobalVector("_MousePos", eyeLookAtPos);
        //position of mouse relative to right eye position
        
        
        //Vector2 rotatedRight = Rotate2D(rightX, -rightEye.pd.currentAngle * Mathf.Deg2Rad);
        //Vector2 rotatedRight2 = Rotate2D(rightY, -rightEye.pd.currentAngle * Mathf.Deg2Rad);


        rightX = Mathf.Clamp(rightX/5f, -.5f, .5f);
        leftX = Mathf.Clamp(leftX/5f, -.5f, .5f);
        rightY = Mathf.Clamp(rightY/5f, -.25f, .25f);
        leftY = Mathf.Clamp(leftY/5f, -.25f, .25f);

        float rotatedRightX = (rightX*Mathf.Cos(-rightEye.transform.localEulerAngles.z * Mathf.Deg2Rad)) - (rightY*Mathf.Sin(-rightEye.transform.localEulerAngles.z * Mathf.Deg2Rad));
        float rotatedRightY = (rightX*Mathf.Sin(-rightEye.transform.localEulerAngles.z * Mathf.Deg2Rad)) + (rightY*Mathf.Cos(-rightEye.transform.localEulerAngles.z * Mathf.Deg2Rad));
        float rotatedLeftX = (leftX*Mathf.Cos(-leftEye.transform.localEulerAngles.z * Mathf.Deg2Rad)) - (leftY*Mathf.Sin(-leftEye.transform.localEulerAngles.z * Mathf.Deg2Rad));
        float rotatedLeftY = (leftX*Mathf.Sin(-leftEye.transform.localEulerAngles.z * Mathf.Deg2Rad)) + (leftY*Mathf.Cos(-leftEye.transform.localEulerAngles.z * Mathf.Deg2Rad));
        
        rightEye.UpdateSingleShaderFloatUnsafe("_PupilOffsetX", rotatedRightX);
        rightEye.UpdateSingleShaderFloatUnsafe("_PupilOffsetY", rotatedRightY);
        rightEye.UpdateRenderPropBlock();

        leftEye.UpdateSingleShaderFloatUnsafe("_PupilOffsetX", rotatedLeftX);
        leftEye.UpdateSingleShaderFloatUnsafe("_PupilOffsetY", rotatedLeftY);
        leftEye.UpdateRenderPropBlock();
        //Vector2 rotatedLeft = Rotate2D(leftX, rightEye.pd.currentAngle * Mathf.Deg2Rad);
        //Vector2 rotatedLeft2 = Rotate2D(leftY, rightEye.pd.currentAngle * Mathf.Deg2Rad);

        //clampVal = rightEye.pd.shadePropertyDict["_EyeRadius"].propertyValue/6f;
        //rightPupilX = rotatedRight.x - rotatedRight2.x;
        //rightPupilY = rotatedRight2.y - rotatedRight.y;

        //Vector2 rightPupilTarget = new Vector2(rotatedRight.x - rotatedRight2.x, rotatedRight2.y - rotatedRight.y);
        //Vector2 leftPupilTarget = new Vector2(rotatedLeft.x - rotatedLeft2.x, rotatedLeft2.y - rotatedLeft.y);

        //rightPupilX = Mathf.Lerp(rightPupilX, rightPupilTarget.x*clampVal, 4f* Time.deltaTime);
        //rightPupilY = Mathf.Lerp(rightPupilY, rightPupilTarget.y*clampVal, 4f* Time.deltaTime);
        //leftPupilX = Mathf.Lerp(leftPupilX, leftPupilTarget.x*clampVal, 4f* Time.deltaTime);
        //leftPupilY = Mathf.Lerp(leftPupilY, leftPupilTarget.y*clampVal, 4f* Time.deltaTime);
        
        //rightPupilX = Mathf.Lerp(rightPupilX, Mathf.Clamp(rightPupilTarget.x, -clampVal, clampVal), Time.deltaTime * 3f);
        //rightPupilY = Mathf.Lerp(rightPupilY, Mathf.Clamp(rightPupilTarget.y, -clampVal, clampVal), Time.deltaTime * 3f);
        //leftPupilX = Mathf.Lerp(leftPupilX, Mathf.Clamp(leftPupilTarget.x, -clampVal, clampVal), Time.deltaTime * 3f);
        //leftPupilY = Mathf.Lerp(leftPupilY, Mathf.Clamp(leftPupilTarget.y, -clampVal, clampVal), Time.deltaTime * 3f);
       // leftEye.UpdateSingleShaderValue("_PupilOffsetX", -leftPupilX);
       // leftEye.UpdateSingleShaderValue("_PupilOffsetY", leftPupilY);
       // leftEye.UpdateAllShadersValue(0f);
        //LeftEyeProp.SetFloat("_PupilOffsetX", -leftPupilX);
        //LeftEyeProp.SetFloat("_PupilOffsetY", leftPupilY);
        
        //rightEye.UpdateAllShadersValue(0f);
        //RightEyeProp.SetFloat("_PupilOffsetX", rightPupilX);
        //RightEyeProp.SetFloat("_PupilOffsetY", rightPupilY);
    }

    [ContextMenu("Refresh Connected Data")]
    public void RefreshDataConnection(){
        bodyData = new PartData[9];
        leftEye.pd = currentChar.eyeData;
        rightEye.pd = currentChar.eyeData;
        bodyData[5] = currentChar.eyeData;
        leftEyebrow.pd = currentChar.eyebrowData;
        rightEyebrow.pd = currentChar.eyebrowData;
        bodyData[6] = currentChar.eyebrowData;
        leftEar.pd = currentChar.earData;
        rightEar.pd = currentChar.earData;
        bodyData[4] = currentChar.earData;
        head.pd = currentChar.headData;
        bodyData[1] = currentChar.headData;
        neck.pd = currentChar.neckData;
        bodyData[0] = currentChar.neckData;
        nose.pd = currentChar.noseData;
        bodyData[7] = currentChar.noseData;
        mouth.pd = currentChar.mouthData;
        bodyData[8] = currentChar.mouthData;
        hair.pd = currentChar.hairBackData;
        bodyData[2] = currentChar.hairBackData;
        bangs.pd = currentChar.hairFrontData;
        bodyData[3] = currentChar.hairFrontData;
    }

    public void Interpolate(float val, CharacterData gameData, CharacterData blendFrom, CharacterData blendTo)
    {
        for(int i = 0; i < gameData.allParts.Length; i++)
        {
            if(gameData.allParts[i].activeInScene)
                BlendProfile(val, gameData.allParts[i], blendFrom.allParts[i], blendTo.allParts[i]);
        }
    }

    public void UpdateAllControllers()
    {
        foreach(BodyPartController pc in partControllers){
            if(pc.pd.activeInScene){
                pc.UpdateAllShadersValue(0f);
                if(!pc.detached){
                   pc.UpdateAllTransformValues(); 
                }
            }
        }
    }

    public void InitializeDictionaries()
    {
        foreach(BodyPartController pc in partControllers){
            pc.InitializePartDataDictionary();
        }
    }

    public void BlendProfile(float val, PartData partData, PartData blendFrom, PartData blendTo){
        
        //partData.absoluteWorldPosition = Vector3.Lerp(blendFrom.absoluteWorldPosition, blendTo.absoluteWorldPosition, val);
        partData.relativeToParentPosition = Vector3.Lerp(blendFrom.relativeToParentPosition, blendTo.relativeToParentPosition, val);
        partData.relativeToParentAngle = Mathf.Lerp(blendFrom.relativeToParentAngle, blendTo.relativeToParentAngle, val);
        //partData.absoluteWorldScale = Vector3.Lerp(blendFrom.absoluteWorldScale, blendTo.absoluteWorldScale, val);
        partData.relativeToParentScale = Vector3.Lerp(blendFrom.relativeToParentScale, blendTo.relativeToParentScale, val);

        for(int i = 0; i < partData.shaderProperties.Count; i++){
            partData.shaderProperties[i].SetValue(Mathf.Lerp(blendFrom.shaderProperties[i].propertyValue, blendTo.shaderProperties[i].propertyValue, val));
        }

        for(int j = 0; j < partData.shaderColors.Count; j++){
            partData.shaderColors[j].colorValue = Color.Lerp(blendFrom.shaderColors[j].colorValue, blendTo.shaderColors[j].colorValue, val);
        }

    }

    public void BlendCharacter(CharacterData char1, CharacterData char2, float animLength)
    {
        if(blending != null){
            StopCoroutine(blending);
        }

        blending = StartCoroutine(Blend(char1, char2, animLength));
    }

    public void SetCharacter(CharacterData characterToBe){
        currentChar.CopyData(characterToBe);
        UpdateAllControllers();
    }

    public Coroutine BlendCharacterSequence(CharacterData char1, CharacterData char2, float animLength){
        return StartCoroutine(Blend(char1, char2, animLength));
    }

    public Coroutine BlendPartSequence(CharacterData char1, CharacterData char2, int partID, float animLength){
        return StartCoroutine(BlendPart(char1, char2, partID, animLength));
    }

    public IEnumerator Blend(CharacterData cd1, CharacterData cd2, float value){
        float journey = 0;
        currentlyBlending = true;
        while(journey < value){
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey/value);
            float blendPercent = blendCurve.Evaluate(percent);
            Interpolate(blendPercent, currentChar, cd1, cd2);
            UpdateAllControllers();
            yield return null;
        }

        currentChar.CopyData(cd2);

        if(cd1.writeable){
            cd1.CopyData(cd2);
        }

        currentlyBlending = false;
    }

    public IEnumerator BlendPart(CharacterData char1, CharacterData char2, int partID, float animLength){
        float journey = 0;
        currentlyBlending = true;
        while(journey < animLength){
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey/animLength);
            float blendPercent = blendCurve.Evaluate(percent);
            BlendProfile(blendPercent, currentChar.allParts[partID], char1.allParts[partID], char2.allParts[partID]);
            UpdateAllControllers();
            yield return null;
        }
        
        if(char1.writeable){
            char1.allParts[partID].CopyData(char2.allParts[partID]);
        }
        currentlyBlending = false;
        Debug.Log("copied " + currentChar.allParts[partID].name + " data from blend target to writeable character");
    }

    private IEnumerator AnimatePartShaderProperty(BodyPartController pc, string shaderParam, float animationLength)
    {
        float animationTime = 0;
        float initialPropertyValue = pc.GetSingleShaderFloat(shaderParam);

        while(animationTime < animationLength)
        {
            float interval = Mathf.Clamp01(animationTime/animationLength);
            interval = propertyCurve.Evaluate(interval);
            pc.UpdateSingleShaderFloat(shaderParam, initialPropertyValue * interval);
            pc.UpdateRenderPropBlock();
            animationTime += Time.deltaTime;
            yield return null;
        }
    }

}