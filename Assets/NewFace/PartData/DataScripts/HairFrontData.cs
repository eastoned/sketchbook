using UnityEngine;

[CreateAssetMenu(fileName = "HairFrontData", menuName = "ScriptableObjects/HairFront", order = 8)]
public class HairFrontData : PartData
{
    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.GetAbsoluteScale().x + GetAbsoluteScale().x/4f;
        maxScaleY = parentData.GetAbsoluteScale().y/2f;
    }

    public override void SetPositionBounds(PartData parentData)
    {
        minPosY = parentData.GetAbsolutePosition().y - parentData.GetAbsoluteScale().y/2f;
        maxPosY = parentData.GetAbsolutePosition().y + parentData.GetAbsoluteScale().y/2f;
    }
}
