using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    [SerializeField] public Renderer LeftEye, RightEye, LeftEyebrow, RightEyebrow, LeftEar, RightEar, Nose, Mouth, Head, Neck;

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

    void OnValidate(){
        SetTransformValues();
    }

    void SetTransformValues(){
        LeftEye.transform.localEulerAngles = new Vector3(0, 0, eyeAngle);
        RightEye.transform.localEulerAngles = new Vector3(0, 0, -eyeAngle);
        LeftEye.transform.localPosition = new Vector3(-0.5f - eyeSpacing, 1+eyePos, 0);
        RightEye.transform.localPosition = new Vector3(0.5f + eyeSpacing, 1+eyePos, 0);
        LeftEye.transform.localScale = new Vector3(eyeWidth, eyeHeight, 1);
        RightEye.transform.localScale = new Vector3(eyeWidth, eyeHeight, 1);

        LeftEyebrow.transform.localEulerAngles = new Vector3(0, 0, eyebrowAngle);
        RightEyebrow.transform.localEulerAngles = new Vector3(0, 0, -eyebrowAngle);
        LeftEyebrow.transform.localPosition = new Vector3(-0.5f - eyebrowSpacing, 1.25f + eyebrowHeight, -0.1f);
        RightEyebrow.transform.localPosition = new Vector3(0.5f + eyebrowSpacing, 1.25f + eyebrowHeight, -0.1f);
        LeftEyebrow.transform.localScale = new Vector3(eyebrowLength, eyebrowWidth, 1);
        RightEyebrow.transform.localScale = new Vector3(eyebrowLength, eyebrowWidth, 1);

        Nose.transform.localPosition = new Vector3(0, noseHeight, -0.1f);
        Nose.transform.localScale = new Vector3(noseWidth, noseLength, 1);

        Mouth.transform.localScale = new Vector3(mouthWidth, mouthHeight, 1);
    }
    
    [ContextMenu("Random")]
    public void AllRandom(){
        RandomEye();
        RandomNose();
        RandomEyebrow();
        RandomMouth();
    }
    void RandomMouth(){
        mouthWidth = Random.Range(0.25f, 2.5f);
        mouthHeight = Random.Range(0f, 1f);
    }

    void RandomEye(){
        MaterialPropertyBlock prop = new MaterialPropertyBlock();

        float radius = Random.Range(0.1f, 1f);
        float xUpper = Random.Range(0f, 1f);
        float yUpper = Random.Range(0f, 1f);
        float xLower = Random.Range(0f, 1f);
        float yLower = Random.Range(0f, 1f);
        float xPupil = Random.Range(0f, 1f);
        float yPupil = Random.Range(0f, 1f);
        float pupil = Random.Range(0f, 1f);

        eyeAngle = Random.Range(-45f, 25);
        eyeSpacing = Random.Range(0f, 1f);
        eyePos = Random.Range(-1f, .1f);
        eyeWidth = Random.Range(0.5f, 1f);
        eyeHeight = Random.Range(0.5f, 1f);

        prop.SetFloat("_Radius", radius);
        prop.SetFloat("_xScaleUpper", xUpper);
        prop.SetFloat("_yScaleUpper", yUpper);
        prop.SetFloat("_xScaleLower", xLower);
        prop.SetFloat("_yScaleLower", yLower);
        prop.SetFloat("_xPupil", xPupil);
        prop.SetFloat("_yPupil", yPupil);
        prop.SetFloat("_radiusPupil", pupil);

        prop.SetFloat("_Radius", radius);
        prop.SetFloat("_xScaleUpper", xUpper);
        prop.SetFloat("_yScaleUpper", yUpper);
        prop.SetFloat("_xScaleLower", xLower);
        prop.SetFloat("_yScaleLower", yLower);
        prop.SetFloat("_xPupil", xPupil);
        prop.SetFloat("_yPupil", yPupil);
        prop.SetFloat("_radiusPupil", pupil);

        LeftEye.SetPropertyBlock(prop);
        RightEye.SetPropertyBlock(prop);

        SetTransformValues();
        
    }

    void RandomEyebrow(){

        eyebrowAngle = Random.Range(-45f, 25f);
        eyebrowSpacing = Random.Range(0f, 1f);
        eyebrowHeight = Random.Range(-.1f, 0.5f);
        eyebrowLength = Random.Range(0.2f, 1.5f);
        eyebrowWidth = Random.Range(0.1f, 0.5f);

        SetTransformValues();
    }

    void RandomNose(){
        MaterialPropertyBlock prop = new MaterialPropertyBlock();

        float xScale1 = Random.Range(0.1f, 2.5f);
        float xScale2 = Random.Range(0.1f, 2.5f);
        float yScale1 = Random.Range(1f, 5f);
        float yScale2 = Random.Range(1f, 5f);
        float blend = Random.Range(0.1f, 2f);
        
        noseHeight = Random.Range(-0.25f, 0.5f);
        noseLength = Random.Range(0.5f, 2f);
        noseWidth = Random.Range(0.5f, 2f);

        prop.SetFloat("_xScaleUpper", xScale1);
        prop.SetFloat("_yScaleUpper", xScale2);
        prop.SetFloat("_xScaleUpper2", yScale1);
        prop.SetFloat("_yScaleUpper2", yScale2);
        prop.SetFloat("_Blend", blend);

        Nose.SetPropertyBlock(prop);

        SetTransformValues();

    }
    
}
