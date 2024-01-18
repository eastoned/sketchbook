using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using OpenCvSharp;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.U2D.IK;

public class FaceController : MonoBehaviour
{

    public PartController leftEye, rightEye, mouth, nose, head, leftEyebrow, rightEyebrow, bangs;
    public Transform[] bodyParts;

    [Range(-90, 90)]
    public float rotation;

    public float leftPupilX;
    public float leftPupilY;
    public float rightPupilX;
    public float rightPupilY;

    public Vector2 mousePos;
    public float clampVal;
    public Transform hoveredTransform;
    public PartController currentPC;
    public Transform currentTransform;
    
    public SaveCharacterProfile scp;

    public CharacterData currentChar;
    public AnimationCurve blendCurve; 
    public AnimationCurve blinkCurve;

    public bool currentlyBlending = false;

    private Coroutine blending;

    [Range(0f, 1f)]
    public float blinkAmount;
    private float eyelidTop, eyelidBottom;
    [Range(0f, 1f)]
    public float mouthClosedAmount;
    private float mouthTop, mouthBottom;

    public Transform cube;
    public Vector3 positionDifference;

    public PartTransformController widthRight, heightTop;

    public PartController currentHovered;
    [SerializeField] private Material colliderMaterial;

    public float currentChange = 0f;

     void OnEnable()
	{
        OnHoveredNewFacePartEvent.Instance.AddListener(SetMaterialOutline);
        OnSelectedNewFacePartEvent.Instance.AddListener(SetTransformControllers);
        OnDeselectedFacePartEvent.Instance.AddListener(ClearCurrentHover);
        OnTranslatePartController.Instance.AddListener(SetPartPosition);
        OnRotatePartController.Instance.AddListener(SetPartRotation);
        OnScalePartController.Instance.AddListener(SetPartScale);
        OnSetKeyFrameData.Instance.AddListener(SetDefaultBlinkAndMouthPos);
        OnConfirmTransformPart.Instance.AddListener(UpdateMoneyAmount);
    }

    void OnDisable(){
        OnHoveredNewFacePartEvent.Instance.RemoveListener(SetMaterialOutline);
        OnSelectedNewFacePartEvent.Instance.RemoveListener(SetTransformControllers);
        OnDeselectedFacePartEvent.Instance.RemoveListener(ClearCurrentHover);
        OnTranslatePartController.Instance.RemoveListener(SetPartPosition);
        OnRotatePartController.Instance.RemoveListener(SetPartRotation);
        OnScalePartController.Instance.RemoveListener(SetPartScale);
        OnSetKeyFrameData.Instance.RemoveListener(SetDefaultBlinkAndMouthPos);
        OnConfirmTransformPart.Instance.AddListener(UpdateMoneyAmount);
    }

    private void UpdateMoneyAmount(){
        NuFaceManager.money -= currentChange;
        currentChange = 0f;
    }

    public void SetMaterialOutline(Transform hoveredTarget){
        ClearCurrentHover();
        hoveredTarget.GetComponent<Renderer>().sharedMaterials = new Material[2]{hoveredTarget.GetComponent<Renderer>().sharedMaterials[0], colliderMaterial};
        hoveredTransform = hoveredTarget;
    }

    public void SetDefaultBlinkAndMouthPos(){
        Debug.Log("Set new eye and mouth vars");
        eyelidTop = rightEye.pd.shadePropertyDict["_EyelidTopOpen"].propertyValue;
        eyelidBottom = rightEye.pd.shadePropertyDict["_EyelidBottomOpen"].propertyValue;
        mouthTop = mouth.pd.shadePropertyDict["_MouthLipTop"].propertyValue;
        mouthBottom = mouth.pd.shadePropertyDict["_MouthLipBottom"].propertyValue;
    }

    private void ClearCurrentHover(){
        if(hoveredTransform != null){
            //Debug.Log(hoveredTransform.name);
            hoveredTransform.GetComponent<Renderer>().sharedMaterials = new Material[1]{hoveredTransform.GetComponent<Renderer>().sharedMaterials[0]};
        }
    }

    private void DisappearControllers(){
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

        if(currentPC.rotatable)
            widthRight.transform.localPosition = selectedTarget.TransformPoint(new Vector3(-0.4f, 0, 0));
        
        widthRight.transform.localPosition = new Vector3(widthRight.transform.localPosition.x, widthRight.transform.localPosition.y, -1f);

        if(currentPC.scalable)
            heightTop.transform.localPosition = selectedTarget.TransformPoint(new Vector3(0.4f, 0.4f, 0));
        
        heightTop.transform.localPosition = new Vector3(heightTop.transform.localPosition.x, heightTop.transform.localPosition.y, -1f);
    
    }

    private void SetPartPosition(Vector3 pos){
        //each part has a relative position to other objects
        float flip = currentPC.flippedXAxis? -1f : 1f;
        //Debug.Log(currentPC.pd.GetAbsolutePosition() - pos);
        
        currentTransform.localPosition = new Vector3(pos.x, pos.y, currentTransform.localPosition.z);
        Vector3 absPos = new Vector3(pos.x*flip, pos.y, currentTransform.localPosition.z);

        currentChange = Vector3.Distance(currentPC.pd.ReturnClampedPosition(pos), currentPC.pd.GetAbsolutePosition());
        

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

        if(currentPC.flippedXAxis){
            flip = -1;
        }
        
        Vector3 diff = currentTransform.InverseTransformDirection(currentTransform.localPosition - pos)*2f;
        diff = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), 1);

        currentChange = Vector3.Distance(diff, currentPC.pd.GetAbsoluteScale());

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

        if(angle < currentPC.pd.currentAngle){
            //Debug.Log("The new angle is less than the current angle");
        }else if (angle > currentPC.pd.currentAngle){
          //  Debug.Log("The new angle is greater than the current angle");
        }

        currentChange = Mathf.Abs(angle - currentPC.pd.currentAngle);
        //Debug.Log("The current angle diff: " + currentChange);

        currentTransform.localRotation = Quaternion.Euler(0f, 0f, currentPC.pd.ClampedAngle(angle, currentPC.flippedXAxis));
        
        if(currentPC.mirroredPart != null){
            currentPC.mirroredPart.UpdateAllTransformValues();
        }
        
        SetTransformControllers(currentTransform);
        
    }
    
    public Vector2 Rotate2D(Vector2 v, float delta) {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    public void Interpolate(float val, CharacterData gameData, CharacterData blendFrom, CharacterData blendTo){
        BlendProfile(val, gameData.headData, blendFrom.headData, blendTo.headData);
        BlendProfile(val, gameData.neckData, blendFrom.neckData, blendTo.neckData);
        BlendProfile(val, gameData.eyeData, blendFrom.eyeData, blendTo.eyeData);
        BlendProfile(val, gameData.eyebrowData, blendFrom.eyebrowData, blendTo.eyebrowData);
        BlendProfile(val, gameData.noseData, blendFrom.noseData, blendTo.noseData);
        BlendProfile(val, gameData.mouthData, blendFrom.mouthData, blendTo.mouthData);
        BlendProfile(val, gameData.earData, blendFrom.earData, blendTo.earData);
        BlendProfile(val, gameData.hairFrontData, blendFrom.hairFrontData, blendTo.hairFrontData);
        BlendProfile(val, gameData.hairBackData, blendFrom.hairBackData, blendTo.hairBackData);

        scp.UpdateAllControllers();
    }

    public float GetCharacterDifference(CharacterData gameData, CharacterData targetData){
        float score = 0;
        score +=GetPartDifference(gameData.headData, targetData.headData);
        score +=GetPartDifference(gameData.neckData, targetData.neckData);
        score +=GetPartDifference(gameData.eyeData, targetData.eyeData);
        score += GetPartDifference(gameData.eyebrowData, targetData.eyebrowData);
        score +=GetPartDifference(gameData.noseData, targetData.noseData);
        score +=GetPartDifference(gameData.mouthData, targetData.mouthData);
        score +=GetPartDifference(gameData.earData, targetData.earData);
        score +=GetPartDifference(gameData.hairFrontData, targetData.hairFrontData);
        score +=GetPartDifference(gameData.hairBackData, targetData.hairBackData);
       Debug.Log("Similarity score between current face and : " + targetData.name + " is : " + score);
       return score;
    }

    public float GetPartDifference(PartData gamePart, PartData characterPart)
    {
        string diffDebug = "";
        float score = 0;
        score += Vector3.Dot(gamePart.absolutePosition.normalized, characterPart.absolutePosition.normalized);
        
        diffDebug += "The absolutePosition difference of the: " + gamePart.name + " is: " + Vector3.Dot(gamePart.absolutePosition.normalized, characterPart.absolutePosition.normalized) + ".\n";
        //diffDebug += "The relativePosition difference of the: " + gamePart.name + " is: " + Vector3.Dot(gamePart.relativePosition.normalized, characterPart.relativePosition.normalized) + ".\n";
        score += Vector3.Dot(gamePart.absoluteScale.normalized, characterPart.absoluteScale.normalized);
        diffDebug += "The currentAngle difference of the: " + gamePart.name + " is: " + Mathf.Abs(gamePart.currentAngle - characterPart.currentAngle) + ".\n";
        score -= Mathf.Abs(gamePart.currentAngle - characterPart.currentAngle)/180f;
        diffDebug += "The absoluteScale difference of the: " + gamePart.name + " is: " + Vector3.Dot(gamePart.absoluteScale.normalized, characterPart.absoluteScale.normalized) + ".\n";
        //diffDebug += "The relativeScale difference of the: " + gamePart.name + " is: " + Vector3.Dot(gamePart.relativeScale.normalized, characterPart.relativeScale.normalized) + ".\n";
            
            for(int i = 0; i < gamePart.shaderProperties.Count; i++){
                diffDebug += "The " + gamePart.shaderProperties[i].propertyName + " difference of the " + gamePart.name + " is: " +
                 Mathf.Abs(gamePart.shaderProperties[i].propertyValue - characterPart.shaderProperties[i].propertyValue) + ".\n";
                 score -= Mathf.Abs(gamePart.shaderProperties[i].propertyValue - characterPart.shaderProperties[i].propertyValue)/2f;
            }
        diffDebug = "";
            for(int j = 0; j < gamePart.shaderColors.Count; j++){
                diffDebug += "The " + gamePart.shaderColors[j].colorName + " difference of the " + gamePart.name + " is: " +
                Vector3.Dot(
                    new Vector3(gamePart.shaderColors[j].GetValue(), gamePart.shaderColors[j].GetHue(), gamePart.shaderColors[j].GetSaturation()).normalized,
                    new Vector3(characterPart.shaderColors[j].GetValue(), characterPart.shaderColors[j].GetHue(), characterPart.shaderColors[j].GetSaturation()).normalized) +
                    ".\n";
                Vector3 currentColor = new Vector3(gamePart.shaderColors[j].GetValue(), gamePart.shaderColors[j].GetHue(), gamePart.shaderColors[j].GetSaturation()).normalized;
                Vector3 charColor = new Vector3(characterPart.shaderColors[j].GetValue(), characterPart.shaderColors[j].GetHue(), characterPart.shaderColors[j].GetSaturation()).normalized;

                if (currentColor.magnitude == 0f || charColor.magnitude == 0f){
                    float addedScore = 1f;
                    float differ = Vector3.Distance(currentColor.normalized, charColor.normalized);
                    addedScore -= differ;
                    Debug.Log("colors are not the same but one is zero so subtracting the value of the > 0 vector");
                    if(currentColor == charColor){
                        Debug.Log("current color is the exact same as the template color");
                        //score += 1f;
                    }
                    //score += addedScore;
                }else{
                    //score += Vector3.Dot(currentColor, charColor);
                }
                //score += Vector3.Dot(
                //    new Vector3(gamePart.shaderColors[j].GetValue(), gamePart.shaderColors[j].GetHue(), gamePart.shaderColors[j].GetSaturation()).normalized,
                //    new Vector3(characterPart.shaderColors[j].GetValue(), characterPart.shaderColors[j].GetHue(), characterPart.shaderColors[j].GetSaturation()).normalized);
            }

        Debug.Log(diffDebug);
        return score;
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

    public void BlendCharacter(CharacterData char1, CharacterData char2, float animLength){

        if(blending != null){
            StopCoroutine(blending);
        }

        blending = StartCoroutine(Blend(char1, char2, animLength));
        
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
            
            yield return null;
        }
        scp.Morph(cd2);
        currentChar.CopyData(cd2);
        if(cd1.writeable){
            cd1.CopyData(cd2);
        }
        currentlyBlending = false;
        Debug.Log("copied data from blend target to writeable character");
    }

    public IEnumerator BlendPart(CharacterData char1, CharacterData char2, int partID, float animLength){
        float journey = 0;
        currentlyBlending = true;
        while(journey < animLength){
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey/animLength);
            float blendPercent = blendCurve.Evaluate(percent);
            BlendProfile(blendPercent, currentChar.allParts[partID], char1.allParts[partID], char2.allParts[partID]);
            scp.UpdateAllControllers();
            yield return null;
        }
        
        if(char1.writeable){
            char1.allParts[partID].CopyData(char2.allParts[partID]);
        }
        currentlyBlending = false;
        Debug.Log("copied " + currentChar.allParts[partID].name + " data from blend target to writeable character");
    }

    void Rotation(){
        /*
        nose.transform.position = new Vector3(Mathf.Lerp(-head.pd.GetAbsoluteScale().x/2f, head.pd.GetAbsoluteScale().x/2f, rotation/180f + 0.5f), nose.transform.position.y, nose.transform.position.z);
        mouth.transform.position = new Vector3(Mathf.Lerp(-head.pd.GetAbsoluteScale().x/4f, head.pd.GetAbsoluteScale().x/4f, rotation/180f + 0.5f), mouth.transform.position.y, mouth.transform.position.z);
        leftEye.transform.position = new Vector3(Mathf.Lerp(leftEye.pd.GetFlippedAbsolutePosition().x*2, 0, rotation/180f + 0.5f), leftEye.transform.position.y, leftEye.transform.position.z);
        rightEye.transform.position = new Vector3(Mathf.Lerp(0, rightEye.pd.GetAbsolutePosition().x*2, rotation/180f + 0.5f), rightEye.transform.position.y, rightEye.transform.position.z);
        leftEyebrow.transform.position = new Vector3(Mathf.Lerp(leftEyebrow.pd.GetFlippedAbsolutePosition().x*2, 0, rotation/180f + 0.5f), leftEyebrow.transform.position.y, leftEyebrow.transform.position.z);
        rightEyebrow.transform.position = new Vector3(Mathf.Lerp(0, rightEyebrow.pd.GetAbsolutePosition().x*2, rotation/180f + 0.5f), rightEyebrow.transform.position.y, rightEyebrow.transform.position.z);
        bangs.transform.position = new Vector3(Mathf.Lerp(-head.pd.GetAbsoluteScale().x/4f, head.pd.GetAbsoluteScale().x/4f, rotation/180f + 0.5f), bangs.transform.position.y, bangs.transform.position.z);
        */
        foreach(Transform part in bodyParts){
            part.parent = this.transform;
        }
        
        transform.localEulerAngles = new Vector3(0, 0, rotation);

        foreach(Transform part in bodyParts){
            part.parent = null;
        }

    }

    private IEnumerator Blink(float blinkLength, float eyelidTopOpen, float eyelidBottomOpen){
        float animationTime = 0;
        currentlyBlending = true;
        while(animationTime < blinkLength){
            float interval = Mathf.Clamp01(animationTime/blinkLength);
            interval = blinkCurve.Evaluate(interval);
            rightEye.UpdateSingleShaderValue("_EyelidTopOpen", Mathf.Lerp(eyelidTopOpen, 0, interval));
            leftEye.UpdateSingleShaderValue("_EyelidTopOpen", Mathf.Lerp(eyelidTopOpen, 0, interval));
            rightEye.UpdateSingleShaderValue("_EyelidBottomOpen", Mathf.Lerp(eyelidBottomOpen, 0, interval));
            leftEye.UpdateSingleShaderValue("_EyelidBottomOpen", Mathf.Lerp(eyelidBottomOpen, 0, interval));
            rightEye.UpdateRenderPropBlock();
            leftEye.UpdateRenderPropBlock();
            animationTime += Time.deltaTime;
            yield return null;
        }
        currentlyBlending = false;
    }

    void Update(){
        //Rotation();
        if(currentTransform){
            cube.position = Vector3.MoveTowards(cube.position, currentTransform.position, 2f*Time.deltaTime);
            positionDifference = currentTransform.position - cube.position;
            //sourceaud.pitch = positionDifference.magnitude*2f;
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

        //rightPupilX = Mathf.Lerp(rightPupilX, rightPupilTarget.x*clampVal, 4f* Time.deltaTime);
        //rightPupilY = Mathf.Lerp(rightPupilY, rightPupilTarget.y*clampVal, 4f* Time.deltaTime);
        //leftPupilX = Mathf.Lerp(leftPupilX, leftPupilTarget.x*clampVal, 4f* Time.deltaTime);
        //leftPupilY = Mathf.Lerp(leftPupilY, leftPupilTarget.y*clampVal, 4f* Time.deltaTime);
        
        
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
        rightEye.UpdateSingleShaderValue("_PupilOffsetX", rightPupilX);
        rightEye.UpdateSingleShaderValue("_PupilOffsetY", rightPupilY);
        rightEye.UpdateRenderPropBlock();
        //rightEye.UpdateAllShadersValue(0f);
        //RightEyeProp.SetFloat("_PupilOffsetX", rightPupilX);
        //RightEyeProp.SetFloat("_PupilOffsetY", rightPupilY);
    }
}
