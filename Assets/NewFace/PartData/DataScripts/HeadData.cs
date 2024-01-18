using UnityEngine;

[CreateAssetMenu(fileName = "HeadData", menuName = "ScriptableObjects/Head", order = 1)]
public class HeadData : PartData
{
     public override Vector2 GetColliderSize()
    {
    
        return new Vector2(1f, Mathf.Lerp(1f, .5f, shadePropertyDict["_ChinScale"].propertyValue/2f + shadePropertyDict["_ForeheadScale"].propertyValue/2f));
    }

    public override Vector2 GetColliderOffset()
    {
        return new Vector2(0f, (shadePropertyDict["_ChinScale"].propertyValue - shadePropertyDict["_ForeheadScale"].propertyValue)/8f);
    }
}
