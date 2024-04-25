using UnityEngine;

[CreateAssetMenu(fileName = "EarData", menuName = "ScriptableObjects/Ear", order = 6)]
public class EarData : PartData{
    public override void SetPositionBounds(PartData parentData)
    {
        minPosX = GetAbsScale().x/2f;
        maxPosX = parentData.GetAbsScale().x/2f + GetAbsScale().x/2f;
        minPosY = parentData.GetAbsPosition().y - parentData.GetAbsScale().y/2f;
        maxPosY = parentData.GetAbsPosition().y + parentData.GetAbsScale().y/2f;
    }

    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.GetAbsScale().x;
        maxScaleY = parentData.GetAbsScale().y;
    }
}
