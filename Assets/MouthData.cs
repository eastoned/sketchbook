using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MouthData", menuName = "ScriptableObjects/Mouth", order = 8)]
public class MouthData : PartData
{
   public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.maxScaleX;
        maxScaleY = parentData.maxScaleY/2f;
    }

    public override void SetPositionBounds(PartData parentData)
    {
        minPosY = parentData.minPosY;
        maxPosY = parentData.GetAbsolutePosition().y;
    }
}
