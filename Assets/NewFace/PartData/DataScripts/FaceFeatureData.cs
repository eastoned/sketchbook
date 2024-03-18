using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FaceFeatureList", menuName = "ScriptableObjects/FaceFeatures", order = 10)]
public class FaceFeatureData : ScriptableObject
{
    public bool canSee = true;
    public bool canSpeak = true;
    public bool canSmell = true;
    public bool canHear = true;

    public void UpdateVision(PartController eye){
        float eyeRadius = eye.GetSingleShaderFloat("_PupilRadius");
        float eyeOpen = eye.GetSingleShaderFloat("_EyelidBottomOpen") + eye.GetSingleShaderFloat("_EyelidTopOpen");
        canSee = eyeRadius > 0.05f && eyeOpen > 0.05f;
    }
            
}
