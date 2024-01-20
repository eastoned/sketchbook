using UnityEngine;

[CreateAssetMenu(fileName = "HairBackData", menuName = "ScriptableObjects/HairBack", order = 9)]
public class HairBackData : PartData
{
    public override void SetScaleBounds(PartData parentData)
    {
    }

    public override void SetPositionBounds(PartData parentData)
    {
        minPosY = -parentData.GetAbsoluteScale().y/2f;
        maxPosY = parentData.GetAbsoluteScale().y/2f;
    }
}
