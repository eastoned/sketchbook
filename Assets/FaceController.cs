using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    [SerializeField] public Renderer LeftEye, RightEye, LeftEyebrow, RightEyebrow, LeftEar, RightEar, Nose, Mouth, Head, Neck;

    public HeadController headScalerTop, headScalerBottom, headScalerLeft, headScalerRight;

    public Collider headtop, headbottom;

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
    [Range(-1f, 1f)] public float mouthPos;
    
    [Range(0f, 1f)] public float neckWidth;

    [Range(0f, 1f)] public float headWidth, headHeight;

    [Range(.5f, 2f)] public float earWidth, earHeight;
    [Range(-0.5f,1f)] public float earPos;

    [Range(0f, 45f)] public float earAngle;

    void OnValidate(){
        SetTransformValues();
        
    }


    void SetTransformValues(){
        float actualHeadWidth = Mathf.Lerp(2,4,headWidth);
        Head.transform.localScale = new Vector3(actualHeadWidth, Mathf.Lerp(2,4,headHeight), 0);
        
        LeftEye.transform.localEulerAngles = new Vector3(0, 0, eyeAngle);
        RightEye.transform.localEulerAngles = new Vector3(0, 0, -eyeAngle);
        LeftEye.transform.localPosition = new Vector3(-0.5f - eyeSpacing, 1+eyePos, 0);
        RightEye.transform.localPosition = new Vector3(0.5f + eyeSpacing, 1+eyePos, 0);
        LeftEye.transform.localScale = new Vector3(-eyeWidth, eyeHeight, 1);
        RightEye.transform.localScale = new Vector3(eyeWidth, eyeHeight, 1);

        LeftEyebrow.transform.localEulerAngles = new Vector3(0, 0, eyebrowAngle);
        RightEyebrow.transform.localEulerAngles = new Vector3(0, 0, -eyebrowAngle);
        LeftEyebrow.transform.localPosition = new Vector3(-0.5f - eyebrowSpacing, 1.25f + eyebrowHeight, -0.1f);
        RightEyebrow.transform.localPosition = new Vector3(0.5f + eyebrowSpacing, 1.25f + eyebrowHeight, -0.1f);
        LeftEyebrow.transform.localScale = new Vector3(eyebrowLength, eyebrowWidth, 1);
        RightEyebrow.transform.localScale = new Vector3(eyebrowLength, eyebrowWidth, 1);

        Nose.transform.localPosition = new Vector3(0, noseHeight, -0.1f);
        Nose.transform.localScale = new Vector3(noseWidth, noseLength, 1);

        LeftEar.transform.localPosition = new Vector3(Mathf.Lerp(-1.25f, -2.25f, headWidth), earPos, 0.2f);
        RightEar.transform.localPosition = new Vector3(Mathf.Lerp(1.25f, 2.25f, headWidth), earPos, 0.2f);

        LeftEar.transform.localScale = new Vector3(-earWidth,earHeight,1);
        RightEar.transform.localScale = new Vector3(earWidth,earHeight,1);

        LeftEar.transform.localEulerAngles = new Vector3(0, 0, earAngle);
        RightEar.transform.localEulerAngles = new Vector3(0, 0, -earAngle);

        Mouth.transform.localScale = new Vector3(mouthWidth, mouthHeight, 1);
        Mouth.transform.localPosition = new Vector3(0, -1 + mouthPos, 0);

        Neck.transform.localScale = new Vector3(Mathf.Lerp(0.5f, actualHeadWidth, neckWidth), 2, 1);
    }
    
    [ContextMenu("Random")]
    public void AllRandom(){
        RandomHead();
        RandomNeck();
        RandomEar();
        RandomEye();
        RandomNose();
        RandomEyebrow();
        RandomMouth();
    }

    void RandomHead(){
        MaterialPropertyBlock prop = new MaterialPropertyBlock();

        float rad1 = Random.Range(0.1f, 5f);
        float rad2 = Random.Range(0.1f, 5f);
        float rad3 = Random.Range(0.1f, 5f);
        float rad4 = Random.Range(0.1f, 5f);

        prop.SetFloat("_Radius3", rad1);
        prop.SetFloat("_Radius4", rad2);
        prop.SetFloat("_Radius5", rad3);
        prop.SetFloat("_Radius6", rad4);

        Head.SetPropertyBlock(prop);

        headWidth = Random.Range(0f,1f);
        headHeight = Random.Range(0f,1f);
    }

    void RandomNeck(){
        MaterialPropertyBlock prop = new MaterialPropertyBlock();
    }

    void RandomEar(){
        MaterialPropertyBlock prop = new MaterialPropertyBlock();
    }

    void RandomMouth(){
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

        mouthWidth = Random.Range(0.25f, 2.5f);
        mouthHeight = Random.Range(0f, 1f);


    }

    void RandomEye(){
        MaterialPropertyBlock prop = new MaterialPropertyBlock();

        float radius = Random.Range(0.1f, 1f);
        float xUpper = Random.Range(0f, 2f);
        float yUpper = Random.Range(0f, 1f);
        float xLower = Random.Range(0f, 2f);
        float yLower = Random.Range(0f, 1f);
        float xPupil = Random.Range(0f, 1f);
        float yPupil = Random.Range(0f, 1f);
        float pupil = Random.Range(0f, 1f);

        eyeAngle = Random.Range(-45f, 25);
        eyeSpacing = Random.Range(0f, 1f);
        eyePos = Random.Range(-1f, .1f);
        eyeWidth = Random.Range(0.5f, 1f);
        eyeHeight = Random.Range(0.5f, 1f);

        prop.SetFloat("_Radius", 1f);
        prop.SetFloat("_xLevel", xUpper);
        prop.SetFloat("_yLevel", yUpper);
        prop.SetFloat("_xLevel3", xLower);
        prop.SetFloat("_yLevel3", yLower);
        prop.SetFloat("_xPupil", xPupil);
        prop.SetFloat("_yPupil", yPupil);
        prop.SetFloat("_radiusPupil", pupil);

        LeftEye.SetPropertyBlock(prop);
        RightEye.SetPropertyBlock(prop);

        SetTransformValues();
        
    }

    void RandomEyebrow(){
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

        eyebrowAngle = Random.Range(-45f, 25f);
        eyebrowSpacing = Random.Range(0f, 1f);
        eyebrowHeight = Random.Range(-.1f, 0.5f);
        eyebrowLength = Random.Range(0.2f, 1.5f);
        eyebrowWidth = Random.Range(0.25f, 1f);

        SetTransformValues();
    }

    void RandomNose(){
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
        noseHeight = Random.Range(-0.25f, 0.5f);
        noseLength = Random.Range(0.5f, 2f);
        noseWidth = Random.Range(0.5f, 2f);

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

        SetTransformValues();

    }

    void Update(){
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

        LeftEye.SetPropertyBlock(prop);
        RightEye.SetPropertyBlock(prop);

        

        SetTransformValues();

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
