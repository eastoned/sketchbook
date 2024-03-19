using UnityEngine;

[CreateAssetMenu(fileName = "HeadData", menuName = "ScriptableObjects/Head", order = 1)]
public class HeadData : PartData
{
     public override Vector2 GetColliderSize()
    {
        if(shadePropertyDict.Count > 0){
            return new Vector2(1f, Mathf.Lerp(1f, .5f, shadePropertyDict["_ChinScale"].propertyValue/2f + shadePropertyDict["_ForeheadScale"].propertyValue/2f));
        }else{
            return Vector2.one;
        }
    }

    public override Vector2 GetColliderOffset()
    {
        return new Vector2(0f, (shadePropertyDict["_ChinScale"].propertyValue - shadePropertyDict["_ForeheadScale"].propertyValue)/8f);
    }

    public override void SetPositionBounds()
    {
        minPosY = -1/GetAbsoluteScale().y;
        maxPosY = 1/GetAbsoluteScale().y;
    }

    public override void SetScaleBounds()
    {
        //maxScaleX = parentData.GetAbsoluteScale().x/2f;
        //maxScaleY = parentData.GetAbsoluteScale().y/2f;
    }
}
