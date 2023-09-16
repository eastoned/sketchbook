using System.Collections;
using System.Collections.Generic;
using OpenCvSharp;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR;

public class FaceController : MonoBehaviour
{
    [SerializeField] public Renderer LeftEye, RightEye, LeftEyebrow, RightEyebrow, LeftEar, RightEar, Nose, Mouth, Head, Neck;

    public HeadController headScalerTop, headScalerBottom, headScalerLeft, headScalerRight;

    public CharacterData currentChar;

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

    [Range(-0.25f, 0.5f)] public float noseHeight;
    [Range(0.5f, 2f)] public float noseLength, noseWidth;

    [Range(0.25f, 2.5f)] public float mouthWidth;
    [Range(0.2f, 1f)] public float mouthLength;
    [Range(-1f, 1.25f)] public float mouthHeight;
        
    [Range(.5f, 2f)] public float earWidth, earLength;
    [Range(-0.5f, 1f)] public float earHeight;
    [Range(-45f, 45f)] public float earAngle;
    [Range(0f, 1f)] public float earSpacing;

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


    public float leftPupilX;
    public float leftPupilY;
    public float rightPupilX;
    public float rightPupilY;

    public Vector2 mousePos;

    public void SetTransformValues(){
        float actualHeadWidth = Mathf.Lerp(2,4, headWidth);
        Head.transform.localScale = new Vector3(actualHeadWidth, Mathf.Lerp(2,4, headLength), 0);
        
        LeftEye.transform.localEulerAngles = new Vector3(0, 0, eyeAngle);
        RightEye.transform.localEulerAngles = new Vector3(0, 0, -eyeAngle);
        
        LeftEye.transform.localPosition = new Vector3(((Mathf.Lerp(-0.5f, -1.5f, headWidth) - (eyeWidth/2f)) * eyeSpacing), eyeHeight * Mathf.Lerp(1, 2, headLength) * Mathf.Lerp(2/foreheadScale,2/chinScale,eyeHeight*.5f+0.5f), 0);
        RightEye.transform.localPosition = new Vector3(((Mathf.Lerp(0.5f, 1.5f, headWidth) + (eyeWidth/2f)) * eyeSpacing), eyeHeight * Mathf.Lerp(1, 2, headLength) * Mathf.Lerp(2/foreheadScale,2/chinScale,eyeHeight*.5f+0.5f), 0);
        LeftEye.transform.localScale = new Vector3(-eyeWidth, eyeLength, 1);
        RightEye.transform.localScale = new Vector3(eyeWidth, eyeLength, 1);

        LeftEyebrow.transform.localEulerAngles = new Vector3(0, 0, eyebrowAngle);
        RightEyebrow.transform.localEulerAngles = new Vector3(0, 0, -eyebrowAngle);
        
        
        LeftEyebrow.transform.localPosition = new Vector3(((Mathf.Lerp(-0.5f, -1.5f, headWidth)*eyeSpacing - (eyebrowLength/2f)) * eyebrowSpacing), eyeHeight * Mathf.Lerp(1, 2, headLength) + eyebrowHeight, -0.1f);
        RightEyebrow.transform.localPosition = new Vector3(((Mathf.Lerp(0.5f, 1.5f, headWidth)*eyeSpacing + (eyebrowLength/2f)) * eyebrowSpacing), eyeHeight * Mathf.Lerp(1, 2, headLength) + eyebrowHeight, -0.1f);
        LeftEyebrow.transform.localScale = new Vector3(-eyebrowLength, eyebrowWidth, 1);
        RightEyebrow.transform.localScale = new Vector3(eyebrowLength, eyebrowWidth, 1);
    
        Nose.transform.localPosition = new Vector3(0, noseHeight, -0.1f);
        Nose.transform.localScale = new Vector3(noseWidth, noseLength, 1);

        LeftEar.transform.localPosition = new Vector3(((Mathf.Lerp(-1.25f, -2.25f, headWidth) - (earWidth/3f)) * earSpacing), earHeight * headLength - earAngle/90f, 0.2f);
        RightEar.transform.localPosition = new Vector3(((Mathf.Lerp(1.25f, 2.25f, headWidth) + (earWidth/3f)) * earSpacing), earHeight * headLength - earAngle/90f, 0.2f);

        LeftEar.transform.localScale = new Vector3(-earWidth,earLength,1);
        RightEar.transform.localScale = new Vector3(earWidth,earLength,1);

        LeftEar.transform.localEulerAngles = new Vector3(0, 0, earAngle);
        RightEar.transform.localEulerAngles = new Vector3(0, 0, -earAngle);

        Mouth.transform.localScale = new Vector3(mouthWidth, mouthLength, 1);
        Mouth.transform.localPosition = new Vector3(0, -1 + mouthHeight, 0);

        Neck.transform.localScale = new Vector3(Mathf.Lerp(0.5f, actualHeadWidth, neckWidth), 2f, 1f);
    }

    public void SetShaderValues(){
        MaterialPropertyBlock EyeProp, EyebrowProp, EarProp, NoseProp, MouthProp, HeadProp, NeckProp;
        EyeProp = new MaterialPropertyBlock();
        EyebrowProp = new MaterialPropertyBlock();
        EarProp = new MaterialPropertyBlock();
        NoseProp = new MaterialPropertyBlock();
        MouthProp = new MaterialPropertyBlock();
        HeadProp = new MaterialPropertyBlock();
        NeckProp = new MaterialPropertyBlock();

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

        EyeProp.SetFloat("_EyeRadius", eyeRadius);
        EyeProp.SetFloat("_PupilRadius", pupilRadius);
        EyeProp.SetFloat("_PupilWidth", pupilWidth);
        EyeProp.SetFloat("_PupilLength", pupilLength);
        EyeProp.SetFloat("_EyelidTopLength", eyelidTopLength);
        EyeProp.SetFloat("_EyelidTopSkew", eyelidTopSkew);
        EyeProp.SetFloat("_EyelidBottomLength", eyelidBottomLength);
        EyeProp.SetFloat("_EyelidBottomSkew", eyelidBottomSkew);
        EyeProp.SetFloat("_EyelidTopOpen", eyelidTopOpen);
        EyeProp.SetFloat("_EyelidBottomOpen", eyelidBottomOpen);
        EyeProp.SetFloat("_PupilRoundness", pupilRoundness);

        EyeProp.SetColor("_Color1", eyelidCenter);
        EyeProp.SetColor("_Color2", eyelidEdge);
        EyeProp.SetColor("_Color3", Color.black);
        EyeProp.SetColor("_Color4", Color.black);

        LeftEye.SetPropertyBlock(EyeProp);
        RightEye.SetPropertyBlock(EyeProp);

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

    }

    void Start(){
        LoadCharacterData();
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
        mouthHeight = Random.Range(-1f, 1.25f);

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
        noseHeight = Random.Range(-0.25f, 0.5f);
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

    #endregion
}
