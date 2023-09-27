using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using OpenCvSharp;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR;

public class FaceController : MonoBehaviour
{

    public PartController leftEye, rightEye;

    public float leftPupilX;
    public float leftPupilY;
    public float rightPupilX;
    public float rightPupilY;

    public Vector2 mousePos;
    public float clampVal;
    public PartController currentPC;
    public Transform currentTransform;

    [Range(0f, 1f)]
    public float blendCharacterValue;

    public PartTransformController widthLeft, widthRight, heightTop, heightBottom;


     void OnEnable()
	{
        OnSelectedNewFacePartEvent.Instance.AddListener(SetTransformControllers);
        OnDeselectedFacePartEvent.Instance.AddListener(DisappearControllers);
        OnTranslatePartController.Instance.AddListener(SetPartPosition);
        OnRotatePartController.Instance.AddListener(SetPartRotation);
        OnScalePartController.Instance.AddListener(SetPartScale);
    }

    void OnDisable(){
        OnSelectedNewFacePartEvent.Instance.RemoveListener(SetTransformControllers);
        OnDeselectedFacePartEvent.Instance.RemoveListener(DisappearControllers);
        OnTranslatePartController.Instance.RemoveListener(SetPartPosition);
        OnRotatePartController.Instance.RemoveListener(SetPartRotation);
        OnScalePartController.Instance.RemoveListener(SetPartScale);
    }

    private void DisappearControllers(){
        widthLeft.transform.localPosition = new Vector3(100, 100, 100);
        widthRight.transform.localPosition = new Vector3(100, 100, 100);
        heightTop.transform.localPosition = new Vector3(100, 100, 100);
    }

    private void SetTransformControllers(Transform selectedTarget){
        
        currentPC = selectedTarget.GetComponent<PartController>();
        currentTransform = selectedTarget;
        DisappearControllers();

        if(currentPC.pd.translatable)
            widthLeft.transform.localPosition = selectedTarget.TransformPoint(new Vector3(0, 0, 0));

        widthLeft.transform.localPosition = new Vector3(widthLeft.transform.localPosition.x, widthLeft.transform.localPosition.y, -1f);

        if(currentPC.pd.rotatable)
            widthRight.transform.localPosition = selectedTarget.TransformPoint(new Vector3(-0.5f, 0, 0));

        if(currentPC.pd.scalable)
            heightTop.transform.localPosition = selectedTarget.TransformPoint(new Vector3(0.5f, 0.5f, 0));
        
        heightTop.transform.localPosition = new Vector3(heightTop.transform.localPosition.x, heightTop.transform.localPosition.y, -1f);
    
    }

    private void SetPartPosition(Vector3 pos){
        //each part has a relative position to other objects
        float flip = 1;

        if(currentPC.flippedXAxis)
            flip = -1;

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
        
        currentTransform.localRotation = Quaternion.Euler(0f, 0f, ClampPartRotation(currentPC, angle));
        currentPC.pd.currentAngle = currentTransform.localEulerAngles.z;
        
        if(currentPC.mirroredPart != null){
            currentPC.mirroredPart.UpdateAllTransformValues();
        }
        SetTransformControllers(currentTransform);
        
    }


    public float ClampPartRotation(PartController pc, float angle){
        return pc.pd.ClampedAngle(angle);
    }
/*
    public void SetTransformValues(){

        float actualHeadWidth = Mathf.Lerp(1,4, headWidth);
        float actualHeadLength = Mathf.Lerp(2,4, headLength);
        //Head.transform.localScale = new Vector3(actualHeadWidth, actualHeadLength, 0);

        float actualEyeHeight = eyeHeight * Mathf.Lerp(1, 2, headLength) * Mathf.Lerp(2/chinScale, 2/foreheadScale, eyeHeight*.5f+0.5f);
        
        LeftEye.transform.localPosition = new Vector3(Mathf.Lerp(-eyeWidth/2f, -actualHeadWidth/2f + eyeWidth/2f, headWidth * eyeSpacing), actualEyeHeight, 0);
        RightEye.transform.localPosition = new Vector3(Mathf.Lerp(eyeWidth/2f, actualHeadWidth/2f - eyeWidth/2f, headWidth * eyeSpacing), actualEyeHeight, 0);
        LeftEye.transform.localScale = new Vector3(-eyeWidth, eyeLength, 1);
        RightEye.transform.localScale = new Vector3(eyeWidth, eyeLength, 1);
        LeftEye.transform.localEulerAngles = new Vector3(0, 0, eyeAngle);
        RightEye.transform.localEulerAngles = new Vector3(0, 0, -eyeAngle);

        float actualEyebrowHeight = Mathf.Lerp(actualEyeHeight, Mathf.Lerp(1, 2, headLength) * 2/foreheadScale, eyebrowHeight);
        
        LeftEyebrow.transform.localPosition = new Vector3(Mathf.Lerp(-eyebrowLength/2, -actualHeadWidth/2f, headWidth* eyebrowSpacing), actualEyebrowHeight, -0.15f);
        RightEyebrow.transform.localPosition = new Vector3(Mathf.Lerp(eyebrowLength/2, actualHeadWidth/2f, headWidth* eyebrowSpacing), actualEyebrowHeight, -0.15f);
        LeftEyebrow.transform.localScale = new Vector3(-eyebrowLength, eyebrowWidth, 1);
        RightEyebrow.transform.localScale = new Vector3(eyebrowLength, eyebrowWidth, 1);
        LeftEyebrow.transform.localEulerAngles = new Vector3(0, 0, eyebrowAngle);
        RightEyebrow.transform.localEulerAngles = new Vector3(0, 0, -eyebrowAngle);
    
        float actualNoseHeight = Mathf.Lerp(actualEyeHeight, -actualHeadLength/2f * (2f/chinScale), noseHeight);
       // Nose.transform.localPosition = new Vector3(0, actualNoseHeight, -0.05f);
       // Nose.transform.localScale = new Vector3(noseWidth, noseLength, 1);

        LeftEar.transform.localScale = new Vector3(-earWidth,earLength,1);
        RightEar.transform.localScale = new Vector3(earWidth,earLength,1);

        LeftEar.transform.localEulerAngles = new Vector3(0, 0, earAngle);
        RightEar.transform.localEulerAngles = new Vector3(0, 0, -earAngle);
        float actualEarHeight = earHeight * Mathf.Lerp(1, 2, headLength) * Mathf.Lerp(2/chinScale, 2/foreheadScale, earHeight*.5f+0.5f);
        LeftEar.transform.localPosition = new Vector3(Mathf.Lerp(-earWidth/2f, -actualHeadWidth/2f - earWidth/2f, headWidth * earSpacing) + Mathf.Abs(earAngle)/90f, earHeight * headLength - earAngle/90f, 0.15f);
        RightEar.transform.localPosition = new Vector3(Mathf.Lerp(earWidth/2f, actualHeadWidth/2f + earWidth/2f, headWidth * earSpacing) - Mathf.Abs(earAngle)/90f, earHeight * headLength - earAngle/90f, 0.15f);

        MouthR.transform.localScale = new Vector3(mouthWidth, mouthLength, 1);
        MouthR.transform.localPosition = new Vector3(0, Mathf.Lerp(actualNoseHeight - (mouthLength/4f), (-actualHeadLength/2 * (2f/chinScale)) + (mouthLength/4f), mouthHeight), 0.05f);

        Neck.transform.localScale = new Vector3(Mathf.Lerp(0.5f, actualHeadWidth, neckWidth), 2f, 1f);

        HairFront.transform.localPosition = new Vector3(0, Mathf.Lerp(1, 2, headLength) * (2/foreheadScale) - bangLength + 0.05f, -0.1f);
        HairFront.transform.localScale = new Vector3(Mathf.Lerp(0, actualHeadWidth, bangWidth), bangLength*2, 1);

        HairBack.transform.localPosition = new Vector3(0, Mathf.Lerp(0, Mathf.Lerp(1, 2, headLength) * (2/foreheadScale) - hairLength/2, hairHeight), 0.3f);
        HairBack.transform.localScale = new Vector3(Mathf.Lerp(0, actualHeadWidth * 1.25f, hairWidth), hairLength, 1);
    }*/
    
    public Vector2 Rotate2D(Vector2 v, float delta) {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    void Update(){

        mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
       
        //position of mouse relative to right eye position
        Vector2 rightX = new Vector2(rightEye.pd.absolutePosition.x - mousePos.x, 0);
        Vector2 rightY = new Vector2(0, rightEye.pd.absolutePosition.y - mousePos.y);
        Vector2 rotatedRight = Rotate2D(rightX, -rightEye.pd.currentAngle * Mathf.Deg2Rad);
        Vector2 rotatedRight2 = Rotate2D(rightY, -rightEye.pd.currentAngle * Mathf.Deg2Rad);

        Vector2 leftX = new Vector2(-rightEye.pd.absolutePosition.x - mousePos.x, 0);
        Vector2 leftY = new Vector2(0, rightEye.pd.absolutePosition.y - mousePos.y);
        Vector2 rotatedLeft = Rotate2D(leftX, rightEye.pd.currentAngle * Mathf.Deg2Rad);
        Vector2 rotatedLeft2 = Rotate2D(leftY, rightEye.pd.currentAngle * Mathf.Deg2Rad);

        clampVal = rightEye.pd.shadePropertyDict["_EyeRadius"].propertyValue/6f;
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
