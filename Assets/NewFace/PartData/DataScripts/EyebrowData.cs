using UnityEngine;

[CreateAssetMenu(fileName = "EyebrowData", menuName = "ScriptableObjects/Eyebrow", order = 7)]
public class EyebrowData : PartData
{
    public override void SetPositionBounds(PartData parentData)
    {
        minPosX = parentData.GetAbsolutePosition().x + GetAbsoluteScale().x/2f;
        maxPosX = parentData.GetAbsolutePosition().x + parentData.GetAbsoluteScale().x/2f;
        minPosY = parentData.GetAbsolutePosition().y - parentData.GetAbsoluteScale().y/2f;
        maxPosY = parentData.GetAbsolutePosition().y + parentData.GetAbsoluteScale().y/2f;
    }

    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.GetAbsoluteScale().x/2f;
         
    }

    public override Vector2 GetColliderSize()
    {
        return new Vector2(1f, Mathf.Max(Mathf.Lerp(1f, .2f, shadePropertyDict["_EyebrowThickness"].propertyValue), Mathf.Lerp(0f, 0.5f, Mathf.Abs(shadePropertyDict["_EyebrowCurve"].propertyValue - 0.5f) * 2f)));
    }

    public override Vector2 GetColliderOffset()
    {
        return new Vector2(0f, Mathf.Lerp(.25f, -.25f, shadePropertyDict["_EyebrowCurve"].propertyValue) * shadePropertyDict["_EyebrowThickness"].propertyValue);
    }
}
