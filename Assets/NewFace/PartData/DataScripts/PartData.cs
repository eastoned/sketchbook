using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PartData : ScriptableObject
{

        public string partName;

    #region TransformData
        public Vector2 relativeToParentPosition;

        public float absoluteWorldPositionZ;

        public float minPosX, maxPosX, minPosY, maxPosY;

        public float minAngle, maxAngle;
        public float relativeToParentAngle;
        public Vector2 relativeToParentScale;

        public bool detached = false;
        public bool activeInScene = true;

        public float minScaleX, maxScaleX, minScaleY, maxScaleY;
    #endregion

    #region ShaderData
        public List<ShaderProperty> shaderProperties = new List<ShaderProperty>();
        public Dictionary<string, ShaderProperty> shadePropertyDict = new Dictionary<string, ShaderProperty>();
        public List<ShaderColor> shaderColors = new List<ShaderColor>();
    #endregion

    //return absolute values to render object in space
    public Vector3 GetAbsPosition()
    {
        //Debug.Log(minPosY);
        return new Vector3(Mathf.Lerp(minPosX, maxPosX, relativeToParentPosition.x), Mathf.Lerp(minPosY, maxPosY, relativeToParentPosition.y), absoluteWorldPositionZ);
    }

    public Vector3 GetFlippedAbsPosition()
    {
        return new Vector3(Mathf.Lerp(-minPosX, -maxPosX, relativeToParentPosition.x), Mathf.Lerp(minPosY, maxPosY, relativeToParentPosition.y), absoluteWorldPositionZ);
    }

    public virtual void SetClampedPosition(Vector3 posIn)
    {
        Vector2 clampedPos = new Vector2(Mathf.Clamp(posIn.x, minPosX, maxPosX), Mathf.Clamp(posIn.y, minPosY, maxPosY));
        relativeToParentPosition = new Vector2(Mathf.InverseLerp(minPosX, maxPosX, clampedPos.x), Mathf.InverseLerp(minPosY, maxPosY, clampedPos.y));
    }

    public virtual Vector3 GetClampedPosition(Vector3 posIn)
    {
        Vector3 clampedPos = new Vector3(Mathf.Clamp(posIn.x, minPosX, maxPosX), Mathf.Clamp(posIn.y, minPosY, maxPosY), absoluteWorldPositionZ);
        return clampedPos;
    }
    
    public Quaternion GetAbsRotation()
    {
        return Quaternion.Euler(0, 0, relativeToParentAngle);
    }

    public Vector3 GetAbsScale()
    {
        return new Vector3(Mathf.Lerp(minScaleX, maxScaleX, relativeToParentScale.x), Mathf.Lerp(minScaleY, maxScaleY, relativeToParentScale.y), 1f);
    }

    public Vector3 GetFlippedAbsScale()
    {
        //Debug.Log("returning flipped absolute scale");
        return new Vector3(Mathf.Lerp(-minScaleX, -maxScaleX, relativeToParentScale.x), Mathf.Lerp(minScaleY, maxScaleY, relativeToParentScale.y), 1f);
    }

    public virtual void SetScaleBounds(PartData parentBounds)
    {
    }

    public virtual void SetScaleBounds()
    {  
    }

    public virtual void SetPositionBounds()
    {
    }

    public virtual void SetPositionBounds(PartData parentBounds)
    {
        minPosX = parentBounds.relativeToParentPosition.x - parentBounds.relativeToParentScale.x/2f;
        maxPosX = parentBounds.relativeToParentPosition.x + parentBounds.relativeToParentScale.x/2f;
        minPosY = parentBounds.relativeToParentPosition.y - parentBounds.relativeToParentScale.y/2f;
        maxPosY = parentBounds.relativeToParentPosition.y + parentBounds.relativeToParentScale.y/2f;
    }

    public virtual Vector2 GetColliderSize()
    {
        return Vector2.one;
    }

    public virtual Vector2 GetColliderOffset()
    {
        return Vector2.zero;
    }

    public virtual void SetClampedScale(Vector3 scaleIn)
    {
        //Debug.Log("Setting scale clamped : "+ scaleIn);
        Vector2 clampedScale = new Vector2(Mathf.Clamp(scaleIn.x, minScaleX, maxScaleX), Mathf.Clamp(scaleIn.y, minScaleY, maxScaleY));
        relativeToParentScale = new Vector2(Mathf.InverseLerp(minScaleX, maxScaleX, clampedScale.x), Mathf.InverseLerp(minScaleY, maxScaleY, clampedScale.y));
    }

    public Vector3 GetClampedScale(Vector3 scaleIn)
    {
        return new Vector3(Mathf.Clamp(scaleIn.x, minScaleX, maxScaleX), Mathf.Clamp(scaleIn.y, minScaleY, maxScaleY), 1);
    }

    public Vector3 GetFlippedClampedScale(Vector3 scaleIn)
    {
        return new Vector3(Mathf.Clamp(-scaleIn.x, -maxScaleX, -minScaleX), Mathf.Clamp(scaleIn.y, minScaleY, maxScaleY), 1);
    }

    public float GetClampedAngle(float angle, bool flippedXAxis){
        if(flippedXAxis)
        {
            if(angle < 0)
            {
                relativeToParentAngle = -Mathf.Clamp(angle + 180, minAngle, maxAngle);
            }else{
                relativeToParentAngle = -Mathf.Clamp(angle - 180, minAngle, maxAngle);
            }
            return -relativeToParentAngle;
        }else{
            return relativeToParentAngle;
        }
    }

    public bool IsPositionOutsideMaximum(Vector3 posIn)
    {
        return posIn.x > maxPosX + .3f || posIn.y > maxPosY + .3f || posIn.x < minPosX - .3f || posIn.y < minPosY - .3f;
    }

    [ContextMenu("Random")]
    public void Randomize(float randomFactor)
    {
        relativeToParentAngle = Mathf.Lerp(minAngle, maxAngle, Random.Range(0.5f - randomFactor, 0.5f + randomFactor));

        relativeToParentPosition = new Vector3(
            Mathf.Lerp(minPosX, maxPosX, Random.Range(0.5f - randomFactor, 0.5f + randomFactor)),
            Mathf.Lerp(minPosY, maxPosY, Random.Range(0.5f - randomFactor, 0.5f + randomFactor)),
            absoluteWorldPositionZ);
            
        relativeToParentScale = new Vector3(
            Mathf.Lerp(minScaleX, maxScaleX, Random.Range(0.5f - randomFactor, 0.5f + randomFactor)),
            Mathf.Lerp(minScaleY, maxScaleY, Random.Range(0.5f - randomFactor, 0.5f + randomFactor)),
            1f);

        SetClampedPosition(relativeToParentPosition);
        SetClampedScale(relativeToParentScale);

        RandomizeShaders(randomFactor);
    }

    public void RandomizeShaders(float randomFactor){

        foreach(ShaderProperty sp in shaderProperties){
            float val = UnityEngine.Random.Range(0.5f - randomFactor, 0.5f + randomFactor);
            sp.SetValue(val);
        }
        
        foreach(ShaderColor sc in shaderColors){
            sc.SetValue(UnityEngine.Random.Range(0f, 1f));
            sc.SetHue(UnityEngine.Random.Range(0f, 1f));
            sc.SetSaturation(UnityEngine.Random.Range(0f, 1f));
        }
    }

    public void CopyData(PartData pd)
    {
        relativeToParentPosition = pd.relativeToParentPosition;
        relativeToParentAngle = pd.relativeToParentAngle;
        //minPosX = pd.minPosX;
        //maxPosX = pd.maxPosX;
        //minPosY = pd.minPosY;
        //maxPosY = pd.maxPosY;
        //minAngle = pd.minAngle;
        //maxAngle = pd.maxAngle;
        relativeToParentScale = pd.relativeToParentScale;
        //minScaleX = pd.minScaleX;
        //maxScaleX = pd.maxScaleX;
        //minScaleY = pd.minScaleY;
        //maxScaleY = pd.maxScaleY;
        
        
        for(int i = 0; i < shaderProperties.Count; i++)
        {
            shaderProperties[i] = new ShaderProperty(pd.shaderProperties[i].propertyName, pd.shaderProperties[i].propertyValue, pd.shaderProperties[i].propertyFeature);
        }

        for(int j = 0; j < shaderColors.Count; j++)
        {
            shaderColors[j] = new ShaderColor(pd.shaderColors[j].colorName, pd.shaderColors[j].colorValue);
        }
    }

    public void CopyPartData(PartData pd)
    {
        relativeToParentPosition = pd.relativeToParentPosition;
        //minPosX = pd.minPosX;
        //maxPosX = pd.maxPosX;
        //minPosY = pd.minPosY;
        //maxPosY = pd.maxPosY;
        //minAngle = pd.minAngle;
        //maxAngle = pd.maxAngle;
        relativeToParentScale = pd.relativeToParentScale;
        //minScaleX = pd.minScaleX;
        //maxScaleX = pd.maxScaleX;
        //minScaleY = pd.minScaleY;
        //maxScaleY = pd.maxScaleY;
    }
}

[System.Serializable]
public class ShaderProperty{

    public enum AffectedFeature
    {
        NOTHING,
        SIGHT,
        HEARING,
        SMELL,
        SPEECH
    }
    public string propertyName;

    [Range(0f, 1f)]
    public float propertyValue;

    public bool wholeNumberInterval;
    public float valueInterval;
    public AffectedFeature propertyFeature;

    public void SetValue(float value){
        propertyValue = value;
    }

    public ShaderProperty(string name, float value, AffectedFeature affectedFeature){
        propertyName = name;
        propertyValue = value;
        propertyFeature = affectedFeature;
    }
}

[System.Serializable]
public class ShaderColor{
    public string colorName;
    public Color colorValue;

    float h,s,v;

    public void SetHue(float value){
        //Debug.Log("Changing Hue");
        Color.RGBToHSV(colorValue, out h, out s, out v);
        //value /= 0.1f;
        //value = Mathf.Round(value);
        //value *= 0.1f;
        colorValue = Color.HSVToRGB(value, s, v);
    }

    public float GetHue(){
        Color.RGBToHSV(colorValue, out h, out s, out v);
        return h;
    }

    public void SetSaturation(float value){
        //Debug.Log("Changing Saturation");
        Color.RGBToHSV(colorValue, out h, out s, out v);
        //value /= 0.1f;
        //value = Mathf.Round(value);
        //value *= 0.1f;
        colorValue = Color.HSVToRGB(h, value, v);
    }
    public float GetSaturation(){
        Color.RGBToHSV(colorValue, out h, out s, out v);
        return s;
    }

    public void SetValue(float value){
        //Debug.Log("Changing Value");
        Color.RGBToHSV(colorValue, out h, out s, out v);
        //value /= 0.1f;
        //value = Mathf.Round(value);
        //value *= 0.1f;
        colorValue = Color.HSVToRGB(h, s, value);
    }

    public float GetValue(){
        Color.RGBToHSV(colorValue, out h, out s, out v);
        return v;
    }

    public ShaderColor(string name, Color value){
        colorName = name;
        colorValue = value;
    }

    

}