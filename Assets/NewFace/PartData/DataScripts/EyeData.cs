using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EyeData", menuName = "ScriptableObjects/Eye", order = 3)]
public class EyeData : PartData
{

    public override void SetPositionBounds(PartData parentData)
    {
        //Debug.Log("Setting eye lowest position with parent");
        minPosX = parentData.GetAbsPosition().x + GetAbsScale().x/2f;
        maxPosX = parentData.GetAbsPosition().x + parentData.GetAbsScale().x/2f;
        minPosY = parentData.GetAbsPosition().y - parentData.GetColliderSize().y/2f;
        maxPosY = parentData.GetAbsPosition().y + parentData.GetColliderSize().y/2f;
    }

    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.GetAbsScale().x/2f;
        maxScaleY = parentData.GetAbsScale().y/2f;
    }

    public override Vector2 GetColliderSize()
    {
        return new Vector2(1f, Mathf.Lerp(0f, 1f, shadePropertyDict["_EyelidTopLength"].propertyValue/2f + shadePropertyDict["_EyelidBottomLength"].propertyValue/2f));
    }

    public override Vector2 GetColliderOffset()
    {
        return new Vector2(0f, Mathf.Lerp(0f, .25f, shadePropertyDict["_EyelidTopLength"].propertyValue) + Mathf.Lerp(0f, -.25f, shadePropertyDict["_EyelidBottomLength"].propertyValue));
    }

    public List<ShaderProperty> reactiveProperties = new List<ShaderProperty>();
    
}