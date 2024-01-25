using UnityEngine;

[CreateAssetMenu(fileName = "HairBackData", menuName = "ScriptableObjects/HairBack", order = 9)]
public class HairBackData : PartData
{
    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.GetAbsoluteScale().x*2f;
        maxScaleY = parentData.GetAbsoluteScale().y*2f;
    }

    public override void SetPositionBounds(PartData parentData)
    {
        minPosY = parentData.GetAbsolutePosition().y - parentData.GetAbsoluteScale().y/2f;
        maxPosY = parentData.GetAbsolutePosition().y + parentData.GetAbsoluteScale().y/2f;
    }
}
