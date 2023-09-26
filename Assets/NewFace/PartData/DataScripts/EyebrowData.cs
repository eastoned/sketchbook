using UnityEngine;

[CreateAssetMenu(fileName = "EyebrowData", menuName = "ScriptableObjects/Eyebrow", order = 7)]
public class EyebrowData : PartData
{
    public override void SetPositionBounds(PartData parentData)
    {
        minPosX = GetAbsoluteScale().x/2f;
        maxPosX = parentData.GetAbsoluteScale().x/3f;
        minPosY = -parentData.GetAbsoluteScale().y/2f + GetAbsoluteScale().y/2f;
        maxPosY = parentData.GetAbsoluteScale().y/2f + GetAbsoluteScale().y/2f;

    }

    public override void SetPositionBounds(){
        minPosX = GetAbsoluteScale().x/2f;
    }

    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.maxScaleX;
         
    }
}
