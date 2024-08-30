using UnityEngine;

[CreateAssetMenu(fileName = "NoseData", menuName = "ScriptableObjects/Nose", order = 4)]
public class NoseData : PartData
{
    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.GetAbsScale().x;
        maxScaleY = parentData.GetAbsScale().y;
    }

    public override void SetPositionBounds(PartData parentData)
    {
        minPosX = parentData.GetAbsPosition().x;
        maxPosX = parentData.GetAbsPosition().x;
        minPosY = parentData.GetAbsPosition().y - parentData.GetAbsScale().y/2f;
        maxPosY = parentData.GetAbsPosition().y + parentData.GetAbsScale().y/2f;
    }

    public override Vector2 GetColliderSize()
    {
        float val = .95f;
        val *= Mathf.Lerp(1f, .2f, shadePropertyDict["_NoseTotalWidth"].propertyValue/2f + shadePropertyDict["_NoseCurve"].propertyValue/2f);
        val *= Mathf.Lerp(1f, .5f, shadePropertyDict["_NoseTopWidth"].propertyValue/2f + shadePropertyDict["_NoseBaseWidth"].propertyValue/2f);
        return new Vector2(val, Mathf.Lerp(1f, .35f, shadePropertyDict["_NoseTotalLength"].propertyValue));
    }

}
