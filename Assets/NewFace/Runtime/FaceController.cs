using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using OpenCvSharp;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR;

public class FaceController : MonoBehaviour
{
    [HideInInspector][SerializeField] public Renderer LeftEye,
    RightEye, LeftEyebrow, RightEyebrow, LeftEar, RightEar,
    MouthR, Neck, HairFront, HairBack;

    public PartData head, eye, nose, ear, mouth;

    #region Transform Variables

    [Range(0f, 1f)] public float headWidth, headLength;
        
    [Range(0f, 1f)] public float neckWidth;

    [Range(-45f, 25)] public float eyeAngle;
    [Range(0f, 1f)] public float eyeSpacing;
    [Range(-0.75f, 1f)] public float eyeHeight;
    [Range(0.5f, 1f)] public float eyeLength, eyeWidth;

    [Range(-45f, 25f)] public float eyebrowAngle;
    [Range(0f, 1f)] public float eyebrowSpacing;
    [Range(0f, 1f)] public float eyebrowHeight;
    [Range(0.2f, 1.5f)] public float eyebrowLength;
    [Range(0.1f, 0.5f)] public float eyebrowWidth;

    [Range(0f, 1f)] public float noseHeight;
    [Range(0.5f, 2f)] public float noseLength, noseWidth;

    [Range(0.25f, 2.5f)] public float mouthWidth;
    [Range(0.2f, 1f)] public float mouthLength;
    [Range(0f, 1f)] public float mouthHeight;
        
    [Range(.5f, 2f)] public float earWidth, earLength;
    [Range(-0.5f, 1f)] public float earHeight;
    [Range(-45f, 45f)] public float earAngle;
    [Range(0f, 1f)] public float earSpacing;

    [Range(0f, 1f)] public float bangWidth;
    [Range(0.25f, 1f)] public float bangLength;
    [Range(0.25f, 1f)] public float hairWidth;
    [Range(0f, 1f)] public float hairHeight;
    [Range(0.5f, 4f)] public float hairLength;

    #endregion

    #region Shader Variables

    [Range(0.1f, 5f)] public float chinWidth, chinLength, foreheadWidth, foreheadLength;
    [Range(2f, 4f)] public float chinScale, foreheadScale;
    public Color headTop, headBottom;

    [Range(1f, 5f)] public float neckTopWidth, neckCurveScale;
    [Range(0f, 3f)] public float neckCurveRoundness;
    public Color neckTop, neckBottom;

    [Range(0f, 1f)] public float eyeRadius, pupilRadius;
    [Range(0.1f, 1f)] public float pupilWidth, pupilLength;
    [Range(0f, 2.75f)] public float eyelidTopLength, eyelidBottomLength;
    [Range(0f, 1f)] public float eyelidTopSkew, eyelidBottomSkew;
    [Range(0f, 1f)] public float eyelidTopOpen, eyelidBottomOpen;
    [Range(0.25f, 1f)] public float pupilRoundness;
    public Color eyelidCenter, eyelidEdge;

    [Range(0, 16)] public float eyebrowCount;
    [Range(1f, 8f)] public float eyebrowThickness;
    [Range(0.3f, 4f)] public float eyebrowRoundness;
    [Range(-1f, 1f)] public float eyebrowCurve;
    public Color eyebrowInner, eyebrowOuter;

        //nose
    [Range(0.1f, 2.5f)]public float noseBaseWidth, noseTopWidth;
    [Range(1f, 5f)] public float noseTotalWidth, noseCurve;
    [Range(0.5f, 3f)] public float noseTotalLength;
    [Range(0f, 0.5f)] public float nostrilRadius;
    [Range(-0.5f, 0f)] public float nostrilHeight;
    [Range(0f, 1)] public float nostrilSpacing;
    [Range(0.25f, 2f)] public float nostrilScale;
    public Color noseTop, noseBottom;

        //mout
    [Range(0.1f, 1f)] public float mouthRadius;
    [Range(-1f, 1f)] public float mouthLipTop, mouthLipBottom;
    [Range(0f, 1f)] public float mouthLipMaskRoundness;
    [Range(0f, 1f)] public float teethTop, teethBottom;
    [Range(0, 30)] public float teethCount;
    [Range(0.5f, 4f)] public float teethRoundness;
    [Range(0f, .25f)] public float tongueRadius;
    [Range(0.25f, .75f)] public float tongueScale;
    [Range(0f, 1f)] public float tongueHeight;
    public Color mouthTop, mouthBottom;
    public Color tongueTop, tongueBottom;

        //ear
    [Range(0f, 1f)] public float earWidthSkew;
    [Range(-1f, 1f)] public float earLengthSkew;
    [Range(1f, 6f)] public float earShape;
    [Range(1f, 1.5f)]public float earOpenWidth, earOpenLength;
    [Range(0.6f, 1.25f)] public float earRoundness;
    [Range(0.5f, 1.25f)] public float earConcha;
    [Range(0f, 1f)] public float earTragus;
    public Color earTop, earBottom;

    [Range(0.25f, 4f)] public float bangRoundnessFront, bangRoundnessBack;
    [Range(0, 20)] public float strandCountFront, strandCountBack;
    [Range(0, 1)] public float strandOffsetFront, strandOffsetBack;
    [Range(0.5f, 2f)] public float hairBangScaleFront, hairBangScaleBack;

    [Range(1f, 5f)] public float hairRoundnessFront, hairRoundnessBack;
    public Color hairBaseFront, hairBaseBack, hairAccentFront, hairAccentBack;

    #endregion

    public float leftPupilX;
    public float leftPupilY;
    public float rightPupilX;
    public float rightPupilY;

    public Vector2 mousePos;

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

        currentTransform.localPosition = new Vector3(pos.x * flip, pos.y, currentTransform.localPosition.z);

        currentPC.pd.ClampedPosition(currentTransform.localPosition);

        if(currentPC.affectedParts.Count > 0){
            for(int i = 0; i < currentPC.affectedParts.Count; i++){
                currentPC.affectedParts[i].pd.SetPositionBounds(currentPC.pd);
                currentPC.affectedParts[i].pd.SetScaleBounds(currentPC.pd);
                currentPC.affectedParts[i].UpdateAllTransformValues();
            }
        }

        currentPC.UpdateAllTransformValues();
        
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
        //currentPC.pd.SetPositionBounds();

        SetTransformControllers(currentTransform);

    }

    private void SetPartRotation(Vector3 pos){

        float angle = Mathf.Atan2(currentTransform.localPosition.y - pos.y, currentTransform.localPosition.x - pos.x) * Mathf.Rad2Deg;
        
        currentTransform.localRotation = Quaternion.Euler(0f, 0f, ClampPartRotation(currentPC, angle));
        currentPC.pd.currentAngle = currentTransform.localEulerAngles.z;
        SetTransformControllers(currentTransform);
        
    }


    public float ClampPartRotation(PartController pc, float angle){
        return pc.pd.ClampedAngle(angle);
    }

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
    }
    

/*
    void Update(){

        mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
       
        //position of mouse relative to right eye position
        Vector2 rightX = new Vector2(RightEye.transform.localPosition.x - mousePos.x, 0);
        Vector2 rightY = new Vector2(0, RightEye.transform.localPosition.y - mousePos.y);
        Vector2 rotatedRight = Rotate2D(rightX, -eyeAngle * Mathf.Deg2Rad);
        Vector2 rotatedRight2 = Rotate2D(rightY, -eyeAngle * Mathf.Deg2Rad);

        Vector2 leftX = new Vector2(LeftEye.transform.localPosition.x - mousePos.x, 0);
        Vector2 leftY = new Vector2(0, RightEye.transform.localPosition.y - mousePos.y);
        Vector2 rotatedLeft = Rotate2D(leftX, eyeAngle * Mathf.Deg2Rad);
        Vector2 rotatedLeft2 = Rotate2D(leftY, eyeAngle * Mathf.Deg2Rad);

        float clampVal = eyeRadius/6f;
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

        LeftEyeProp.SetFloat("_PupilOffsetX", -leftPupilX);
        LeftEyeProp.SetFloat("_PupilOffsetY", leftPupilY);

        RightEyeProp.SetFloat("_PupilOffsetX", rightPupilX);
        RightEyeProp.SetFloat("_PupilOffsetY", rightPupilY);

        LeftEyeProp.SetFloat("_PupilOffsetX", 0f);
        LeftEyeProp.SetFloat("_PupilOffsetY", 0f);

        RightEyeProp.SetFloat("_PupilOffsetX", 0f);
        RightEyeProp.SetFloat("_PupilOffsetY", 0f);

        LeftEyeProp.SetFloat("_EyelidTopOpen", Mathf.Sin(Time.time)+1);
        LeftEyeProp.SetFloat("_EyelidBottomOpen", Mathf.Sin(Time.time)+1);

        RightEyeProp.SetFloat("_EyelidTopOpen", Mathf.Sin(Time.time)+1);
        RightEyeProp.SetFloat("_EyelidBottomOpen", Mathf.Sin(Time.time)+1);

        LeftEye.SetPropertyBlock(LeftEyeProp);
        RightEye.SetPropertyBlock(RightEyeProp);


    }*/
}
