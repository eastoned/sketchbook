using UnityEngine;

[CreateAssetMenu(fileName = "HandData", menuName = "ScriptableObjects/Hand", order = 10)]
public class HandData : PartData{
    public override void SetPositionBounds(PartData parentData)
    {
        minPosX = -parentData.GetAbsScale().x/2f - GetAbsScale().x/2f;
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
