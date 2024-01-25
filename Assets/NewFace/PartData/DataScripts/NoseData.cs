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
        minPosY = parentData.GetAbsolutePosition().y - parentData.GetAbsoluteScale().y/2f;
        maxPosY = parentData.GetAbsolutePosition().y + parentData.GetAbsoluteScale().y/2f;
    }

    public override Vector2 GetColliderSize()
    {
        //return base.GetColliderSize();_NoseTotalWidth_NoseCurve
        float val = .95f;
        val *= Mathf.Lerp(1f, .2f, shadePropertyDict["_NoseTotalWidth"].propertyValue/2f + shadePropertyDict["_NoseCurve"].propertyValue/2f);
        val *= Mathf.Lerp(1f, .5f, shadePropertyDict["_NoseTopWidth"].propertyValue/2f + shadePropertyDict["_NoseBaseWidth"].propertyValue/2f);
        //val *= Mathf.Lerp(1f, .9f, );
        //Mathf.Max(shadePropertyDict["_NoseTotalWidth"].propertyValue, shadePropertyDict["_NoseBaseWidth"].propertyValue) * shadePropertyDict["_NoseCurve"].propertyValue
        //float val =  shadePropertyDict["_NoseTopWidth"].propertyValue   * .1f;
        //val *= shadePropertyDict["_NoseTotalWidth"].propertyValue;
        return new Vector2(val, Mathf.Lerp(1f, .35f, shadePropertyDict["_NoseTotalLength"].propertyValue));
    }

}
