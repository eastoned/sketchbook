using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PartData : ScriptableObject
{

    public void CopyData(PartData pd){
        absolutePosition = pd.absolutePosition;
        relativePosition = pd.relativePosition;
        //minPosX = pd.minPosX;
        //maxPosX = pd.maxPosX;
        //minPosY = pd.minPosY;
        //maxPosY = pd.maxPosY;
        //minAngle = pd.minAngle;
        //maxAngle = pd.maxAngle;
        currentAngle = pd.currentAngle;
        absoluteScale = pd.absoluteScale;
        relativeScale = pd.relativeScale;
        //minScaleX = pd.minScaleX;
        //maxScaleX = pd.maxScaleX;
        //minScaleY = pd.minScaleY;
        //maxScaleY = pd.maxScaleY;
        
        
        for(int i = 0; i < shaderProperties.Count; i++)
        {
            shaderProperties[i] = new ShaderProperty(pd.shaderProperties[i].propertyName, pd.shaderProperties[i].propertyValue);
        }

        for(int j = 0; j < shaderColors.Count; j++)
        {
            shaderColors[j] = new ShaderColor(pd.shaderColors[j].colorName, pd.shaderColors[j].colorValue);
        }
    }

    public void CopyPartData(PartData pd){
        absolutePosition = pd.absolutePosition;
        relativePosition = pd.relativePosition;
        //minPosX = pd.minPosX;
        //maxPosX = pd.maxPosX;
        //minPosY = pd.minPosY;
        //maxPosY = pd.maxPosY;
        //minAngle = pd.minAngle;
        //maxAngle = pd.maxAngle;
        currentAngle = pd.currentAngle;
        absoluteScale = pd.absoluteScale;
        relativeScale = pd.relativeScale;
        //minScaleX = pd.minScaleX;
        //maxScaleX = pd.maxScaleX;
        //minScaleY = pd.minScaleY;
        //maxScaleY = pd.maxScaleY;
    }
        public string partName;

    #region TransformData
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

    public List<PartData> affectedPartData = new List<PartData>();

    //return normalized values to store part relation to parent object
    public void SetRelativePos(Vector3 pos){
        relativePosition = new Vector3(Mathf.InverseLerp(minPosX, maxPosX, pos.x), Mathf.InverseLerp(minPosY, maxPosY, pos.y), pos.z);
        absolutePosition = pos;
    }

    //return absolute values to render object in space
    public Vector3 GetAbsolutePosition(){
        return new Vector3(Mathf.Lerp(minPosX, maxPosX, relativePosition.x), Mathf.Lerp(minPosY, maxPosY, relativePosition.y), absolutePosition.z);
    }

    public Quaternion GetAbsoluteRotation(){
        return Quaternion.Euler(0, 0, currentAngle);
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
        //Debug.Log(this.name + " has new scale factors");
        maxScaleX = pd.GetAbsoluteScale().x/2f;
        maxScaleY = pd.GetAbsoluteScale().y/2f;
    }

    public virtual void SetScaleBounds(){
        
    }

    public virtual void SetPositionBounds(PartData pd){

    }

    public virtual void SetPositionBounds(){

    }

    public virtual Vector2 GetColliderSize(){
        return Vector2.one;
    }

    public virtual Vector2 GetColliderOffset(){
        return Vector2.zero;
    }

    public virtual void ClampedScale(Vector3 scaleIn){
        Vector3 clampedSize = new Vector3(Mathf.Clamp(scaleIn.x, minScaleX, maxScaleX), Mathf.Clamp(scaleIn.y, minScaleY, maxScaleY), 1);
        //clampedSize = clampedSize/.25f;
        //clampedSize = new Vector3(Mathf.Round(clampedSize.x), Mathf.Round(clampedSize.y), clampedSize.z);
        //clampedSize *= .25f;
        SetRelativeScale(clampedSize);
    }
    public Vector3 GetClampedScale(Vector3 scaleIn)
    {
        return new Vector3(Mathf.Clamp(scaleIn.x, minScaleX, maxScaleX), Mathf.Clamp(scaleIn.y, minScaleY, maxScaleY), 1);
    }

    public Vector3 GetFlippedClampedScale(Vector3 scaleIn)
    {
        return new Vector3(Mathf.Clamp(-scaleIn.x, -maxScaleX, -minScaleX), Mathf.Clamp(scaleIn.y, minScaleY, maxScaleY), 1);
    }

    public float ClampedAngle(float angle, bool flippedXAxis){
        if(flippedXAxis){
            if(angle < 0){
                currentAngle = -Mathf.Clamp(angle + 180, minAngle, maxAngle);
            }else{
                currentAngle = -Mathf.Clamp(angle - 180, minAngle, maxAngle);
            }
            currentAngle = currentAngle/15f;
            currentAngle = Mathf.Round(currentAngle);
            currentAngle *= 15f;
            return -currentAngle;
        }else{
            currentAngle = Mathf.Clamp(angle, minAngle, maxAngle);
            currentAngle = currentAngle/15f;
            currentAngle = Mathf.Round(currentAngle);
            currentAngle *= 15f;
            return currentAngle;
        }
        
    }

    public bool PositionOutsideMaximum(Vector3 posIn){
        return posIn.x > maxPosX + .3f || posIn.y > maxPosY + .3f || posIn.x < minPosX - .3f || posIn.y < minPosY - .3f;
    }

    public virtual void ClampedPosition(Vector3 posIn){
        
        Vector3 clampedPos = new Vector3(Mathf.Clamp(posIn.x, minPosX, maxPosX), Mathf.Clamp(posIn.y, minPosY, maxPosY), posIn.z);
        //clampedPos = clampedPos/.1f;
        //clampedPos = new Vector3(Mathf.Round(clampedPos.x), Mathf.Round(clampedPos.y), clampedPos.z);
        //clampedPos *= .1f;
        SetRelativePos(clampedPos);
    }

    public virtual Vector3 ReturnClampedPosition(Vector3 posIn){
        Vector3 clampedPos = new Vector3(Mathf.Clamp(posIn.x, minPosX, maxPosX), Mathf.Clamp(posIn.y, minPosY, maxPosY), posIn.z);
        //clampedPos = clampedPos/.1f;
        //clampedPos = new Vector3(Mathf.Round(clampedPos.x), Mathf.Round(clampedPos.y), clampedPos.z);
        //clampedPos *= .1f;
        return clampedPos;
    }
}

[System.Serializable]
public class ShaderProperty{
    public string propertyName;

    [Range(0f, 1f)]
    public float propertyValue;

    public bool wholeNumberInterval;
    public float valueInterval;

    public void SetValue(float value){
        //value /= 0.25f;
        //value = Mathf.Round(value);
        //value *= 0.25f;
        propertyValue = value;
    }

    public ShaderProperty(string name, float value){
        propertyName = name;
        propertyValue = value;
    }

    public void ReadRandomRemark(float value){
        //Debug.Log("Getting value as: " + value + ". And the current property value is: " + propertyValue + ".");
        if(SpeechController.timeSinceLastRemark > 5f){
            if(value > propertyValue){
                //
                if(IncreaseValueRemarks.Length > 0){
                    OnSendRemarkToSpeech.Instance.Invoke(IncreaseValueRemarks[Random.Range(0, IncreaseValueRemarks.Length)].message);
                }
            }else{
                //
                if(DecreaseValueRemarks.Length > 0){
                    OnSendRemarkToSpeech.Instance.Invoke(DecreaseValueRemarks[Random.Range(0, DecreaseValueRemarks.Length)].message);
                }
            }
        }
    }

    public Remark[] IncreaseValueRemarks;
    public Remark[] DecreaseValueRemarks;

}

[System.Serializable]
public class Remark{
    public string message;
    public bool wasSaid = false;
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