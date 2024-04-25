using UnityEngine;

[CreateAssetMenu(fileName = "HairFrontData", menuName = "ScriptableObjects/HairFront", order = 8)]
public class HairFrontData : PartData
{
    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.GetAbsScale().x + GetAbsScale().x/4f;
        maxScaleY = parentData.GetAbsScale().y/2f;
    }

    public override void SetPositionBounds(PartData parentData)
    {
        minPosY = parentData.GetAbsPosition().y - parentData.GetAbsScale().y/2f;
        maxPosY = parentData.GetAbsPosition().y + parentData.GetAbsScale().y/2f;
    }
}
