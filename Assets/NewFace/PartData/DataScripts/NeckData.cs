using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NeckData", menuName = "ScriptableObjects/Neck", order = 2)]
public class NeckData : PartData{

    public override void SetScaleBounds()
    {
        //maxScaleX = parentData.GetAbsoluteScale().x;
        //minScaleY = Math.Abs(parentData.GetAbsolutePosition().y + 2f);
        //maxScaleY = Math.Abs(parentData.GetAbsolutePosition().y + 2f);
    }

    public override void SetPositionBounds()
    {
        Debug.Log("nexk " + GetAbsoluteScale().y);
        minPosY = -2f + GetAbsoluteScale().y/2f;
        maxPosY = -2f + GetAbsoluteScale().y/2f;
    }

}