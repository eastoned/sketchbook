using UnityEngine;

public class NosePartController : BodyPartController
{

    public override void SetColliderSize()
    {
        float val = .95f;
        
        val *= Mathf.Lerp(1f, 0.2f, shaderProperties[1].propertyValue/2f + shaderProperties[3].propertyValue/2f);
        val *= Mathf.Lerp(1f, 0.5f, shaderProperties[2].propertyValue/2f + shaderProperties[0].propertyValue/2f);
        colid.size =  new Vector2(val, Mathf.Lerp(1f, .35f, shaderProperties[4].propertyValue));
    }

}
