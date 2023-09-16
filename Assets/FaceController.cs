using System.Collections;
using System.Collections.Generic;
using OpenCvSharp;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    [SerializeField] public Renderer LeftEye, RightEye, LeftEyebrow, RightEyebrow, LeftEar, RightEar, Nose, Mouth, Head, Neck;

    public HeadController headScalerTop, headScalerBottom, headScalerLeft, headScalerRight;

    public Vector2 mousePos;

    public CharacterData currentChar;

    [Range(-45f, 25)] public float eyeAngle;
    [Range(0f, 1f)] public float eyeSpacing;
    [Range(-1f, .1f)] public float eyePos;
    [Range(0.5f, 1f)] public float eyeWidth, eyeHeight;

    [Range(-45f, 25f)] public float eyebrowAngle;
    [Range(0f, 1f)] public float eyebrowSpacing;
    [Range(-1f, 0.5f)] public float eyebrowHeight;
    [Range(0.2f, 1.5f)] public float eyebrowLength;
    [Range(0.1f, 0.5f)] public float eyebrowWidth;

    [Range(-0.25f, 0.5f)] public float noseHeight;
    [Range(0.5f, 2f)] public float noseLength, noseWidth;

    [Range(0.25f, 2.5f)] public float mouthWidth;
    [Range(0f, 1f)] public float mouthHeight;
    [Range(-1f, 1.25f)] public float mouthPos;
    
    [Range(0f, 1f)] public float neckWidth;

    [Range(0f, 1f)] public float headWidth, headHeight;

    [Range(.5f, 2f)] public float earWidth, earHeight;
    [Range(-0.5f,1f)] public float earPos;

    [Range(-45f, 45f)] public float earAngle;

    [Range(0f, 1f)] public float earSpacing;


    public float eyeRadius;
    public float leftPupilX;
    public float leftPupilY;
    public float rightPupilX;
    public float rightPupilY;

    MaterialPropertyBlock leftProp, rightProp;


    void SetTransformValues(){
        float actualHeadWidth = Mathf.Lerp(2,4, currentChar.headWidth);
        Head.transform.localScale = new Vector3(actualHeadWidth, Mathf.Lerp(2,4, currentChar.headLength), 0);
        
        LeftEye.transform.localEulerAngles = new Vector3(0, 0, currentChar.eyeAngle);
        RightEye.transform.localEulerAngles = new Vector3(0, 0, -currentChar.eyeAngle);
        
        LeftEye.transform.localPosition = new Vector3(((Mathf.Lerp(-0.5f, -1.5f, currentChar.headWidth) - (currentChar.eyeWidth/2f)) * eyeSpacing), 1+currentChar.eyeHeight, 0);
        RightEye.transform.localPosition = new Vector3(((Mathf.Lerp(0.5f, 1.5f, currentChar.headWidth) + (currentChar.eyeWidth/2f)) * eyeSpacing), 1+currentChar.eyeHeight, 0);
        LeftEye.transform.localScale = new Vector3(-currentChar.eyeWidth, currentChar.eyeLength, 1);
        RightEye.transform.localScale = new Vector3(currentChar.eyeWidth, currentChar.eyeLength, 1);

        LeftEyebrow.transform.localEulerAngles = new Vector3(0, 0, currentChar.eyebrowAngle);
        RightEyebrow.transform.localEulerAngles = new Vector3(0, 0, -currentChar.eyebrowAngle);
        LeftEyebrow.transform.localPosition = new Vector3(-0.5f - currentChar.eyebrowSpacing, 1.25f + currentChar.eyebrowHeight, -0.1f);
        RightEyebrow.transform.localPosition = new Vector3(0.5f + currentChar.eyebrowSpacing, 1.25f + currentChar.eyebrowHeight, -0.1f);
        LeftEyebrow.transform.localScale = new Vector3(currentChar.eyebrowLength, currentChar.eyebrowWidth, 1);
        RightEyebrow.transform.localScale = new Vector3(currentChar.eyebrowLength, currentChar.eyebrowWidth, 1);
    
        Nose.transform.localPosition = new Vector3(0, currentChar.noseHeight, -0.1f);
        Nose.transform.localScale = new Vector3(currentChar.noseWidth, currentChar.noseLength, 1);

        LeftEar.transform.localPosition = new Vector3(((Mathf.Lerp(-1.25f, -2.25f, currentChar.headWidth) - (currentChar.earWidth/3f)) * currentChar.earSpacing), currentChar.earHeight, 0.2f);
        RightEar.transform.localPosition = new Vector3(((Mathf.Lerp(1.25f, 2.25f, currentChar.headWidth) + (currentChar.earWidth/3f)) * currentChar.earSpacing), currentChar.earHeight, 0.2f);

        LeftEar.transform.localScale = new Vector3(-currentChar.earWidth,currentChar.earLength,1);
        RightEar.transform.localScale = new Vector3(currentChar.earWidth,currentChar.earLength,1);

        LeftEar.transform.localEulerAngles = new Vector3(0, 0, currentChar.earAngle);
        RightEar.transform.localEulerAngles = new Vector3(0, 0, -currentChar.earAngle);

        Mouth.transform.localScale = new Vector3(currentChar.mouthWidth, currentChar.mouthLength, 1);
        Mouth.transform.localPosition = new Vector3(0, -1 + currentChar.mouthHeight, 0);

        Neck.transform.localScale = new Vector3(Mathf.Lerp(0.5f, actualHeadWidth, currentChar.neckWidth), 2f, 1f);
    }

    void SetShaderValues(){
        MaterialPropertyBlock EyeProp, EyebrowProp, EarProp, NoseProp, MouthProp, HeadProp, NeckProp;
        EyeProp = new MaterialPropertyBlock();
        EyebrowProp = new MaterialPropertyBlock();
        EarProp = new MaterialPropertyBlock();
        NoseProp = new MaterialPropertyBlock();
        MouthProp = new MaterialPropertyBlock();
        HeadProp = new MaterialPropertyBlock();
        NeckProp = new MaterialPropertyBlock();

        HeadProp.SetFloat("_ChinWidth", currentChar.chinWidth);
        HeadProp.SetFloat("_ChinLength", currentChar.chinLength);
        HeadProp.SetFloat("_ForeheadWidth", currentChar.foreheadWidth);
        HeadProp.SetFloat("_ForeheadLength", currentChar.foreheadLength);
        HeadProp.SetColor("_Color1", currentChar.headBottom);
        HeadProp.SetColor("_Color2", currentChar.headTop);

        Head.SetPropertyBlock(HeadProp);
        NeckProp.SetFloat("_NeckTopWidth", currentChar.neckTopWidth);
        NeckProp.SetFloat("_NeckCurveRoundness", currentChar.neckCurveRoundness);
        NeckProp.SetFloat("_NeckCurveScale", currentChar.neckCurveScale);
        NeckProp.SetColor("_Color1", currentChar.neckBottom);
        NeckProp.SetColor("_Color2", currentChar.neckTop);

        Neck.SetPropertyBlock(NeckProp);

    }

    void Start(){
        LoadCharacterData();
    }

    void LoadCharacterData(){
        SetTransformValues();
        SetShaderValues();
    }
    
    [ContextMenu("Random")]
    public void AllRandom(){
        RandomHeadShape();
        RandomNeckShape();
        RandomEarShape();
        RandomEyeShape();
        RandomNoseShape();
        RandomEyebrowShape();
        RandomMouthShape();
    }

    void RandomHeadShape(){

        MaterialPropertyBlock prop = new MaterialPropertyBlock();

        float rad1 = Random.Range(0.1f, 5f);
        float rad2 = Random.Range(0.1f, 5f);
        float rad3 = Random.Range(0.1f, 5f);
        float rad4 = Random.Range(0.1f, 5f);
        Color col1 = Random.ColorHSV();
        Color col2 = Random.ColorHSV();
        
        prop.SetFloat("_Radius3", rad1);
        prop.SetFloat("_Radius4", rad2);
        prop.SetFloat("_Radius5", rad3);
        prop.SetFloat("_Radius6", rad4);
        prop.SetColor("_Color", col1);
        prop.SetColor("_Color2", col2);

        Head.SetPropertyBlock(prop);
    }

    void RandomHeadTransform(){
        headWidth = Random.Range(0f,1f);
        headHeight = Random.Range(0f,1f);
    }

    void RandomNeckShape(){
        MaterialPropertyBlock prop = new MaterialPropertyBlock();

        float width = Random.Range(1f, 5f);
        float radius = Random.Range(0f, 3f);
        float scale = Random.Range(1f,5f);

        prop.SetFloat("_Width", width);
        prop.SetFloat("_Radius", radius);
        prop.SetFloat("_NeckScale", scale);

        Neck.SetPropertyBlock(prop);
    }

    void RandomEarShape(){
        MaterialPropertyBlock prop = new MaterialPropertyBlock();

        float scale1 = Random.Range(0f, 1f);
        float scale2 = Random.Range(-1f, 1f);
        float scale3 = Random.Range(0.5f, 1.25f);
        float scale4 = Random.Range(1f, 6f);
        float scale5 = Random.Range(0f, 1f);
        float scale6 = Random.Range(0f, 1f);
        float scale8 = Random.Range(1f, 1.5f);
        float scale9 = Random.Range(1f, 1.5f);

        prop.SetFloat("_Scale1", scale1);
        prop.SetFloat("_Scale2", scale2);
        prop.SetFloat("_Scale3", scale3);
        prop.SetFloat("_Scale4", scale4);
        prop.SetFloat("_Scale5", scale5);
        prop.SetFloat("_Scale6", scale6);
        prop.SetFloat("_Scale8", scale8);
        prop.SetFloat("_Scale9", scale9);
       
        LeftEar.SetPropertyBlock(prop);
        RightEar.SetPropertyBlock(prop);
    }

    void RandomMouthShape(){
        MaterialPropertyBlock prop = new MaterialPropertyBlock();

        float radius = Random.Range(0.2f, 1f);
        float xScale = Random.Range(0f, 1f);
        float yScaleUp = Random.Range(-1f, 1f);
        float yScaleDown = Random.Range(-1f, 1f);

        float topTeeth = Random.Range(0f, 1f);
        float bottomTeeth = Random.Range(0f, 1f);

        prop.SetFloat("_Radius", radius);
        prop.SetFloat("_xScaleUpper", xScale);
        prop.SetFloat("_yScaleUpper", yScaleUp);
        prop.SetFloat("_yScaleUpper2", yScaleDown);
        prop.SetFloat("_TopTeeth", topTeeth);
        prop.SetFloat("_BotTeeth", bottomTeeth);

        Mouth.SetPropertyBlock(prop);



    }

    void RandomMouthTransform(){
        mouthWidth = Random.Range(0.25f, 2.5f);
        mouthHeight = Random.Range(0f, 1f);
    }

    void RandomEyeShape(){
        leftProp = new MaterialPropertyBlock();
        rightProp = new MaterialPropertyBlock();

        eyeRadius = Random.Range(0.1f, 1f);
        float xPupil = Random.Range(0.1f, 1f);
        float yPupil = Random.Range(0.1f, 1f);
        float pupil = Random.Range(0f, 1f);

        float xUpper = Random.Range(0f, 2.75f);
        float yUpper = Random.Range(0f, 1f);
        float xLower = Random.Range(0f, 2.75f);
        float yLower = Random.Range(0f, 1f);

        float lid1 = Random.Range(0f, 1f);
        float lid2 = Random.Range(0f, 1f);

        float squash = Random.Range(0.25f, 1f);

        leftProp.SetFloat("_Radius", eyeRadius);
        leftProp.SetFloat("_xPupil", xPupil);
        leftProp.SetFloat("_yPupil", yPupil);
        leftProp.SetFloat("_radiusPupil", pupil);
        
        leftProp.SetFloat("_xLevel", xUpper);
        leftProp.SetFloat("_yLevel", yUpper);
        leftProp.SetFloat("_xLevel3", xLower);
        leftProp.SetFloat("_yLevel3", yLower);
        leftProp.SetFloat("_Lid1", lid1);
        leftProp.SetFloat("_Lid2", lid2);
        leftProp.SetFloat("_SquashPupil", squash);

        rightProp.SetFloat("_Radius", eyeRadius);
        rightProp.SetFloat("_xPupil", xPupil);
        rightProp.SetFloat("_yPupil", yPupil);
        rightProp.SetFloat("_radiusPupil", pupil);
        
        rightProp.SetFloat("_xLevel", xUpper);
        rightProp.SetFloat("_yLevel", yUpper);
        rightProp.SetFloat("_xLevel3", xLower);
        rightProp.SetFloat("_yLevel3", yLower);
        rightProp.SetFloat("_Lid1", lid1);
        rightProp.SetFloat("_Lid2", lid2);
        rightProp.SetFloat("_SquashPupil", squash);


    }

    void RandomEyeTransform(){

        eyeAngle = Random.Range(-45f, 25);
        eyeSpacing = Random.Range(0f, 1f);
        eyePos = Random.Range(-1f, .1f);
        eyeWidth = Random.Range(0.5f, 1f);
        eyeHeight = Random.Range(0.5f, 1f);
        SetTransformValues();
    }

    void RandomEyebrowShape(){
        MaterialPropertyBlock prop = new MaterialPropertyBlock();

        float strands = Random.Range(1, 16);
        float offset = Random.Range(1, 8);
        float curve = Random.Range(0.3f, 4f);
        float curve2 = Random.Range(-1f, 1f);

        prop.SetFloat("_Strokes", strands);
        prop.SetFloat("_Offset", offset);
        prop.SetFloat("_Curve", curve);
        prop.SetFloat("_Curve2", curve2);

        LeftEyebrow.SetPropertyBlock(prop);
        RightEyebrow.SetPropertyBlock(prop);


    }

    void RandomEyebrowTransform(){
         eyebrowAngle = Random.Range(-45f, 25f);
        eyebrowSpacing = Random.Range(0f, 1f);
        eyebrowHeight = Random.Range(-.1f, 0.5f);
        eyebrowLength = Random.Range(0.2f, 1.5f);
        eyebrowWidth = Random.Range(0.25f, 1f);

        SetTransformValues();
    }

    void RandomNoseShape(){
        MaterialPropertyBlock prop = new MaterialPropertyBlock();

        float xScale1 = Random.Range(0.1f, 2.5f);
        float xScale2 = Random.Range(0.1f, 2.5f);
        float yScale1 = Random.Range(1f, 5f);
        float yScale2 = Random.Range(1f, 5f);
        float blend = Random.Range(0.1f, 2f);

        float radius = Random.Range(0.1f, 1f);
        float nostrilSpace = Random.Range(1f, 2f);
        float nostrilHeight = Random.Range(-0.5f, -0.25f);
        float nostrilScale = Random.Range(0f, 2f);
        

        prop.SetFloat("_xScaleUpper", xScale1);
        prop.SetFloat("_yScaleUpper", xScale2);
        prop.SetFloat("_xScaleUpper2", yScale1);
        prop.SetFloat("_yScaleUpper2", yScale2);
        prop.SetFloat("_Blend", blend);
        prop.SetFloat("_Radius", radius);
        prop.SetFloat("_NostrilSpacing", nostrilSpace);
        prop.SetFloat("_NostrilHeight", nostrilHeight);
        prop.SetFloat("_NostrilScale", nostrilScale);

        Nose.SetPropertyBlock(prop);

    }

    void RandomNoseTransform(){
        noseHeight = Random.Range(-0.25f, 0.5f);
        noseLength = Random.Range(0.5f, 2f);
        noseWidth = Random.Range(0.5f, 2f);
        SetTransformValues();
    }

    void Update(){
        mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        
        if(Input.GetMouseButtonDown(0)){
            //AllRandom();
        }
        eyeRadius = 1f;
        leftPupilX = Mathf.Lerp(leftPupilX, Mathf.Clamp(LeftEye.transform.localPosition.x - mousePos.x, -0.3f*eyeRadius, 0.3f*eyeRadius), Time.deltaTime*5);
        leftPupilY = Mathf.Lerp(leftPupilY, Mathf.Clamp(LeftEye.transform.localPosition.y - mousePos.y, -0.2f*eyeRadius, 0.2f*eyeRadius), Time.deltaTime*5);
        rightPupilX = Mathf.Lerp(rightPupilX, Mathf.Clamp(RightEye.transform.localPosition.x - mousePos.x, -0.3f*eyeRadius, 0.3f*eyeRadius), Time.deltaTime*5);
        rightPupilY = Mathf.Lerp(rightPupilY, Mathf.Clamp(RightEye.transform.localPosition.y - mousePos.y, -0.2f*eyeRadius, 0.2f*eyeRadius), Time.deltaTime*5);

        leftProp.SetFloat("_pupilOffsetX", -leftPupilX);
        leftProp.SetFloat("_pupilOffsetY", leftPupilY);

        rightProp.SetFloat("_pupilOffsetX", rightPupilX);
        rightProp.SetFloat("_pupilOffsetY", rightPupilY);

        LeftEye.SetPropertyBlock(leftProp);
        RightEye.SetPropertyBlock(rightProp);
        
        
        
        /*
        MaterialPropertyBlock prop = new MaterialPropertyBlock();
        float x = Mathf.PerlinNoise(Time.time*0.2f,0.1f);
        float y = Mathf.PerlinNoise(Time.time*.1f,0.2f);
        float z = Mathf.PerlinNoise(Time.time*0.3f,0.3f);
        float a = Mathf.PerlinNoise(Time.time*.5f,0.4f);
        float b = Mathf.PerlinNoise(Time.time*0.1f,0.5f);
        float c = Mathf.PerlinNoise(Time.time*0.6f,0.6f);
        float d = Mathf.PerlinNoise(Time.time*.2f,0.7f);
        float e = Mathf.PerlinNoise(Time.time*0.1f,0.8f);

        float f = Mathf.PerlinNoise(Time.time*.25f,0.8f);
        float g = Mathf.PerlinNoise(Time.time*0.35f,0.2f);
        float h = Mathf.PerlinNoise(Time.time*.15f,0.3f);
        float i = Mathf.PerlinNoise(Time.time*0.71f,0.4f);
        float j = Mathf.PerlinNoise(Time.time*0.36f,0.5f);

        float k = Mathf.PerlinNoise(Time.time*0.16f,0.7f);
        float l = Mathf.PerlinNoise(Time.time*0.47f,0.8f);

        float radius = (x*0.9f) + 0.1f;
        float xUpper = y*2f;
        float yUpper = z;
        float xLower = a*2;
        float yLower = b;
        
        float xPupil = c;
        float yPupil = d;
        float pupil = e;
        
        

        //eyeAngle = (f*70) - 45;
       // eyeSpacing = g;
        //eyePos = (h*0.2f) - 0.1f;
        //eyeWidth = (i*0.5f) + 0.5f;
        //eyeHeight = (j*0.5f) + 0.5f;

        float lid1 = k*2;
        float lid2 = l*2;


        prop.SetFloat("_xPupil", xPupil);
        prop.SetFloat("_yPupil", yPupil);
        prop.SetFloat("_radiusPupil", pupil);
        prop.SetFloat("_Radius", 1f);
        prop.SetFloat("_xLevel", xUpper);
        prop.SetFloat("_yLevel", yUpper);
        prop.SetFloat("_xLevel3", xLower);
        prop.SetFloat("_yLevel3", yLower);
        prop.SetFloat("_Lid1", lid1);
        prop.SetFloat("_Lid2", lid2);
*/
        //LeftEye.SetPropertyBlock(prop);
        //RightEye.SetPropertyBlock(prop);

        

        //SetTransformValues();

    }

    public void UpdateHeightFromTransformTool(float valueChange){
        headHeight = valueChange;
        headScalerTop.UpdatePosition(valueChange);
        headScalerBottom.UpdatePosition(valueChange);
        SetTransformValues();
    }

    public void UpdateWidthFromTransformTool(float valueChange){
        headWidth = valueChange;
        headScalerLeft.UpdatePosition(valueChange);
        headScalerRight.UpdatePosition(valueChange);
        SetTransformValues();
    }
    
}
