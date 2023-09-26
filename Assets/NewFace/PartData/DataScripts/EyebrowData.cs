using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EyebrowData", menuName = "ScriptableObjects/Eyebrow", order = 7)]
public class EyebrowData : PartData
{
    public override void SetPositionBounds(PartData parentData)
    {
        minPosX = GetAbsoluteScale().x/2f;
        maxPosX = parentData.maxPosX;
        minPosY = parentData.GetAbsolutePosition().y + parentData.GetAbsoluteScale().y/2f;
        maxPosY = parentData.maxPosY + parentData.GetAbsoluteScale().y/2f;

        //SetRelativePos(absolutePosition);
    }

    public override void SetPositionBounds(){
        minPosX = GetAbsoluteScale().x/2f;
    }

    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.maxScaleX;
         
    }
}
