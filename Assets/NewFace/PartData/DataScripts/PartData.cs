using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class PartData : ScriptableObject
{
    #region TransformData
        public bool translatable, rotatable, scalable;
        
        public Vector3 absolutePosition;
        public Vector3 relativePosition;

        public float minPosX, maxPosX, minPosY, maxPosY;
        public float minAngle, maxAngle;
        public float currentAngle;
        public Vector3 absoluteScale;
        public Vector3 relativeScale;
        public float minScaleX, maxScaleX, minScaleY, maxScaleY;
    #endregion

    #region ShaderData
        public List<ShaderProperty> shaderProperties = new List<ShaderProperty>();
        public Dictionary<string, ShaderProperty> shadePropertyDict = new Dictionary<string, ShaderProperty>();
        public List<ShaderColor> shaderColors = new List<ShaderColor>();
    #endregion

    //return normalized values to store part relation to parent object
    public void SetRelativePos(Vector3 pos){
        relativePosition = new Vector3(Mathf.InverseLerp(minPosX, maxPosX, pos.x), Mathf.InverseLerp(minPosY, maxPosY, pos.y), pos.z);
        absolutePosition = pos;
    }

    //return absolute alues to render object in space
    public Vector3 GetAbsolutePosition(){
        return new Vector3(Mathf.Lerp(minPosX, maxPosX, relativePosition.x), Mathf.Lerp(minPosY, maxPosY, relativePosition.y), absolutePosition.z);
    }

    public Vector3 GetFlippedAbsolutePosition(){
        return new Vector3(Mathf.Lerp(-minPosX, -maxPosX, relativePosition.x), Mathf.Lerp(minPosY, maxPosY, relativePosition.y), absolutePosition.z);
    }

    public void SetRelativeScale(Vector3 scl){
        relativeScale = new Vector3(Mathf.InverseLerp(minScaleX, maxScaleX, scl.x), Mathf.InverseLerp(minScaleY, maxScaleY, scl.y), scl.z);
    }

    public Vector3 GetFlippedAbsoluteScale(){
        return new Vector3(Mathf.Lerp(-minScaleX, -maxScaleX, relativeScale.x), Mathf.Lerp(minScaleY, maxScaleY, relativeScale.y), relativeScale.z);
    }

    public Vector3 GetAbsoluteScale(){
        return new Vector3(Mathf.Lerp(minScaleX, maxScaleX, relativeScale.x), Mathf.Lerp(minScaleY, maxScaleY, relativeScale.y), relativeScale.z);
    }


    public virtual void SetScaleBounds(PartData pd){

    }

    public virtual void SetPositionBounds(PartData pd){

    }

    public virtual void SetPositionBounds(){

    }

    public virtual void ClampedScale(Vector3 scaleIn){
        Vector3 clampedSize = new Vector3(Mathf.Clamp(scaleIn.x, minScaleX, maxScaleX), Mathf.Clamp(scaleIn.y, minScaleY, maxScaleY), 1);
        SetRelativeScale(clampedSize);
    }

    public float ClampedAngle(float angle){
        return Mathf.Clamp(angle, minAngle, maxAngle);
    }

    public virtual void ClampedPosition(Vector3 posIn){
        Vector3 clampedPos = new Vector3(Mathf.Clamp(posIn.x, minPosX, maxPosX), Mathf.Clamp(posIn.y, minPosY, maxPosY), posIn.z);
        SetRelativePos(clampedPos);
    }
}

[System.Serializable]
public class ShaderProperty{
    public string propertyName;

    [Range(0f, 1f)]
    public float propertyValue;

    public void SetValue(float value){
        propertyValue = value;
    }


}

[System.Serializable]
public class ShaderColor{
    public string colorName;
    public Color colorValue;

    float h,s,v;

    public void SetHue(float value){
        Debug.Log("Changing Hue");
        Color.RGBToHSV(colorValue, out h, out s, out v);
        colorValue = Color.HSVToRGB(value, s, v);
    }

    public float GetHue(){
        Color.RGBToHSV(colorValue, out h, out s, out v);
        return h;
    }

    public void SetSaturation(float value){
        Debug.Log("Changing Saturation");
        Color.RGBToHSV(colorValue, out h, out s, out v);
        colorValue = Color.HSVToRGB(h, value, v);
    }
    public float GetSaturation(){
        Color.RGBToHSV(colorValue, out h, out s, out v);
        return s;
    }

    public void SetValue(float value){
        Debug.Log("Changing Value");
        Color.RGBToHSV(colorValue, out h, out s, out v);
        colorValue = Color.HSVToRGB(h, s, value);
    }

    public float GetValue(){
        Color.RGBToHSV(colorValue, out h, out s, out v);
        return v;
    }


}