using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using JetBrains.Annotations;
using OpenCvSharp;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR;

public class FaceController : MonoBehaviour
{

    public PartController leftEye, rightEye, mouth;

    public float leftPupilX;
    public float leftPupilY;
    public float rightPupilX;
    public float rightPupilY;

    public Vector2 mousePos;
    public float clampVal;
    public PartController currentPC;
    public Transform currentTransform;
    
    #if UNITY_EDITOR
    public SaveCharacterProfile scp;
    #endif

    public CharacterData currentChar;
    public AnimationCurve blendCurve; 

    [Range(0f, 1f)]
    public float blinkAmount;
    private float eyelidTop, eyelidBottom;
    [Range(0f, 1f)]
    public float mouthClosedAmount;
    private float mouthTop, mouthBottom;

    public Transform cube;
    public Vector3 positionDifference;

    public PartTransformController widthLeft, widthRight, heightTop;

     void OnEnable()
	{
        OnSelectedNewFacePartEvent.Instance.AddListener(SetTransformControllers);
        OnDeselectedFacePartEvent.Instance.AddListener(DisappearControllers);
        OnTranslatePartController.Instance.AddListener(SetPartPosition);
        OnRotatePartController.Instance.AddListener(SetPartRotation);
        OnScalePartController.Instance.AddListener(SetPartScale);
        OnSetKeyFrameData.Instance.AddListener(SetDefaultBlinkAndMouthPos);
    }

    void OnDisable(){
        OnSelectedNewFacePartEvent.Instance.RemoveListener(SetTransformControllers);
        OnDeselectedFacePartEvent.Instance.RemoveListener(DisappearControllers);
        OnTranslatePartController.Instance.RemoveListener(SetPartPosition);
        OnRotatePartController.Instance.RemoveListener(SetPartRotation);
        OnScalePartController.Instance.RemoveListener(SetPartScale);
        OnSetKeyFrameData.Instance.RemoveListener(SetDefaultBlinkAndMouthPos);
    }

    public void SetDefaultBlinkAndMouthPos(){
        Debug.Log("Set new eye and mouth vars");
        eyelidTop = rightEye.pd.shadePropertyDict["_EyelidTopOpen"].propertyValue;
        eyelidBottom = rightEye.pd.shadePropertyDict["_EyelidBottomOpen"].propertyValue;
        mouthTop = mouth.pd.shadePropertyDict["_MouthLipTop"].propertyValue;
        mouthBottom = mouth.pd.shadePropertyDict["_MouthLipBottom"].propertyValue;
    }

    private void DisappearControllers(){
        widthLeft.transform.localPosition = new Vector3(100, 100, 100);
        widthRight.transform.localPosition = new Vector3(100, 100, 100);
        heightTop.transform.localPosition = new Vector3(100, 100, 100);
    }

    private void SetTransformControllers(Transform selectedTarget){
        
        currentPC = selectedTarget.GetComponent<PartController>();
        
        if(currentTransform != selectedTarget){
            currentTransform = selectedTarget;
        
            cube.position = currentTransform.position;
        }
        
        DisappearControllers();

        if(currentPC.translatable)
            widthLeft.transform.localPosition = selectedTarget.TransformPoint(new Vector3(0, 0, 0));

        widthLeft.transform.localPosition = new Vector3(widthLeft.transform.localPosition.x, widthLeft.transform.localPosition.y, -1f);

        if(currentPC.rotatable)
            widthRight.transform.localPosition = selectedTarget.TransformPoint(new Vector3(-0.5f, 0, 0));
        
        widthRight.transform.localPosition = new Vector3(widthRight.transform.localPosition.x, widthRight.transform.localPosition.y, -1f);

        if(currentPC.scalable)
            heightTop.transform.localPosition = selectedTarget.TransformPoint(new Vector3(0.5f, 0.5f, 0));
        
        heightTop.transform.localPosition = new Vector3(heightTop.transform.localPosition.x, heightTop.transform.localPosition.y, -1f);
    
    }

    private void SetPartPosition(Vector3 pos){
        //each part has a relative position to other objects
        float flip = currentPC.flippedXAxis? -1f : 1f;

        currentTransform.localPosition = new Vector3(pos.x, pos.y, currentTransform.localPosition.z);
        Vector3 absPos = new Vector3(pos.x*flip, pos.y, currentTransform.localPosition.z);

        currentPC.pd.ClampedPosition(absPos);

        if(currentPC.affectedParts.Count > 0){
            for(int i = 0; i < currentPC.affectedParts.Count; i++){
                currentPC.affectedParts[i].pd.SetPositionBounds(currentPC.pd);
                currentPC.affectedParts[i].pd.SetScaleBounds(currentPC.pd);

                currentPC.affectedParts[i].UpdateAllTransformValues();
            }
        }

        currentPC.UpdateAllTransformValues();

        if(currentPC.mirroredPart != null){
            currentPC.mirroredPart.UpdateAllTransformValues();
        }
        
        SetTransformControllers(currentTransform);
    }

    private void SetPartScale(Vector3 pos){

        float flip = 1;

        if(currentPC.flippedXAxis)
            flip = -1;
        
        Vector3 diff = currentTransform.InverseTransformDirection(currentTransform.localPosition - pos)*2f;
        diff = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), 1);

        currentPC.pd.ClampedScale(diff);

        if(currentPC.affectedParts.Count > 0){
            for(int i = 0; i < currentPC.affectedParts.Count; i++){
                currentPC.affectedParts[i].pd.SetPositionBounds(currentPC.pd);
                currentPC.affectedParts[i].pd.SetScaleBounds(currentPC.pd);
                currentPC.affectedParts[i].UpdateAllTransformValues();
            }
        }

        currentPC.UpdateAllTransformValues();
        
        if(currentPC.mirroredPart != null){
            currentPC.mirroredPart.UpdateAllTransformValues();
        }
        //currentPC.pd.SetPositionBounds();

        SetTransformControllers(currentTransform);

    }

    private void SetPartRotation(Vector3 pos){

        float angle = Mathf.Atan2(currentTransform.localPosition.y - pos.y, currentTransform.localPosition.x - pos.x) * Mathf.Rad2Deg;

        if(!currentPC.flippedXAxis){
            currentTransform.localRotation = Quaternion.Euler(0f, 0f, ClampPartRotation(currentPC, angle));
        }else{
            //currentTransform.localRotation = Quaternion.Euler(0f, 0f, ClampPartRotation(currentPC, angle));
        }

        if(currentTransform.localEulerAngles.z > 180f){
            currentPC.pd.currentAngle = currentTransform.localEulerAngles.z-360f;
        }else{
            currentPC.pd.currentAngle = currentTransform.localEulerAngles.z;
        }
        
        if(currentPC.mirroredPart != null){
            currentPC.mirroredPart.UpdateAllTransformValues();
        }
        
        SetTransformControllers(currentTransform);
        
    }

    public float ClampPartRotation(PartController pc, float angle){
        return pc.pd.ClampedAngle(angle);
    }
    
    public Vector2 Rotate2D(Vector2 v, float delta) {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    public void Interpolate(float val, CharacterData gameData, CharacterData blendFrom, CharacterData blendTo){
        Debug.Log("Blending between: " + blendFrom.name + " and " + blendTo.name +". All changes are stored on: " + gameData.name);
        BlendProfile(val, gameData.headData, blendFrom.headData, blendTo.headData);
        BlendProfile(val, gameData.neckData, blendFrom.neckData, blendTo.neckData);
        BlendProfile(val, gameData.eyeData, blendFrom.eyeData, blendTo.eyeData);
        BlendProfile(val, gameData.eyebrowData, blendFrom.eyebrowData, blendTo.eyebrowData);
        BlendProfile(val, gameData.noseData, blendFrom.noseData, blendTo.noseData);
        BlendProfile(val, gameData.mouthData, blendFrom.mouthData, blendTo.mouthData);
        BlendProfile(val, gameData.earData, blendFrom.earData, blendTo.earData);
        BlendProfile(val, gameData.hairFrontData, blendFrom.hairFrontData, blendTo.hairFrontData);
        BlendProfile(val, gameData.hairBackData, blendFrom.hairBackData, blendTo.hairBackData);

        

        #if UNITY_EDITOR
        scp.UpdateAllControllers();
        #endif
    }

    public void GetCharacterDifference(CharacterData gameData, CharacterData targetData){
       // GetPartDifference(gameData.headData, targetData.headData);
       // GetPartDifference(gameData.neckData, targetData.neckData);
       // GetPartDifference(gameData.eyeData, targetData.eyeData);
        GetPartDifference(gameData.eyebrowData, targetData.eyebrowData);
       // GetPartDifference(gameData.noseData, targetData.noseData);
       // GetPartDifference(gameData.mouthData, targetData.mouthData);
       // GetPartDifference(gameData.earData, targetData.earData);
       // GetPartDifference(gameData.hairFrontData, targetData.hairFrontData);
       // GetPartDifference(gameData.hairBackData, targetData.hairBackData);
    }

    public void GetPartDifference(PartData gamePart, PartData characterPart)
    {
        string diffDebug = "";
        
        diffDebug += "The absolutePosition difference of the: " + gamePart.name + " is: " + Vector3.Dot(gamePart.absolutePosition, characterPart.absolutePosition) + ".\n";
        diffDebug += "The relativePosition difference of the: " + gamePart.name + " is: " + Vector3.Dot(gamePart.relativePosition, characterPart.relativePosition) + ".\n";
        diffDebug += "The currentAngle difference of the: " + gamePart.name + " is: " + Mathf.Abs(gamePart.currentAngle - characterPart.currentAngle) + ".\n";
        diffDebug += "The absoluteScale difference of the: " + gamePart.name + " is: " + Vector3.Dot(gamePart.absoluteScale, characterPart.absoluteScale) + ".\n";
        diffDebug += "The relativeScale difference of the: " + gamePart.name + " is: " + Vector3.Dot(gamePart.relativeScale, characterPart.relativeScale) + ".\n";
            
            for(int i = 0; i < gamePart.shaderProperties.Count; i++){
                diffDebug += "The " + gamePart.shaderProperties[i].propertyName + " difference of the " + gamePart.name + " is: " +
                 Mathf.Abs(gamePart.shaderProperties[i].propertyValue - characterPart.shaderProperties[i].propertyValue) + ".\n";
            }

            for(int j = 0; j < gamePart.shaderColors.Count; j++){
                diffDebug += "The " + gamePart.shaderColors[j].colorName + " difference of the " + gamePart.name + " is: " +
                Vector3.Dot(
                    new Vector3(gamePart.shaderColors[j].colorValue.r, gamePart.shaderColors[j].colorValue.g, gamePart.shaderColors[j].colorValue.b),
                    new Vector3(characterPart.shaderColors[j].colorValue.r, characterPart.shaderColors[j].colorValue.g, characterPart.shaderColors[j].colorValue.b)) +
                    ".\n";
            }

            Debug.Log(diffDebug);
    }

    public void BlendProfile(float val, PartData partData, PartData blendFrom, PartData blendTo){
        
        partData.absolutePosition = Vector3.Lerp(blendFrom.absolutePosition, blendTo.absolutePosition, val);
        partData.relativePosition = Vector3.Lerp(blendFrom.relativePosition, blendTo.relativePosition, val);
        partData.minPosX = Mathf.Lerp(blendFrom.minPosX, blendTo.minPosX, val);
        partData.maxPosX = Mathf.Lerp(blendFrom.maxPosX, blendTo.maxPosX, val);
        partData.minPosY = Mathf.Lerp(blendFrom.minPosY, blendTo.minPosY, val);
        partData.maxPosY = Mathf.Lerp(blendFrom.maxPosY, blendTo.maxPosY, val);
        partData.minAngle = Mathf.Lerp(blendFrom.minAngle, blendTo.minAngle, val);
        partData.maxAngle = Mathf.Lerp(blendFrom.maxAngle, blendTo.maxAngle, val);
        partData.currentAngle = Mathf.Lerp(blendFrom.currentAngle, blendTo.currentAngle, val);
        partData.absoluteScale = Vector3.Lerp(blendFrom.absoluteScale, blendTo.absoluteScale, val);
        partData.relativeScale = Vector3.Lerp(blendFrom.relativeScale, blendTo.relativeScale, val);
        partData.minScaleX = Mathf.Lerp(blendFrom.minScaleX, blendTo.minScaleX, val);
        partData.maxScaleX = Mathf.Lerp(blendFrom.maxScaleX, blendTo.maxScaleX, val);
        partData.minScaleY = Mathf.Lerp(blendFrom.minScaleY, blendTo.minScaleY, val);
        partData.maxScaleY = Mathf.Lerp(blendFrom.maxScaleY, blendTo.maxScaleY, val);

        for(int i = 0; i < partData.shaderProperties.Count; i++){
            partData.shaderProperties[i].SetValue(Mathf.Lerp(blendFrom.shaderProperties[i].propertyValue, blendTo.shaderProperties[i].propertyValue, val));
        }

        for(int j = 0; j < partData.shaderColors.Count; j++){
            partData.shaderColors[j].colorValue = Color.Lerp(blendFrom.shaderColors[j].colorValue, blendTo.shaderColors[j].colorValue, val);
        }

    }

    public void CopyRemarks(){

    }

    public void BlendCharacter(CharacterData char1, CharacterData char2, float animLength){
        StartCoroutine(Blend(char1, char2, animLength));
    }

    public IEnumerator Blend(CharacterData cd1, CharacterData cd2, float value){
        float journey = 0;
        while(journey < value){
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey/value);
            float blendPercent = blendCurve.Evaluate(percent);
            Interpolate(blendPercent, currentChar, cd1, cd2);
            yield return null;
        }

    }


    void OnValidate(){
        rightEye.UpdateSingleShaderValue("_EyelidTopOpen", Mathf.Lerp(eyelidTop, 0, blinkAmount));
        leftEye.UpdateSingleShaderValue("_EyelidTopOpen", Mathf.Lerp(eyelidTop, 0, blinkAmount));
        rightEye.UpdateSingleShaderValue("_EyelidBottomOpen", Mathf.Lerp(eyelidBottom, 0, blinkAmount));
        leftEye.UpdateSingleShaderValue("_EyelidBottomOpen", Mathf.Lerp(eyelidBottom, 0, blinkAmount));
        rightEye.UpdateRenderPropBlock();
        leftEye.UpdateRenderPropBlock();

        mouth.UpdateSingleShaderValue("_MouthLipTop", Mathf.Lerp(mouthTop, 1-mouthBottom, mouthClosedAmount/2f));
        mouth.UpdateSingleShaderValue("_MouthLipBottom", Mathf.Lerp(mouthBottom, 1-mouthTop, mouthClosedAmount/2f));
        mouth.UpdateRenderPropBlock();
    }

    void Update(){
        if(currentTransform){
            cube.position = Vector3.MoveTowards(cube.position, currentTransform.position, 2f*Time.deltaTime);
            positionDifference = currentTransform.position - cube.position;
            
            currentPC.UpdateSingleShaderVector("_PositionMomentum", positionDifference);
            currentPC.UpdateRenderPropBlock();
            if(currentPC.mirroredPart){
                currentPC.mirroredPart.UpdateSingleShaderVector("_PositionMomentum", positionDifference);
                currentPC.mirroredPart.UpdateRenderPropBlock();
            }
        }

        

        //mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
         
        mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Shader.SetGlobalVector("_MousePos", mousePos);
        //position of mouse relative to right eye position
        Vector2 rightX = new Vector2(rightEye.pd.absolutePosition.x - mousePos.x, 0);
        Vector2 rightY = new Vector2(0, rightEye.pd.absolutePosition.y - mousePos.y);
        Vector2 rotatedRight = Rotate2D(rightX, -rightEye.pd.currentAngle * Mathf.Deg2Rad);
        Vector2 rotatedRight2 = Rotate2D(rightY, -rightEye.pd.currentAngle * Mathf.Deg2Rad);

        Vector2 leftX = new Vector2(-rightEye.pd.absolutePosition.x - mousePos.x, 0);
        Vector2 leftY = new Vector2(0, rightEye.pd.absolutePosition.y - mousePos.y);
        Vector2 rotatedLeft = Rotate2D(leftX, rightEye.pd.currentAngle * Mathf.Deg2Rad);
        Vector2 rotatedLeft2 = Rotate2D(leftY, rightEye.pd.currentAngle * Mathf.Deg2Rad);

        //clampVal = rightEye.pd.shadePropertyDict["_EyeRadius"].propertyValue/6f;
        //rightPupilX = rotatedRight.x - rotatedRight2.x;
        //rightPupilY = rotatedRight2.y - rotatedRight.y;

        Vector2 rightPupilTarget = new Vector2(rotatedRight.x - rotatedRight2.x, rotatedRight2.y - rotatedRight.y);
        Vector2 leftPupilTarget = new Vector2(rotatedLeft.x - rotatedLeft2.x, rotatedLeft2.y - rotatedLeft.y);

        rightPupilX = Mathf.Lerp(rightPupilX, rightPupilTarget.x*clampVal, 4f* Time.deltaTime);
        rightPupilY = Mathf.Lerp(rightPupilY, rightPupilTarget.y*clampVal, 4f* Time.deltaTime);
        leftPupilX = Mathf.Lerp(leftPupilX, leftPupilTarget.x*clampVal, 4f* Time.deltaTime);
        leftPupilY = Mathf.Lerp(leftPupilY, leftPupilTarget.y*clampVal, 4f* Time.deltaTime);
        
        
//
        //rightPupilX = Mathf.Lerp(rightPupilX, Mathf.Clamp(rightPupilTarget.x, -clampVal, clampVal), Time.deltaTime * 3f);
        //rightPupilY = Mathf.Lerp(rightPupilY, Mathf.Clamp(rightPupilTarget.y, -clampVal, clampVal), Time.deltaTime * 3f);
        //leftPupilX = Mathf.Lerp(leftPupilX, Mathf.Clamp(leftPupilTarget.x, -clampVal, clampVal), Time.deltaTime * 3f);
        //leftPupilY = Mathf.Lerp(leftPupilY, Mathf.Clamp(leftPupilTarget.y, -clampVal, clampVal), Time.deltaTime * 3f);
       // leftEye.UpdateSingleShaderValue("_PupilOffsetX", -leftPupilX);
       // leftEye.UpdateSingleShaderValue("_PupilOffsetY", leftPupilY);
       // leftEye.UpdateAllShadersValue(0f);
        //LeftEyeProp.SetFloat("_PupilOffsetX", -leftPupilX);
        //LeftEyeProp.SetFloat("_PupilOffsetY", leftPupilY);
       // rightEye.UpdateSingleShaderValue("_PupilOffsetX", rightPupilX);
       // rightEye.UpdateSingleShaderValue("_PupilOffsetY", rightPupilY);
        //rightEye.UpdateAllShadersValue(0f);
        //RightEyeProp.SetFloat("_PupilOffsetX", rightPupilX);
       // RightEyeProp.SetFloat("_PupilOffsetY", rightPupilY);
    }
}
