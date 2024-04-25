using UnityEngine;

[CreateAssetMenu(fileName = "HairBackData", menuName = "ScriptableObjects/HairBack", order = 9)]
public class HairBackData : PartData
{
    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.GetAbsScale().x*2f;
        maxScaleY = parentData.GetAbsScale().y*2f;
    }

    public override void SetPositionBounds(PartData parentData)
    {
        minPosY = parentData.GetAbsPosition().y - parentData.GetAbsScale().y/2f;
        maxPosY = parentData.GetAbsPosition().y + parentData.GetAbsScale().y/2f;
    }
}
