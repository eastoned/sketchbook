using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeckData", menuName = "ScriptableObjects/Neck", order = 4)]
public class NeckData : PartData{
    public override void RelativeScale(Vector3 parentScale)
    {
        Debug.Log("Scaling neck relative to: " + parentScale);
        maxScaleX = parentScale.x;
        base.RelativeScale(parentScale);
    }
}