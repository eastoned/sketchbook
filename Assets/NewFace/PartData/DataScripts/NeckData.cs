using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NeckData", menuName = "ScriptableObjects/Neck", order = 2)]
public class NeckData : PartData{

    public override void SetScaleBounds(PartData parentBounds)
    {
        //maxScaleX = parentData.GetAbsoluteScale().x;
        //minScaleY = Math.Abs(parentData.GetAbsolutePosition().y + 2f);
        //maxScaleY = Math.Abs(parentData.GetAbsolutePosition().y + 2f);
    }

    public override void SetPositionBounds(PartData parentBounds)
    {
        Debug.Log("parent setting bounds on neck");
        //minPosY = GetAbsoluteScale().y/2f;
        //maxPosY = -2f + GetAbsoluteScale().y/2f;
    }

    public override void SetPositionBounds(){
        //Debug.Log("no parent setting bounds on neck");
        //relativeToParentPosition
        //minPosY = GetAbsScale().y/2f-2f;
        //maxPosY = GetAbsoluteScale().y/2f-2f;
    }

}