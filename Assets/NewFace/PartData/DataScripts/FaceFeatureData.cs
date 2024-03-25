using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FaceFeatureList", menuName = "ScriptableObjects/FaceFeatures", order = 10)]
public class FaceFeatureData : ScriptableObject
{

    public enum Expression
    {
        NEUTRAL,
        HAPPY,
        SAD,
        ANGRY,
        SURPRISE,
        SCARED
    }
    
    public Expression currentExpression;

    public bool canSee = true;
    public bool canSpeak = true;
    public bool canSmell = true;
    public bool canHear = true;

    public void UpdateVisionStatus(PartController eye)
    {
        float eyeRadius = eye.GetSingleShaderFloat("_PupilRadius");
        float eyeOpen = eye.GetSingleShaderFloat("_EyelidBottomOpen") + eye.GetSingleShaderFloat("_EyelidTopOpen");
        canSee = eyeRadius > 0.05f && eyeOpen > 0.05f;
    }

    public void UpdateHearingStatus(PartController ear)
    {
        canHear = true;
    }

}
