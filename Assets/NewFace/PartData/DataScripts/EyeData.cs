using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EyeData", menuName = "ScriptableObjects/Eye", order = 3)]
public class EyeData : PartData{

    public override void SetPositionBounds(PartData parentData)
    {
        //Debug.Log("Setting eye lowest position with parent");
        minPosX = GetAbsoluteScale().x/2f;
        maxPosX = parentData.GetAbsoluteScale().x/2f;
        minPosY = parentData.GetAbsolutePosition().y - parentData.GetColliderSize().y/2f;
        maxPosY = parentData.GetAbsolutePosition().y + parentData.GetColliderSize().y/2f;
    }

    public override void SetPositionBounds(){
        //Debug.Log("Setting eye lowest position without parent");
        minPosX = GetAbsoluteScale().x/2f;
    }

    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.GetAbsoluteScale().x/2f;
        maxScaleY = parentData.GetAbsoluteScale().y/2f;
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