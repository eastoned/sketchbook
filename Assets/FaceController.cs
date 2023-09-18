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
        float actualHeadWidth = Mathf.Lerp(1,4, headWidth);
        float actualHeadLength = Mathf.Lerp(2,4, headLength);
        Head.transform.localScale = new Vector3(actualHeadWidth, actualHeadLength, 0);
        
        LeftEye.transform.localEulerAngles = new Vector3(0, 0, eyeAngle);
        RightEye.transform.localEulerAngles = new Vector3(0, 0, -eyeAngle);

        float actualEyeHeight = eyeHeight * Mathf.Lerp(1, 2, headLength) * Mathf.Lerp(2/chinScale, 2/foreheadScale, eyeHeight*.5f+0.5f);
        
        LeftEye.transform.localPosition = new Vector3(Mathf.Lerp(-eyeWidth/2f, -actualHeadLength/2f, headWidth * eyeSpacing), actualEyeHeight, 0);
        RightEye.transform.localPosition = new Vector3(Mathf.Lerp(eyeWidth/2f, actualHeadLength/2f, headWidth * eyeSpacing), actualEyeHeight, 0);
        LeftEye.transform.localScale = new Vector3(-eyeWidth, eyeLength, 1);
        RightEye.transform.localScale = new Vector3(eyeWidth, eyeLength, 1);

        LeftEyebrow.transform.localEulerAngles = new Vector3(0, 0, eyebrowAngle);
        RightEyebrow.transform.localEulerAngles = new Vector3(0, 0, -eyebrowAngle);

        float actualEyebrowHeight = Mathf.Lerp(actualEyeHeight, Mathf.Lerp(1, 2, headLength) * 2/foreheadScale, eyebrowHeight);
        
        LeftEyebrow.transform.localPosition = new Vector3(Mathf.Lerp(-eyebrowLength/2, -actualHeadWidth/2f, headWidth* eyebrowSpacing), actualEyebrowHeight, -0.15f);
        RightEyebrow.transform.localPosition = new Vector3(Mathf.Lerp(eyebrowLength/2, actualHeadWidth/2f, headWidth* eyebrowSpacing), actualEyebrowHeight, -0.15f);
        LeftEyebrow.transform.localScale = new Vector3(-eyebrowLength, eyebrowWidth, 1);
        RightEyebrow.transform.localScale = new Vector3(eyebrowLength, eyebrowWidth, 1);
    
        float actualNoseHeight = Mathf.Lerp(actualEyeHeight, -actualHeadLength/2 * (Mathf.Lerp(1, 1/chinScale, noseHeight)), noseHeight);
        Nose.transform.localPosition = new Vector3(0, actualNoseHeight, -0.1f);
        Nose.transform.localScale = new Vector3(noseWidth, noseLength, 1);

        

        LeftEar.transform.localScale = new Vector3(-earWidth,earLength,1);
        RightEar.transform.localScale = new Vector3(earWidth,earLength,1);


        float distance = (LeftEar.transform.localPosition - new Vector3(0,0,0)).magnitude;
        LeftEar.transform.localPosition = new Vector3(0, 0, 0) + Vector3.forward * distance;


        //RotateAround(LeftEar.transform, new Vector3(0, 0, 0), Quaternion.Euler(0,0,earAngle));


        LeftEar.transform.localEulerAngles = new Vector3(0, 0, earAngle);
        RightEar.transform.localEulerAngles = new Vector3(0, 0, -earAngle);
        float actualEarHeight = earHeight * Mathf.Lerp(1, 2, headLength) * Mathf.Lerp(2/chinScale, 2/foreheadScale, earHeight*.5f+0.5f);
        LeftEar.transform.localPosition = new Vector3(Mathf.Lerp(-earWidth/2f, -actualHeadWidth/2f - earWidth/2f, headWidth * earSpacing) + Mathf.Abs(earAngle)/90f, earHeight * headLength - earAngle/90f, 0.2f);
        RightEar.transform.localPosition = new Vector3(Mathf.Lerp(earWidth/2f, actualHeadWidth/2f + earWidth/2f, headWidth * earSpacing) - Mathf.Abs(earAngle)/90f, earHeight * headLength - earAngle/90f, 0.2f);

        Mouth.transform.localScale = new Vector3(mouthWidth, mouthLength, 1);
        Mouth.transform.localPosition = new Vector3(0, Mathf.Lerp(actualNoseHeight, -actualHeadLength/2 * 2/chinScale, mouthHeight), 0);

        Neck.transform.localScale = new Vector3(Mathf.Lerp(0.5f, actualHeadWidth, neckWidth), 2f, 1f);

        HairFront.transform.localPosition = new Vector3(0, Mathf.Lerp(1, 2, headLength) * (2/foreheadScale) - (bangLength * bangHeight), -0.05f);
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

    void BlendTwoCharacters(CharacterData character1, CharacterData character2, float interval){
        headWidth = Mathf.Lerp(character1.headWidth, character2.headWidth, interval);
        headLength = Mathf.Lerp(character1.headLength, character2.headLength, interval);
        neckWidth = Mathf.Lerp(character1.neckWidth, character2.neckWidth, interval);
        eyeAngle = Mathf.Lerp(character1.eyeAngle, character2.eyeAngle, interval);
        eyeSpacing = Mathf.Lerp(character1.eyeSpacing, character2.eyeSpacing, interval);
        eyeHeight = Mathf.Lerp(character1.eyeHeight, character2.eyeHeight, interval);
        eyeLength = Mathf.Lerp(character1.eyeLength, character2.eyeLength, interval);
        eyeWidth = Mathf.Lerp(character1.eyeWidth, character2.eyeWidth, interval);
        eyebrowAngle = Mathf.Lerp(character1.eyebrowAngle, character2.eyebrowAngle, interval);
        eyebrowSpacing = Mathf.Lerp(character1.eyebrowSpacing, character2.eyebrowSpacing, interval);
        eyebrowHeight = Mathf.Lerp(character1.eyebrowHeight, character2.eyebrowHeight, interval);
        eyebrowLength = Mathf.Lerp(character1.eyebrowLength, character2.eyebrowLength, interval);
        eyebrowWidth = Mathf.Lerp(character1.eyebrowWidth, character2.eyebrowWidth, interval);
        noseHeight = Mathf.Lerp(character1.noseHeight, character2.noseHeight, interval);
        noseLength = Mathf.Lerp(character1.noseLength, character2.noseLength, interval);
        noseWidth = Mathf.Lerp(character1.noseWidth, character2.noseWidth, interval);
        mouthWidth = Mathf.Lerp(character1.mouthWidth, character2.mouthWidth, interval);
        mouthLength = Mathf.Lerp(character1.mouthLength, character2.mouthLength, interval);
        mouthHeight = Mathf.Lerp(character1.mouthHeight, character2.mouthHeight, interval);
        earWidth = Mathf.Lerp(character1.earWidth, character2.earWidth, interval);
        earLength = Mathf.Lerp(character1.earLength, character2.earLength, interval);
        earHeight = Mathf.Lerp(character1.earHeight, character2.earHeight, interval);
        earAngle = Mathf.Lerp(character1.earAngle, character2.earAngle, interval);
        earSpacing = Mathf.Lerp(character1.earSpacing, character2.earSpacing, interval);
        chinWidth = Mathf.Lerp(character1.chinWidth, character2.chinWidth, interval);
        chinLength = Mathf.Lerp(character1.chinLength, character2.chinLength, interval);
        foreheadWidth = Mathf.Lerp(character1.foreheadWidth, character2.foreheadWidth, interval);
        foreheadLength = Mathf.Lerp(character1.foreheadLength, character2.foreheadLength, interval);
        chinScale = Mathf.Lerp(character1.chinScale, character2.chinScale, interval);
        foreheadScale = Mathf.Lerp(character1.foreheadScale, character2.foreheadScale, interval);
        headTop = Color.Lerp(character1.headTop, character2.headTop, interval);
        headBottom = Color.Lerp(character1.headBottom, character2.headBottom, interval);
        neckTopWidth = Mathf.Lerp(character1.neckTopWidth, character2.neckTopWidth, interval);
        neckCurveScale = Mathf.Lerp(character1.neckCurveScale, character2.neckCurveScale, interval);
        neckCurveRoundness = Mathf.Lerp(character1.neckCurveRoundness, character2.neckCurveRoundness, interval);
        neckTop = Color.Lerp(character1.neckTop, character2.neckTop, interval);
        neckBottom = Color.Lerp(character1.neckBottom, character2.neckBottom, interval);
        eyeRadius = Mathf.Lerp(character1.eyeRadius, character2.eyeRadius, interval);
        pupilRadius = Mathf.Lerp(character1.pupilRadius, character2.pupilRadius, interval);
        pupilWidth = Mathf.Lerp(character1.pupilWidth, character2.pupilWidth, interval);
        pupilLength = Mathf.Lerp(character1.pupilLength, character2.pupilLength, interval);
        eyelidTopLength = Mathf.Lerp(character1.eyelidTopLength, character2.eyelidTopLength, interval);
        eyelidBottomLength = Mathf.Lerp(character1.eyelidBottomLength, character2.eyelidBottomLength, interval);
        eyelidTopSkew = Mathf.Lerp(character1.eyelidTopSkew, character2.eyelidTopSkew, interval);
        eyelidBottomSkew = Mathf.Lerp(character1.eyelidBottomSkew, character2.eyelidBottomSkew, interval);
        eyelidTopOpen = Mathf.Lerp(character1.eyelidTopOpen, character2.eyelidTopOpen, interval);
        eyelidBottomOpen = Mathf.Lerp(character1.eyelidBottomOpen, character2.eyelidBottomOpen, interval);
        pupilRoundness = Mathf.Lerp(character1.pupilRoundness, character2.pupilRoundness, interval);
        eyelidCenter = Color.Lerp(character1.eyelidCenter, character2.eyelidCenter, interval);
        eyelidEdge = Color.Lerp(character1.eyelidEdge, character2.eyelidEdge, interval);
        eyebrowCount = Mathf.Lerp(character1.eyebrowCount, character2.eyebrowCount, interval);
        eyebrowThickness = Mathf.Lerp(character1.eyebrowThickness, character2.eyebrowThickness, interval);
        eyebrowRoundness = Mathf.Lerp(character1.eyebrowRoundness, character2.eyebrowRoundness, interval);
        eyebrowCurve = Mathf.Lerp(character1.eyebrowCurve, character2.eyebrowCurve, interval);
        eyebrowInner = Color.Lerp(character1.eyebrowInner, character2.eyebrowInner, interval);
        eyebrowOuter = Color.Lerp(character1.eyebrowOuter, character2.eyebrowOuter, interval);
        noseBaseWidth = Mathf.Lerp(character1.noseBaseWidth, character2.noseBaseWidth, interval);
        noseTopWidth = Mathf.Lerp(character1.noseTopWidth, character2.noseTopWidth, interval);
        noseTotalWidth = Mathf.Lerp(character1.noseTotalWidth, character2.noseTotalWidth, interval);
        noseCurve = Mathf.Lerp(character1.noseCurve, character2.noseCurve, interval);
        noseTotalLength = Mathf.Lerp(character1.noseTotalLength, character2.noseTotalLength, interval);
        nostrilRadius = Mathf.Lerp(character1.nostrilRadius, character2.nostrilRadius, interval);
        nostrilHeight = Mathf.Lerp(character1.nostrilHeight, character2.nostrilHeight, interval);
        nostrilSpacing = Mathf.Lerp(character1.nostrilSpacing, character2.nostrilSpacing, interval);
        nostrilScale = Mathf.Lerp(character1.nostrilScale, character2.nostrilScale, interval);
        noseTop = Color.Lerp(character1.noseTop, character2.noseTop, interval);
        noseBottom = Color.Lerp(character1.noseBottom, character2.noseBottom, interval);
        mouthRadius = Mathf.Lerp(character1.mouthRadius, character2.mouthRadius, interval);
        mouthLipTop = Mathf.Lerp(character1.mouthLipTop, character2.mouthLipTop, interval);
        mouthLipBottom = Mathf.Lerp(character1.mouthLipBottom, character2.mouthLipBottom, interval);
        mouthLipMaskRoundness = Mathf.Lerp(character1.mouthLipMaskRoundness, character2.mouthLipMaskRoundness, interval);
        teethTop = Mathf.Lerp(character1.teethTop, character2.teethTop, interval);
        teethBottom = Mathf.Lerp(character1.teethBottom, character2.teethBottom, interval);
        teethCount = Mathf.Lerp(character1.teethCount, character2.teethCount, interval);
        teethRoundness = Mathf.Lerp(character1.teethRoundness, character2.teethRoundness, interval);
        tongueRadius = Mathf.Lerp(character1.tongueRadius, character2.tongueRadius, interval);
        tongueScale = Mathf.Lerp(character1.tongueScale, character2.tongueScale, interval);
        tongueHeight = Mathf.Lerp(character1.tongueHeight, character2.tongueHeight, interval);
        mouthTop = Color.Lerp(character1.mouthTop, character2.mouthTop, interval);
        mouthBottom = Color.Lerp(character1.mouthBottom, character2.mouthBottom, interval);
        tongueTop = Color.Lerp(character1.tongueTop, character2.tongueTop, interval);
        tongueBottom = Color.Lerp(character1.tongueBottom, character2.tongueBottom, interval);
        earWidthSkew = Mathf.Lerp(character1.earWidthSkew, character2.earWidthSkew, interval);
        earLengthSkew = Mathf.Lerp(character1.earLengthSkew, character2.earLengthSkew, interval);
        earShape = Mathf.Lerp(character1.earShape, character2.earShape, interval);
        earOpenWidth = Mathf.Lerp(character1.earOpenWidth, character2.earOpenWidth, interval);
        earOpenLength = Mathf.Lerp(character1.earOpenLength, character2.earOpenLength, interval);
        earRoundness = Mathf.Lerp(character1.earRoundness, character2.earRoundness, interval);
        earConcha = Mathf.Lerp(character1.earConcha, character2.earConcha, interval);
        earTragus = Mathf.Lerp(character1.earTragus, character2.earTragus, interval);
        earTop = Color.Lerp(character1.earTop, character2.earTop, interval);
        earBottom = Color.Lerp(character1.earBottom, character2.earBottom, interval);
        bangWidth = Mathf.Lerp(character1.bangWidth, character2.bangWidth, interval);
        bangHeight = Mathf.Lerp(character1.bangHeight, character2.bangHeight, interval);
        bangLength = Mathf.Lerp(character1.bangLength, character2.bangLength, interval);
        hairWidth = Mathf.Lerp(character1.hairWidth, character2.hairWidth, interval);
        hairHeight = Mathf.Lerp(character1.hairHeight, character2.hairHeight, interval);
        hairLength = Mathf.Lerp(character1.hairLength, character2.hairLength, interval);
        bangRoundnessFront = Mathf.Lerp(character1.bangRoundnessFront, character2.bangRoundnessFront, interval);
        strandCountFront = Mathf.Lerp(character1.strandCountFront, character2.strandCountFront, interval);
        strandOffsetFront = Mathf.Lerp(character1.strandOffsetFront, character2.strandOffsetFront, interval);
        hairBangScaleFront = Mathf.Lerp(character1.hairBangScaleFront, character2.hairBangScaleFront, interval);
        hairRoundnessFront = Mathf.Lerp(character1.hairRoundnessFront, character2.hairRoundnessFront, interval);
        hairBaseFront = Color.Lerp(character1.hairBaseFront, character2.hairBaseFront, interval);
        hairAccentFront = Color.Lerp(character1.hairAccentFront, character2.hairAccentFront, interval);
        bangRoundnessBack = Mathf.Lerp(character1.bangRoundnessBack, character2.bangRoundnessBack, interval);
        strandCountBack = Mathf.Lerp(character1.strandCountBack, character2.strandCountBack, interval);
        strandOffsetBack = Mathf.Lerp(character1.strandOffsetBack, character2.strandOffsetBack, interval);
        hairBangScaleBack = Mathf.Lerp(character1.hairBangScaleBack, character2.hairBangScaleBack, interval);
        hairRoundnessBack = Mathf.Lerp(character1.hairRoundnessBack, character2.hairRoundnessBack, interval);
        hairBaseBack = Color.Lerp(character1.hairBaseBack, character2.hairBaseBack, interval);
        hairAccentBack = Color.Lerp(character1.hairAccentBack, character2.hairAccentBack, interval);

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


    public Vector2 Rotate2D(Vector2 v, float delta) {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    void Update(){

        if(Input.GetMouseButtonDown(0)){
            AllRandom();
            SetTransformValues();
            SetShaderValues();
        }
        if(Input.GetMouseButtonDown(1)){
            LoadCharacterData();
        }

        //BlendTwoCharacters(blendChar1, blendChar2, blendCharacterValue);

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

        LeftEyeProp.SetFloat("_EyelidTopOpen", Mathf.Sin(Time.time)+1);
        LeftEyeProp.SetFloat("_EyelidBottomOpen", Mathf.Sin(Time.time)+1);

        RightEyeProp.SetFloat("_EyelidTopOpen", Mathf.Sin(Time.time)+1);
        RightEyeProp.SetFloat("_EyelidBottomOpen", Mathf.Sin(Time.time)+1);

        LeftEye.SetPropertyBlock(LeftEyeProp);
        RightEye.SetPropertyBlock(RightEyeProp);


    }
}
