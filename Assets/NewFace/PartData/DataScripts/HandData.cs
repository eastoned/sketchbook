using UnityEngine;

[CreateAssetMenu(fileName = "HandData", menuName = "ScriptableObjects/Hand", order = 10)]
public class HandData : PartData{
    public override void SetPositionBounds(PartData parentData)
    {
        minPosX = GetAbsoluteScale().x/2f;
        maxPosX = parentData.GetAbsoluteScale().x/2f + GetAbsoluteScale().x/2f;
        minPosY = parentData.GetAbsolutePosition().y - parentData.GetAbsoluteScale().y/2f;
        maxPosY = parentData.GetAbsolutePosition().y + parentData.GetAbsoluteScale().y/2f;
    }

    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.GetAbsoluteScale().x;
        maxScaleY = parentData.GetAbsoluteScale().y;
    }
}
