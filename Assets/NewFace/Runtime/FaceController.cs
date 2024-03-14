using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FaceController : MonoBehaviour
{

    public PartController leftEye, rightEye, mouth, nose, head, leftEyebrow, rightEyebrow, bangs, hair, neck, leftEar, rightEar;
    public Transform[] bodyParts;
    public PartData[] bodyData;

    [Range(-90, 90)]
    public float rotation;

    public float leftPupilX;
    public float leftPupilY;
    public float rightPupilX;
    public float rightPupilY;

    public Vector2 mousePos;
    public float clampVal;

    public CharacterData currentChar;
    public AnimationCurve blendCurve; 
    public AnimationCurve blinkCurve;
    public AnimationCurve scalePopCurve;

    public bool currentlyBlending = false;

    private Coroutine blending;

    [Range(0f, 1f)]
    public float blinkAmount;
    private float eyelidTop, eyelidBottom;
    [Range(0f, 1f)]
    public float mouthClosedAmount;
    private float mouthTop, mouthBottom;

    public bool animating = false;

    public virtual void OnEnable()
	{
        OnSelectedNewFacePartEvent.Instance.AddListener(Blink);
    }
    public virtual void OnDisable(){
        OnSelectedNewFacePartEvent.Instance.RemoveListener(Blink);
    }

    private void Blink(Transform ignore){
        //Debug.Log("Blink");
        if (faceAnim != null){
            StopCoroutine(faceAnim);
        }
        //Debug.Log(gameObject.name + " eye top: " + rightEye.pd.shadePropertyDict["_EyelidTopOpen"].propertyValue);
        faceAnim = StartCoroutine(Blink(Random.Range(.2f, .5f), rightEye.pd.shadePropertyDict["_EyelidTopOpen"].propertyValue, rightEye.pd.shadePropertyDict["_EyelidBottomOpen"].propertyValue));
    }

    private void Blink(float blinkLength, Transform ignore){
        //Debug.Log("Blink");
        if (faceAnim != null){
            StopCoroutine(faceAnim);
        }
        //Debug.Log(gameObject.name + " eye top: " + rightEye.pd.shadePropertyDict["_EyelidTopOpen"].propertyValue);
        faceAnim = StartCoroutine(Blink(blinkLength, rightEye.pd.shadePropertyDict["_EyelidTopOpen"].propertyValue, rightEye.pd.shadePropertyDict["_EyelidBottomOpen"].propertyValue));
    }

    private void BlinkMouth(float blinkMouthLength){
        if (faceAnim != null){
            StopCoroutine(faceAnim);
        }
        //Debug.Log(gameObject.name + " mouth: " + mouth.pd.shadePropertyDict["_MouthOpen"].propertyValue);
        faceAnim = StartCoroutine(BlinkMouth(blinkMouthLength, mouth.pd.shadePropertyDict["_MouthOpen"].propertyValue));
    }

    private void Start(){
        InitializeControllers();
    }

    private void InitializeControllers(){
        InitializeDictionaries();
        UpdateAllControllers();
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
    }

    public void UpdateAllControllers(){
        head.UpdateAllTransformValues();
        head.UpdateAllShadersValue(0f);
        rightEye.UpdateAllTransformValues();
        rightEye.UpdateAllShadersValue(0f);
        leftEye.UpdateAllShadersValue(0f);
        rightEyebrow.UpdateAllTransformValues();
        rightEyebrow.UpdateAllShadersValue(0f);
        leftEyebrow.UpdateAllShadersValue(0f);
        rightEar.UpdateAllTransformValues();
        rightEar.UpdateAllShadersValue(0f);
        leftEar.UpdateAllShadersValue(0f);
        bangs.UpdateAllTransformValues();
        bangs.UpdateAllShadersValue(0f);
        hair.UpdateAllTransformValues();
        hair.UpdateAllShadersValue(0f);
        mouth.UpdateAllTransformValues();
        mouth.UpdateAllShadersValue(0f);
        neck.UpdateAllTransformValues();
        neck.UpdateAllShadersValue(0f);
        nose.UpdateAllTransformValues();
        nose.UpdateAllShadersValue(0f);
    }

    public void InitializeDictionaries(){
        head.InitializePartDataDictionary();
        rightEye.InitializePartDataDictionary();
        rightEyebrow.InitializePartDataDictionary();
        rightEar.InitializePartDataDictionary();
        bangs.InitializePartDataDictionary();
        hair.InitializePartDataDictionary();
        mouth.InitializePartDataDictionary();
        neck.InitializePartDataDictionary();
        nose.InitializePartDataDictionary();
    }

    public void BlendProfile(float val, PartData partData, PartData blendFrom, PartData blendTo){
        
        partData.absolutePosition = Vector3.Lerp(blendFrom.absolutePosition, blendTo.absolutePosition, val);
        partData.relativePosition = Vector3.Lerp(blendFrom.relativePosition, blendTo.relativePosition, val);
        partData.currentAngle = Mathf.Lerp(blendFrom.currentAngle, blendTo.currentAngle, val);
        partData.absoluteScale = Vector3.Lerp(blendFrom.absoluteScale, blendTo.absoluteScale, val);
        partData.relativeScale = Vector3.Lerp(blendFrom.relativeScale, blendTo.relativeScale, val);

        for(int i = 0; i < partData.shaderProperties.Count; i++){
            partData.shaderProperties[i].SetValue(Mathf.Lerp(blendFrom.shaderProperties[i].propertyValue, blendTo.shaderProperties[i].propertyValue, val));
        }

        for(int j = 0; j < partData.shaderColors.Count; j++){
            partData.shaderColors[j].colorValue = Color.Lerp(blendFrom.shaderColors[j].colorValue, blendTo.shaderColors[j].colorValue, val);
        }

    }

    public void BlendCharacter(CharacterData char1, CharacterData char2, float animLength){

        if (faceAnim != null){
            StopCoroutine(faceAnim);
        }

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

    void Rotation(){
        nose.transform.position = new Vector3(Mathf.Lerp(-head.pd.GetAbsoluteScale().x/2f, head.pd.GetAbsoluteScale().x/2f, rotation/180f + 0.5f), nose.transform.position.y, nose.transform.position.z);
        mouth.transform.position = new Vector3(Mathf.Lerp(-head.pd.GetAbsoluteScale().x/4f, head.pd.GetAbsoluteScale().x/4f, rotation/180f + 0.5f), mouth.transform.position.y, mouth.transform.position.z);
        leftEye.transform.position = new Vector3(Mathf.Lerp(leftEye.pd.GetFlippedAbsolutePosition().x * 2, 0, rotation/180f + 0.5f), leftEye.transform.position.y, leftEye.transform.position.z);
        rightEye.transform.position = new Vector3(Mathf.Lerp(0, rightEye.pd.GetAbsolutePosition().x*2, rotation/180f + 0.5f), rightEye.transform.position.y, rightEye.transform.position.z);
        leftEyebrow.transform.position = new Vector3(Mathf.Lerp(leftEyebrow.pd.GetFlippedAbsolutePosition().x*2, 0, rotation/180f + 0.5f), leftEyebrow.transform.position.y, leftEyebrow.transform.position.z);
        rightEyebrow.transform.position = new Vector3(Mathf.Lerp(0, rightEyebrow.pd.GetAbsolutePosition().x*2, rotation/180f + 0.5f), rightEyebrow.transform.position.y, rightEyebrow.transform.position.z);
        bangs.transform.position = new Vector3(Mathf.Lerp(-head.pd.GetAbsoluteScale().x/4f, head.pd.GetAbsoluteScale().x/4f, rotation/180f + 0.5f), bangs.transform.position.y, bangs.transform.position.z);
    }

    private IEnumerator BlinkMouth(float blinkMouthLength, float mouthOpen){
        float animationTime = 0;
        while(animationTime < blinkMouthLength){
            float interval = Mathf.Clamp01(animationTime/blinkMouthLength);
            interval = blinkCurve.Evaluate(interval);
            mouth.UpdateSingleShaderValue("_MouthOpen", Mathf.Lerp(mouthOpen, 0, interval));
            mouth.UpdateRenderPropBlock();
            animationTime += Time.deltaTime;
            yield return null;
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
    float timer = 0;
    private Coroutine faceAnim;
    void Update(){
        Rotation();

        if(animating){
            timer += Time.deltaTime;

        
            if(timer >= 9f){
                if(Random.Range(0f, 1f) < 0.5f) {
                    Blink(Random.Range(.1f, 2f), transform);
                }else{
                    BlinkMouth(Random.Range(.1f, 2f));
                }
                
                timer = Random.Range(0f, 4f);
            }
        }
        //mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
         
        mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Shader.SetGlobalVector("_MousePos", mousePos);
        //position of mouse relative to right eye position
        float rightX = rightEye.pd.GetAbsolutePosition().x - (mousePos.x - transform.position.x);

        float rightY = rightEye.pd.GetAbsolutePosition().y - (mousePos.y - transform.position.y);
        
        //Vector2 rotatedRight = Rotate2D(rightX, -rightEye.pd.currentAngle * Mathf.Deg2Rad);
        //Vector2 rotatedRight2 = Rotate2D(rightY, -rightEye.pd.currentAngle * Mathf.Deg2Rad);

        float leftX = (mousePos.x - transform.position.x) - rightEye.pd.GetFlippedAbsolutePosition().x;
        float leftY = rightEye.pd.GetAbsolutePosition().y - (mousePos.y - transform.position.y);

        rightX = Mathf.Clamp(rightX/5f, -.5f, .5f);
        leftX = Mathf.Clamp(leftX/5f, -.5f, .5f);
        rightY = Mathf.Clamp(rightY/5f, -.25f, .25f);
        leftY = Mathf.Clamp(leftY/5f, -.25f, .25f);

        float rotatedRightX = (rightX*Mathf.Cos(-rightEye.pd.currentAngle)) - (rightY*Mathf.Sin(-rightEye.pd.currentAngle));
        float rotatedRightY = (rightX*Mathf.Sin(-rightEye.pd.currentAngle)) + (rightY*Mathf.Cos(-rightEye.pd.currentAngle));
        
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
        rightEye.UpdateSingleShaderValue("_PupilOffsetX", rightX);
        rightEye.UpdateSingleShaderValue("_PupilOffsetY", rightY);
        rightEye.UpdateRenderPropBlock();
        leftEye.UpdateSingleShaderValue("_PupilOffsetX", leftX);
        leftEye.UpdateSingleShaderValue("_PupilOffsetY", leftY);
        leftEye.UpdateRenderPropBlock();
        //rightEye.UpdateAllShadersValue(0f);
        //RightEyeProp.SetFloat("_PupilOffsetX", rightPupilX);
        //RightEyeProp.SetFloat("_PupilOffsetY", rightPupilY);
    }
}
