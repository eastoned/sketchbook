using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NeckData", menuName = "ScriptableObjects/Neck", order = 2)]
public class NeckData : PartData{

    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.GetAbsoluteScale().x;
        minScaleY = Math.Abs(parentData.GetAbsolutePosition().y + 2f);
        maxScaleY = Math.Abs(parentData.GetAbsolutePosition().y + 2f);
    }

    public override void SetPositionBounds(PartData parentData)
    {
        minPosY = Mathf.Lerp(-2f, parentData.GetAbsolutePosition().y, 0.5f);
        maxPosY = Mathf.Lerp(-2f, parentData.GetAbsolutePosition().y, 0.5f);
    }

}