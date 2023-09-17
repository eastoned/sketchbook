using System.Collections;
using System.Collections.Generic;
using OpenCvSharp;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR;

public class FaceController : MonoBehaviour
{
    [HideInInspector][SerializeField] public Renderer LeftEye, RightEye, LeftEyebrow, RightEyebrow, LeftEar, RightEar, Nose, Mouth, Head, Neck, HairFront, HairBack;

    public HeadController headScalerTop, headScalerBottom, headScalerLeft, headScalerRight;

    public CharacterData currentChar;

    #region Transform Variables

    [Range(0f, 1f)] public float headWidth, headLength;
        
    [Range(0f, 1f)] public float neckWidth;

    [Range(-45f, 25)] public float eyeAngle;
    [Range(0f, 1f)] public float eyeSpacing;
    [Range(-0.75f, 1f)] public float eyeHeight;
    [Range(0.5f, 1f)] public float eyeLength, eyeWidth;

    [Range(-45f, 25f)] public float eyebrowAngle;
    [Range(0f, 1f)] public float eyebrowSpacing;
    [Range(-0.25f, 0.5f)] public float eyebrowHeight;
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

    [Range(0f, 1f)] public float bangWidth, bangHeight;
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
    [Range(0.1f, 3f)] public float noseTotalLength;
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

    MaterialPropertyBlock LeftEyeProp, RightEyeProp;

    public void SetTransformValues(){
        float actualHeadWidth = Mathf.Lerp(2,4, headWidth);
        float actualHeadLength = Mathf.Lerp(2,4, headLength);
        Head.transform.localScale = new Vector3(actualHeadWidth, actualHeadLength, 0);
        
        LeftEye.transform.localEulerAngles = new Vector3(0, 0, eyeAngle);
        RightEye.transform.localEulerAngles = new Vector3(0, 0, -eyeAngle);

        float actualEyeHeight = eyeHeight * Mathf.Lerp(1, 2, headLength) * Mathf.Lerp(2/chinScale, 2/foreheadScale, eyeHeight*.5f+0.5f);
        
        LeftEye.transform.localPosition = new Vector3(((Mathf.Lerp(-0.5f, -1.5f, headWidth) - (eyeWidth/2f)) * eyeSpacing), actualEyeHeight, 0);
        RightEye.transform.localPosition = new Vector3(((Mathf.Lerp(0.5f, 1.5f, headWidth) + (eyeWidth/2f)) * eyeSpacing), actualEyeHeight, 0);
        LeftEye.transform.localScale = new Vector3(-eyeWidth, eyeLength, 1);
        RightEye.transform.localScale = new Vector3(eyeWidth, eyeLength, 1);

        LeftEyebrow.transform.localEulerAngles = new Vector3(0, 0, eyebrowAngle);
        RightEyebrow.transform.localEulerAngles = new Vector3(0, 0, -eyebrowAngle);
        
        LeftEyebrow.transform.localPosition = new Vector3(((Mathf.Lerp(-0.5f, -1.5f, headWidth)*eyeSpacing - (eyebrowLength/2f)) * eyebrowSpacing), eyeHeight * Mathf.Lerp(1, 2, headLength) + eyebrowHeight, -0.2f);
        RightEyebrow.transform.localPosition = new Vector3(((Mathf.Lerp(0.5f, 1.5f, headWidth)*eyeSpacing + (eyebrowLength/2f)) * eyebrowSpacing), eyeHeight * Mathf.Lerp(1, 2, headLength) + eyebrowHeight, -0.2f);
        LeftEyebrow.transform.localScale = new Vector3(-eyebrowLength, eyebrowWidth, 1);
        RightEyebrow.transform.localScale = new Vector3(eyebrowLength, eyebrowWidth, 1);
    
        float actualNoseHeight = Mathf.Lerp(actualEyeHeight, -actualHeadLength/2 * (Mathf.Lerp(1, 1/chinScale, noseHeight)), noseHeight);
        Nose.transform.localPosition = new Vector3(0, actualNoseHeight, -0.1f);
        Nose.transform.localScale = new Vector3(noseWidth, noseLength, 1);

        LeftEar.transform.localPosition = new Vector3(((Mathf.Lerp(-1.25f, -2.25f, headWidth) - (earWidth/3f)) * earSpacing), earHeight * headLength - earAngle/90f, 0.2f);
        RightEar.transform.localPosition = new Vector3(((Mathf.Lerp(1.25f, 2.25f, headWidth) + (earWidth/3f)) * earSpacing), earHeight * headLength - earAngle/90f, 0.2f);

        LeftEar.transform.localScale = new Vector3(-earWidth,earLength,1);
        RightEar.transform.localScale = new Vector3(earWidth,earLength,1);

        LeftEar.transform.localEulerAngles = new Vector3(0, 0, earAngle);
        RightEar.transform.localEulerAngles = new Vector3(0, 0, -earAngle);

        Mouth.transform.localScale = new Vector3(mouthWidth, mouthLength, 1);
        Mouth.transform.localPosition = new Vector3(0, Mathf.Lerp(actualNoseHeight, -actualHeadLength/2 * 2/chinScale, mouthHeight), 0);

        Neck.transform.localScale = new Vector3(Mathf.Lerp(0.5f, actualHeadWidth, neckWidth), 2f, 1f);

        HairFront.transform.localPosition = new Vector3(0, Mathf.Lerp(1, 2, headLength) * (2/foreheadScale) - (bangLength * bangHeight), 0);
        HairFront.transform.localScale = new Vector3(Mathf.Lerp(0, actualHeadWidth, bangWidth), bangLength*2, 1);

        HairBack.transform.localPosition = new Vector3(0, Mathf.Lerp(0, Mathf.Lerp(1, 2, headLength) * (2/foreheadScale) - hairLength/2, hairHeight), 0.3f);
        HairBack.transform.localScale = new Vector3(Mathf.Lerp(0, actualHeadWidth * 1.25f, hairWidth), hairLength, 1);
    }

    public void SetShaderValues(){
        MaterialPropertyBlock EyebrowProp, EarProp, NoseProp, MouthProp, HeadProp, NeckProp, HairFrontProp, HairBackProp;
        LeftEyeProp = new MaterialPropertyBlock();
        RightEyeProp = new MaterialPropertyBlock();
        EyebrowProp = new MaterialPropertyBlock();
        EarProp = new MaterialPropertyBlock();
        NoseProp = new MaterialPropertyBlock();
        MouthProp = new MaterialPropertyBlock();
        HeadProp = new MaterialPropertyBlock();
        NeckProp = new MaterialPropertyBlock();
        HairFrontProp = new MaterialPropertyBlock();
        HairBackProp = new MaterialPropertyBlock();

        HeadProp.SetFloat("_ChinWidth", chinWidth);
        HeadProp.SetFloat("_ChinLength", chinLength);
        HeadProp.SetFloat("_ChinScale", chinScale);
        HeadProp.SetFloat("_ForeheadWidth", foreheadWidth);
        HeadProp.SetFloat("_ForeheadLength", foreheadLength);
        HeadProp.SetFloat("_ForeheadScale", foreheadScale);
        HeadProp.SetColor("_Color1", headBottom);
        HeadProp.SetColor("_Color2", headTop);

        Head.SetPropertyBlock(HeadProp);

        NeckProp.SetFloat("_NeckTopWidth", neckTopWidth);
        NeckProp.SetFloat("_NeckCurveRoundness", neckCurveRoundness);
        NeckProp.SetFloat("_NeckCurveScale", neckCurveScale);
        NeckProp.SetColor("_Color1", neckBottom);
        NeckProp.SetColor("_Color2", neckTop);

        Neck.SetPropertyBlock(NeckProp);

        LeftEyeProp.SetFloat("_EyeRadius", eyeRadius);
        LeftEyeProp.SetFloat("_PupilRadius", pupilRadius);
        LeftEyeProp.SetFloat("_PupilWidth", pupilWidth);
        LeftEyeProp.SetFloat("_PupilLength", pupilLength);
        LeftEyeProp.SetFloat("_EyelidTopLength", eyelidTopLength);
        LeftEyeProp.SetFloat("_EyelidTopSkew", eyelidTopSkew);
        LeftEyeProp.SetFloat("_EyelidBottomLength", eyelidBottomLength);
        LeftEyeProp.SetFloat("_EyelidBottomSkew", eyelidBottomSkew);
        LeftEyeProp.SetFloat("_EyelidTopOpen", eyelidTopOpen);
        LeftEyeProp.SetFloat("_EyelidBottomOpen", eyelidBottomOpen);
        LeftEyeProp.SetFloat("_PupilRoundness", pupilRoundness);

        LeftEyeProp.SetColor("_Color1", eyelidCenter);
        LeftEyeProp.SetColor("_Color2", eyelidEdge);
        LeftEyeProp.SetColor("_Color3", Color.black);
        LeftEyeProp.SetColor("_Color4", Color.black);

        RightEyeProp.SetFloat("_EyeRadius", eyeRadius);
        RightEyeProp.SetFloat("_PupilRadius", pupilRadius);
        RightEyeProp.SetFloat("_PupilWidth", pupilWidth);
        RightEyeProp.SetFloat("_PupilLength", pupilLength);
        RightEyeProp.SetFloat("_EyelidTopLength", eyelidTopLength);
        RightEyeProp.SetFloat("_EyelidTopSkew", eyelidTopSkew);
        RightEyeProp.SetFloat("_EyelidBottomLength", eyelidBottomLength);
        RightEyeProp.SetFloat("_EyelidBottomSkew", eyelidBottomSkew);
        RightEyeProp.SetFloat("_EyelidTopOpen", eyelidTopOpen);
        RightEyeProp.SetFloat("_EyelidBottomOpen", eyelidBottomOpen);
        RightEyeProp.SetFloat("_PupilRoundness", pupilRoundness);

        RightEyeProp.SetColor("_Color1", eyelidCenter);
        RightEyeProp.SetColor("_Color2", eyelidEdge);
        RightEyeProp.SetColor("_Color3", Color.black);
        RightEyeProp.SetColor("_Color4", Color.black);

        LeftEye.SetPropertyBlock(LeftEyeProp);
        RightEye.SetPropertyBlock(RightEyeProp);

        EyebrowProp.SetFloat("_EyebrowCount", eyebrowCount);
        EyebrowProp.SetFloat("_EyebrowThickness", eyebrowThickness);
        EyebrowProp.SetFloat("_EyebrowRoundness", eyebrowRoundness);
        EyebrowProp.SetFloat("_EyebrowCurve", eyebrowCurve);

        EyebrowProp.SetColor("_Color1", eyebrowInner);
        EyebrowProp.SetColor("_Color2", eyebrowOuter);

        LeftEyebrow.SetPropertyBlock(EyebrowProp);
        RightEyebrow.SetPropertyBlock(EyebrowProp);

        NoseProp.SetFloat("_NoseBaseWidth", noseBaseWidth);
        NoseProp.SetFloat("_NoseTotalWidth", noseTotalWidth);
        NoseProp.SetFloat("_NoseTopWidth", noseTopWidth);
        NoseProp.SetFloat("_NoseCurve", noseCurve);
        NoseProp.SetFloat("_NoseTotalLength", noseTotalLength);
        NoseProp.SetFloat("_NostrilRadius", nostrilRadius);
        NoseProp.SetFloat("_NostrilSpacing", nostrilSpacing);
        NoseProp.SetFloat("_NostrilHeight", nostrilHeight);
        NoseProp.SetFloat("_NostrilScale", nostrilScale);

        NoseProp.SetColor("_Color1", noseBottom);
        NoseProp.SetColor("_Color2", noseTop);

        Nose.SetPropertyBlock(NoseProp);

        MouthProp.SetFloat("_MouthRadius", mouthRadius);
        MouthProp.SetFloat("_MouthLipMaskRoundness", mouthLipMaskRoundness);
        MouthProp.SetFloat("_MouthLipTop", mouthLipTop);
        MouthProp.SetFloat("_MouthLipBottom", mouthLipBottom);
        MouthProp.SetFloat("_TeethTop", teethTop);
        MouthProp.SetFloat("_TeethBottom", teethBottom);
        MouthProp.SetFloat("_TeethCount", teethCount);
        MouthProp.SetFloat("_TeethRoundness", teethRoundness);
        MouthProp.SetFloat("_TongueRadius", tongueRadius);
        MouthProp.SetFloat("_TongueScale", tongueScale);
        MouthProp.SetFloat("_TongueHeight", tongueHeight);

        MouthProp.SetColor("_Color1", mouthBottom);
        MouthProp.SetColor("_Color2", mouthTop);
        MouthProp.SetColor("_Color3", tongueBottom);
        MouthProp.SetColor("_Color4", tongueTop);

        Mouth.SetPropertyBlock(MouthProp);

        EarProp.SetFloat("_EarWidthSkew", earWidthSkew);
        EarProp.SetFloat("_EarLengthSkew", earLengthSkew);
        EarProp.SetFloat("_EarShape", earShape);
        EarProp.SetFloat("_EarRoundness", earRoundness);
        EarProp.SetFloat("_EarOpenWidth", earOpenWidth);
        EarProp.SetFloat("_EarOpenLength", earOpenLength);
        EarProp.SetFloat("_EarConcha", earConcha);
        EarProp.SetFloat("_EarTragus", earTragus);

        EarProp.SetColor("_Color1", earBottom);
        EarProp.SetColor("_Color2", earTop);

        LeftEar.SetPropertyBlock(EarProp);
        RightEar.SetPropertyBlock(EarProp);

        HairFrontProp.SetFloat("_BangRoundness", bangRoundnessFront);
        HairFrontProp.SetFloat("_StrandCount", strandCountFront);
        HairFrontProp.SetFloat("_StrandOffset", strandOffsetFront);
        HairFrontProp.SetFloat("_HairBangScale", hairBangScaleFront);
        HairFrontProp.SetFloat("_HairRoundness", hairRoundnessFront);

        HairFrontProp.SetColor("_Color1", hairBaseFront);
        HairFrontProp.SetColor("_Color2", hairAccentFront);

        HairBackProp.SetFloat("_BangRoundness", bangRoundnessBack);
        HairBackProp.SetFloat("_StrandCount", strandCountBack);
        HairBackProp.SetFloat("_StrandOffset", strandOffsetBack);
        HairBackProp.SetFloat("_HairBangScale", hairBangScaleBack);
        HairBackProp.SetFloat("_HairRoundness", hairRoundnessBack);

        HairBackProp.SetColor("_Color1", hairBaseBack);
        HairBackProp.SetColor("_Color2", hairAccentBack);

        HairFront.SetPropertyBlock(HairFrontProp);
        HairBack.SetPropertyBlock(HairBackProp);

    }

    void Start(){
        //LoadCharacterData();
    }

    public void LoadCharacterData(){
        headWidth = currentChar.headWidth;
        headLength = currentChar.headLength;
        neckWidth = currentChar.neckWidth;
        eyeAngle = currentChar.eyeAngle;
        eyeSpacing = currentChar.eyeSpacing;
        eyeHeight = currentChar.eyeHeight;
        eyeLength = currentChar.eyeLength;
        eyeWidth = currentChar.eyeWidth;
        eyebrowAngle = currentChar.eyebrowAngle;
        eyebrowSpacing = currentChar.eyebrowSpacing;
        eyebrowHeight = currentChar.eyebrowHeight;
        eyebrowLength = currentChar.eyebrowLength;
        eyebrowWidth = currentChar.eyebrowWidth;
        noseHeight = currentChar.noseHeight;
        noseLength = currentChar.noseLength;
        noseWidth = currentChar.noseWidth;
        mouthWidth = currentChar.mouthWidth;
        mouthLength = currentChar.mouthLength;
        mouthHeight = currentChar.mouthHeight;
        earWidth = currentChar.earWidth;
        earLength = currentChar.earLength;
        earHeight = currentChar.earHeight;
        earAngle = currentChar.earAngle;
        earSpacing = currentChar.earSpacing;
        chinWidth = currentChar.chinWidth;
        chinLength = currentChar.chinLength;
        foreheadWidth = currentChar.foreheadWidth;
        foreheadLength = currentChar.foreheadLength;
        chinScale = currentChar.chinScale;
        foreheadScale = currentChar.foreheadScale;
        headTop = currentChar.headTop;
        headBottom = currentChar.headBottom;
        neckTopWidth = currentChar.neckTopWidth;
        neckCurveScale = currentChar.neckCurveScale;
        neckCurveRoundness = currentChar.neckCurveRoundness;
        neckTop = currentChar.neckTop;
        neckBottom = currentChar.neckBottom;
        eyeRadius = currentChar.eyeRadius;
        pupilRadius = currentChar.pupilRadius;
        pupilWidth = currentChar.pupilWidth;
        pupilLength = currentChar.pupilLength;
        eyelidTopLength = currentChar.eyelidTopLength;
        eyelidBottomLength = currentChar.eyelidBottomLength;
        eyelidTopSkew = currentChar.eyelidTopSkew;
        eyelidBottomSkew = currentChar.eyelidBottomSkew;
        eyelidTopOpen = currentChar.eyelidTopOpen;
        eyelidBottomOpen = currentChar.eyelidBottomOpen;
        pupilRoundness = currentChar.pupilRoundness;
        eyelidCenter = currentChar.eyelidCenter;
        eyelidEdge = currentChar.eyelidEdge;
        eyebrowCount = currentChar.eyebrowCount;
        eyebrowThickness = currentChar.eyebrowThickness;
        eyebrowRoundness = currentChar.eyebrowRoundness;
        eyebrowCurve = currentChar.eyebrowCurve;
        eyebrowInner = currentChar.eyebrowInner;
        eyebrowOuter = currentChar.eyebrowOuter;
        noseBaseWidth = currentChar.noseBaseWidth;
        noseTopWidth = currentChar.noseTopWidth;
        noseTotalWidth = currentChar.noseTotalWidth;
        noseCurve = currentChar.noseCurve;
        noseTotalLength = currentChar.noseTotalLength;
        nostrilRadius = currentChar.nostrilRadius;
        nostrilHeight = currentChar.nostrilHeight;
        nostrilSpacing = currentChar.nostrilSpacing;
        nostrilScale = currentChar.nostrilScale;
        noseTop = currentChar.noseTop;
        noseBottom = currentChar.noseBottom;
        mouthRadius = currentChar.mouthRadius;
        mouthLipTop = currentChar.mouthLipTop;
        mouthLipBottom = currentChar.mouthLipBottom;
        mouthLipMaskRoundness = currentChar.mouthLipMaskRoundness;
        teethTop = currentChar.teethTop;
        teethBottom = currentChar.teethBottom;
        teethCount = currentChar.teethCount;
        teethRoundness = currentChar.teethRoundness;
        tongueRadius = currentChar.tongueRadius;
        tongueScale = currentChar.tongueScale;
        tongueHeight = currentChar.tongueHeight;
        mouthTop = currentChar.mouthTop;
        mouthBottom = currentChar.mouthBottom;
        tongueTop = currentChar.tongueTop;
        tongueBottom = currentChar.tongueBottom;
        earWidthSkew = currentChar.earWidthSkew;
        earLengthSkew = currentChar.earLengthSkew;
        earShape = currentChar.earShape;
        earOpenWidth = currentChar.earOpenWidth;
        earOpenLength = currentChar.earOpenLength;
        earRoundness = currentChar.earRoundness;
        earConcha = currentChar.earConcha;
        earTragus = currentChar.earTragus;
        earTop = currentChar.earTop;
        earBottom = currentChar.earBottom;
        bangWidth = currentChar.bangWidth;
        bangHeight = currentChar.bangHeight;
        bangLength = currentChar.bangLength;
        hairWidth = currentChar.hairWidth;
        hairHeight = currentChar.hairHeight;
        hairLength = currentChar.hairLength;
        bangRoundnessFront = currentChar.bangRoundnessFront;
        strandCountFront = currentChar.strandCountFront;
        strandOffsetFront = currentChar.strandOffsetFront;
        hairBangScaleFront = currentChar.hairBangScaleFront;
        hairRoundnessFront = currentChar.hairRoundnessFront;
        hairBaseFront = currentChar.hairBaseFront;
        hairAccentFront = currentChar.hairAccentFront;
        bangRoundnessBack = currentChar.bangRoundnessBack;
        strandCountBack = currentChar.strandCountBack;
        strandOffsetBack = currentChar.strandOffsetBack;
        hairBangScaleBack = currentChar.hairBangScaleBack;
        hairRoundnessBack = currentChar.hairRoundnessBack;
        hairBaseBack = currentChar.hairBaseBack;
        hairAccentBack = currentChar.hairAccentBack;

        SetTransformValues();
        SetShaderValues();
    }

    void OnValidate(){
        SetTransformValues();
        SetShaderValues();
    }

    public void SaveCharacterData(){

        currentChar.headWidth = headWidth;
        currentChar.headLength = headLength;
        currentChar.neckWidth = neckWidth;
        currentChar.eyeAngle = eyeAngle;
        currentChar.eyeSpacing = eyeSpacing;
        currentChar.eyeHeight = eyeHeight;
        currentChar.eyeLength = eyeLength;
        currentChar.eyeWidth = eyeWidth;
        currentChar.eyebrowAngle = eyebrowAngle;
        currentChar.eyebrowSpacing = eyebrowSpacing;
        currentChar.eyebrowHeight = eyebrowHeight;
        currentChar.eyebrowLength = eyebrowLength;
        currentChar.eyebrowWidth = eyebrowWidth;
        currentChar.noseHeight = noseHeight;
        currentChar.noseLength = noseLength;
        currentChar.noseWidth = noseWidth;
        currentChar.mouthWidth = mouthWidth;
        currentChar.mouthLength = mouthLength;
        currentChar.mouthHeight = mouthHeight;
        currentChar.earWidth = earWidth;
        currentChar.earLength = earLength;
        currentChar.earHeight = earHeight;
        currentChar.earAngle = earAngle;
        currentChar.earSpacing = earSpacing;
        currentChar.chinWidth = chinWidth;
        currentChar.chinLength = chinLength;
        currentChar.foreheadWidth = foreheadWidth;
        currentChar.foreheadLength = foreheadLength;
        currentChar.chinScale = chinScale;
        currentChar.foreheadScale = foreheadScale;
        currentChar.headTop = headTop;
        currentChar.headBottom = headBottom;
        currentChar.neckTopWidth = neckTopWidth;
        currentChar.neckCurveScale = neckCurveScale;
        currentChar.neckCurveRoundness = neckCurveRoundness;
        currentChar.neckTop = neckTop;
        currentChar.neckBottom = neckBottom;
        currentChar.eyeRadius = eyeRadius;
        currentChar.pupilRadius = pupilRadius;
        currentChar.pupilWidth = pupilWidth;
        currentChar.pupilLength = pupilLength;
        currentChar.eyelidTopLength = eyelidTopLength;
        currentChar.eyelidBottomLength = eyelidBottomLength;
        currentChar.eyelidTopSkew = eyelidTopSkew;
        currentChar.eyelidBottomSkew = eyelidBottomSkew;
        currentChar.eyelidTopOpen = eyelidTopOpen;
        currentChar.eyelidBottomOpen = eyelidBottomOpen;
        currentChar.pupilRoundness = pupilRoundness;
        currentChar.eyelidCenter = eyelidCenter;
        currentChar.eyelidEdge = eyelidEdge;
        currentChar.eyebrowCount = eyebrowCount;
        currentChar.eyebrowThickness = eyebrowThickness;
        currentChar.eyebrowRoundness = eyebrowRoundness;
        currentChar.eyebrowCurve = eyebrowCurve;
        currentChar.eyebrowInner = eyebrowInner;
        currentChar.eyebrowOuter = eyebrowOuter;
        currentChar.noseBaseWidth = noseBaseWidth;
        currentChar.noseTopWidth = noseTopWidth;
        currentChar.noseTotalWidth = noseTotalWidth;
        currentChar.noseCurve = noseCurve;
        currentChar.noseTotalLength = noseTotalLength;
        currentChar.nostrilRadius = nostrilRadius;
        currentChar.nostrilHeight = nostrilHeight;
        currentChar.nostrilSpacing = nostrilSpacing;
        currentChar.nostrilScale = nostrilScale;
        currentChar.noseTop = noseTop;
        currentChar.noseBottom = noseBottom;
        currentChar.mouthRadius = mouthRadius;
        currentChar.mouthLipTop = mouthLipTop;
        currentChar.mouthLipBottom = mouthLipBottom;
        currentChar.mouthLipMaskRoundness = mouthLipMaskRoundness;
        currentChar.teethTop = teethTop;
        currentChar.teethBottom = teethBottom;
        currentChar.teethCount = teethCount;
        currentChar.teethRoundness = teethRoundness;
        currentChar.tongueRadius = tongueRadius;
        currentChar.tongueScale = tongueScale;
        currentChar.tongueHeight = tongueHeight;
        currentChar.mouthTop = mouthTop;
        currentChar.mouthBottom = mouthBottom;
        currentChar.tongueTop = tongueTop;
        currentChar.tongueBottom = tongueBottom;
        currentChar.earWidthSkew = earWidthSkew;
        currentChar.earLengthSkew = earLengthSkew;
        currentChar.earShape = earShape;
        currentChar.earOpenWidth = earOpenWidth;
        currentChar.earOpenLength = earOpenLength;
        currentChar.earRoundness = earRoundness;
        currentChar.earConcha = earConcha;
        currentChar.earTragus = earTragus;
        currentChar.earTop = earTop;
        currentChar.earBottom = earBottom;
        currentChar.bangWidth = bangWidth;
        currentChar.bangHeight = bangHeight;
        currentChar.bangLength = bangLength;
        currentChar.hairWidth = hairWidth;
        currentChar.hairHeight = hairHeight;
        currentChar.hairLength = hairLength;
        currentChar.bangRoundnessFront = bangRoundnessFront;
        currentChar.strandCountFront = strandCountFront;
        currentChar.strandOffsetFront = strandOffsetFront;
        currentChar.hairBangScaleFront = hairBangScaleFront;
        currentChar.hairRoundnessFront = hairRoundnessFront;
        currentChar.hairBaseFront = hairBaseFront;
        currentChar.hairAccentFront = hairAccentFront;
        currentChar.bangRoundnessBack = bangRoundnessBack;
        currentChar.strandCountBack = strandCountBack;
        currentChar.strandOffsetBack = strandOffsetBack;
        currentChar.hairBangScaleBack = hairBangScaleBack;
        currentChar.hairRoundnessBack = hairRoundnessBack;
        currentChar.hairBaseBack = hairBaseBack;
        currentChar.hairAccentBack = hairAccentBack;

    }
    
    #region Randomization
    [ContextMenu("Random")]
    public void AllRandom(){
        RandomHead();
        RandomNeck();
        RandomEye();
        RandomEyebrow();
        RandomNose();
        RandomMouth();
        RandomEar();
        RandomBangs();
        RandomHair();
    }

    public void RandomHead(){
        headWidth = Random.Range(0f, 1f);
        headLength = Random.Range(0f, 1f);
        chinWidth = Random.Range(0.1f, 5f);
        chinLength = Random.Range(0.1f, 5f);
        foreheadWidth = Random.Range(0.1f, 5f);
        foreheadLength = Random.Range(0.1f, 5f);
        chinScale = Random.Range(2f, 4f);
        foreheadScale = Random.Range(2f, 4f);
        headTop = Random.ColorHSV();
        headBottom = Random.ColorHSV();
    }

    public void RandomNeck(){
        neckWidth = Random.Range(0f, 1f);
        neckTopWidth = Random.Range(1f, 5f);
        neckCurveScale = Random.Range(1f, 5f);
        neckCurveRoundness = Random.Range(0f, 3f);
        neckTop = Random.ColorHSV();
        neckBottom = Random.ColorHSV();
    }

    public void RandomEar(){
        earWidth = Random.Range(0.5f, 2f);
        earLength = Random.Range(0.5f, 2f);
        earHeight = Random.Range(-.5f, 1f);
        earAngle = Random.Range(-45f, 45f);
        earSpacing = Random.Range(0f, 1f);

        earWidthSkew = Random.Range(0f, 1f);
        earLengthSkew = Random.Range(-1f, 1f);
        earShape = Random.Range(1f, 6f);
        earOpenWidth = Random.Range(1f, 1.5f);
        earOpenLength = Random.Range(1f, 1.5f);
        earRoundness = Random.Range(0.6f, 1.25f);
        earConcha = Random.Range(0.5f, 1.25f);
        earTragus = Random.Range(0f, 1f);
        earTop = Random.ColorHSV();
        earBottom = Random.ColorHSV();
    }

    public void RandomMouth(){
        mouthWidth = Random.Range(0.25f, 2.5f);
        mouthLength = Random.Range(0.2f, 1f);
        mouthHeight = Random.Range(0f, 1f);
        mouthRadius = Random.Range(0.1f, 1f);
        mouthLipTop = Random.Range(-1f, 1f);
        mouthLipBottom = Random.Range(-1f, 1f);
        mouthLipMaskRoundness = Random.Range(0f, 1f);
        teethTop = Random.Range(0f, 1f);
        teethBottom = Random.Range(0f, 1f);
        teethCount = Random.Range(0, 31);
        teethRoundness = Random.Range(0.5f, 4f);
        tongueRadius = Random.Range(0f, 0.25f);
        tongueScale = Random.Range(0.25f, 0.75f);
        tongueHeight = Random.Range(0f, 1f);
        mouthTop = Random.ColorHSV();
        mouthBottom = Random.ColorHSV();
        tongueTop = Random.ColorHSV();
        tongueBottom = Random.ColorHSV();
   
    }

    public void RandomEye(){
        eyeAngle = Random.Range(-45f, 25f);
        eyeSpacing = Random.Range(0f, 1f);
        eyeHeight = Random.Range(-0.75f, 1f);
        eyeLength = Random.Range(0.5f, 1f);
        eyeWidth = Random.Range(0.5f, 1f);

        eyeRadius = Random.Range(0.1f, 1f);
        pupilRadius = Random.Range(0.1f, 1f);
        pupilWidth = Random.Range(0.1f, 1f);
        pupilLength = Random.Range(0.1f, 1f);
        eyelidTopLength = Random.Range(0f, 2.75f);
        eyelidBottomLength = Random.Range(0f, 2.75f);    
        eyelidTopSkew = Random.Range(0f, 1f);
        eyelidBottomSkew = Random.Range(0f, 1f);
        eyelidTopOpen = Random.Range(0f, 1f);
        eyelidBottomOpen = Random.Range(0f, 1f);
        pupilRoundness = Random.Range(0.25f, 1f);
        eyelidCenter = Random.ColorHSV();
        eyelidEdge = Random.ColorHSV();
    }

    public void RandomEyebrow(){
        eyebrowAngle = Random.Range(-45f, 45f);
        eyebrowSpacing = Random.Range(0f, 1f);
        eyebrowHeight = Random.Range(-0.25f, 0.5f);
        eyebrowLength = Random.Range(0.2f, 1.5f);
        eyebrowWidth = Random.Range(0.1f, 0.5f);

        eyebrowCount = Random.Range(0, 17);
        eyebrowThickness = Random.Range(1f, 8f);
        eyebrowRoundness = Random.Range(0.3f, 4f);
        eyebrowCurve = Random.Range(-1f, 1f);

        eyebrowInner = Random.ColorHSV();
        eyebrowOuter = Random.ColorHSV();
    
    }

    public void RandomNose(){
        noseHeight = Random.Range(0f, 1f);
        noseLength = Random.Range(0.5f, 2f);
        noseWidth = Random.Range(0.5f, 2f);

        noseBaseWidth = Random.Range(0.1f, 2.5f);
        noseTopWidth = Random.Range(0.1f, 2.5f);
        noseTotalWidth = Random.Range(1f, 5f);
        noseCurve = Random.Range(1f, 5f);
        noseTotalLength = Random.Range(.1f, 3f);
        nostrilRadius = Random.Range(0f, 0.5f);
        nostrilHeight = Random.Range(-0.5f, 0f);
        nostrilSpacing = Random.Range(0f, 1f);
        nostrilScale = Random.Range(0.25f, 2f);
        noseTop = Random.ColorHSV();
        noseBottom = Random.ColorHSV();
    
    }

    public void RandomBangs(){
        bangWidth = Random.Range(0f, 1f);
        bangHeight = Random.Range(0f, 1f);
        bangLength = Random.Range(0.25f, 1f); 
        bangRoundnessFront = Random.Range(0.25f, 4f);
        strandCountFront = Random.Range(0f, 20f);
        strandOffsetFront = Random.Range(0, 2);
        hairBangScaleFront = Random.Range(0.5f, 2f);
        hairRoundnessFront = Random.Range(1f, 5f);
        hairBaseFront = Random.ColorHSV();
        hairAccentFront = Random.ColorHSV();
        
    }

    public void RandomHair(){
        hairWidth = Random.Range(0.25f, 1f);
        hairHeight = Random.Range(0f, 1f);
        hairLength = Random.Range(0.5f, 4f);
        bangRoundnessBack = Random.Range(0.25f, 4f);
        strandCountBack = Random.Range(0f, 20f);
        strandOffsetBack = Random.Range(0, 2);
        hairBangScaleBack = Random.Range(0.5f, 2f);
        hairRoundnessBack = Random.Range(1f, 5f);
        hairBaseBack = Random.ColorHSV();
        hairAccentBack = Random.ColorHSV();
    }

    #endregion

    void Update(){
/*
        if(Input.GetMouseButtonDown(0)){
            AllRandom();
            SetTransformValues();
            SetShaderValues();
        }
        if(Input.GetMouseButtonDown(1)){
            LoadCharacterData();
        }*/

        mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    
        //leftPupilX = Mathf.Lerp(leftPupilX, Mathf.Clamp(LeftEye.transform.localPosition.x - mousePos.x + Mathf.Cos(eyeAngle), -eyeRadius*.1f, eyeRadius*.1f), Time.deltaTime*5);
        //leftPupilY = Mathf.Lerp(leftPupilY, Mathf.Clamp(LeftEye.transform.localPosition.y - mousePos.y + Mathf.Sin(eyeAngle), -eyeRadius*.1f, eyeRadius*.1f), Time.deltaTime*5);
        //rightPupilX = Mathf.Lerp(rightPupilX, Mathf.Clamp(RightEye.transform.localPosition.x - mousePos.x + Mathf.Cos(-eyeAngle), -eyeRadius*.1f, eyeRadius*.1f), Time.deltaTime*5);
        ///rightPupilY = Mathf.Lerp(rightPupilY, Mathf.Clamp(RightEye.transform.localPosition.y - mousePos.y + Mathf.Sin(-eyeAngle), -eyeRadius*.1f, eyeRadius*.1f), Time.deltaTime*5);
        Vector2 leftOffset = new Vector2(LeftEye.transform.localPosition.x - mousePos.x, LeftEye.transform.localPosition.y - mousePos.y);
        leftPupilX = leftOffset.magnitude * Mathf.Cos(-eyeAngle * Mathf.Deg2Rad);
        leftPupilY = leftOffset.magnitude * Mathf.Sin(-eyeAngle * Mathf.Deg2Rad);

        //vector of mouse to eye distance
        Vector2 rightOffset = new Vector2(RightEye.transform.localPosition.x - mousePos.x, RightEye.transform.localPosition.y - mousePos.y);
        //need to transform this counter to the eye rotation angle
        Vector2 xVector = new Vector2(Mathf.Cos(eyeAngle * Mathf.Rad2Deg), Mathf.Sin(eyeAngle * Mathf.Rad2Deg));
        Vector2 yVector = new Vector2(-Mathf.Sin(eyeAngle * Mathf.Rad2Deg), Mathf.Cos(eyeAngle * Mathf.Rad2Deg));
        Vector2 xVector2 = new Vector2(Mathf.Sin(eyeAngle * Mathf.Rad2Deg), -Mathf.Cos(eyeAngle * Mathf.Rad2Deg));

        Vector2 rotatedOffset = (xVector * rightOffset.x) + (yVector * rightOffset.y);
        Vector2 rotatedOffset2 = (xVector * rightOffset.x) + (xVector2 * rightOffset.y);

        rightPupilX = rotatedOffset.x;
        rightPupilY = rotatedOffset.y;

        LeftEyeProp.SetFloat("_PupilOffsetX", -leftPupilX);
        LeftEyeProp.SetFloat("_PupilOffsetY", leftPupilY);

        RightEyeProp.SetFloat("_PupilOffsetX", rightPupilX);
        RightEyeProp.SetFloat("_PupilOffsetY", rightPupilY);

        LeftEye.SetPropertyBlock(LeftEyeProp);
        RightEye.SetPropertyBlock(RightEyeProp);


    }
}
