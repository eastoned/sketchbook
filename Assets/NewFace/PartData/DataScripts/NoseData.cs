using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoseData", menuName = "ScriptableObjects/Nose", order = 6)]
public class NoseData : PartData
{
    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.maxPosX*2f;
        maxScaleY = parentData.maxPosY*2f;
    }

    public override void SetPositionBounds(PartData parentData)
    {
        minPosY = parentData.minPosY;
        maxPosY = parentData.GetAbsolutePosition().y;
    }
}
