using UnityEngine;

[CreateAssetMenu(fileName = "MouthData", menuName = "ScriptableObjects/Mouth", order = 5)]
public class MouthData : PartData
{
   public override void SetScaleBounds(PartData parentData)
    {
        //Debug.Log("Setting max scale of mouth");
        maxScaleX = parentData.GetAbsScale().x;
        maxScaleY = parentData.GetAbsScale().y/2f;
    }

    public override void SetPositionBounds(PartData parentData)
    {
        minPosX = parentData.GetAbsPosition().x;
        maxPosX = parentData.GetAbsPosition().x;
        minPosY = parentData.GetAbsPosition().y - parentData.GetAbsScale().y/2f;
        maxPosY = parentData.GetAbsPosition().y + parentData.GetAbsScale().y/2f;
    }
}
