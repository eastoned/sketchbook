using UnityEngine;

[CreateAssetMenu(fileName = "NoseData", menuName = "ScriptableObjects/Nose", order = 4)]
public class NoseData : PartData
{
    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.GetAbsoluteScale().x;
        maxScaleY = parentData.GetAbsoluteScale().y;
    }

    public override void SetPositionBounds(PartData parentData)
    {
        minPosY = -parentData.GetAbsoluteScale().y/2f;
        maxPosY = parentData.GetAbsoluteScale().y/2f;
    }
}
