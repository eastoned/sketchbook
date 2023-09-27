using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EyeData", menuName = "ScriptableObjects/Eye", order = 3)]
public class EyeData : PartData{

    public override void SetPositionBounds(PartData parentData)
    {
        minPosX = GetAbsoluteScale().x/2f;
        maxPosX = parentData.GetAbsoluteScale().x/2f;
        minPosY = -parentData.GetAbsoluteScale().y/2f + GetAbsoluteScale().y/2f;
        maxPosY = parentData.GetAbsoluteScale().y/2f + GetAbsoluteScale().y/2f;
    }

    public override void SetPositionBounds(){
        minPosX = GetAbsoluteScale().x/2f;
    }

    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.GetAbsoluteScale().x/2f;
        maxScaleY = parentData.GetAbsoluteScale().y/2f;
    }

    public List<ShaderProperty> reactiveProperties = new List<ShaderProperty>();
    
}