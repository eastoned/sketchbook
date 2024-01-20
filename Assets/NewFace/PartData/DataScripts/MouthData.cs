using UnityEngine;

[CreateAssetMenu(fileName = "MouthData", menuName = "ScriptableObjects/Mouth", order = 5)]
public class MouthData : PartData
{
   public override void SetScaleBounds(PartData parentData)
    {
        Debug.Log("Setting max scale of mouth");
        maxScaleX = parentData.GetAbsoluteScale().x;
        maxScaleY = parentData.GetAbsoluteScale().y/2f;
    }

    public override void SetPositionBounds(PartData parentData)
    {
        minPosY = -parentData.GetAbsoluteScale().y/2f;
        maxPosY = parentData.GetAbsoluteScale().y/2f;
    }
}
