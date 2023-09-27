using UnityEngine;

[CreateAssetMenu(fileName = "EarData", menuName = "ScriptableObjects/Ear", order = 6)]
public class EarData : PartData{
    public override void SetPositionBounds(PartData parentData)
    {
        minPosX = GetAbsoluteScale().x/2f;
        maxPosX = parentData.GetAbsoluteScale().x/2f + GetAbsoluteScale().x/2f;
        minPosY = -parentData.GetAbsoluteScale().y/2f;
        maxPosY = parentData.GetAbsoluteScale().y/2f + GetAbsoluteScale().x/2f;
    }

    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.GetAbsoluteScale().x;
        maxScaleY = parentData.GetAbsoluteScale().y;
    }
}
