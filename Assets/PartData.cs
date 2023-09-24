using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "PartData", menuName = "ScriptableObjects/PartData", order = 3)]
public class PartData : ScriptableObject
{

    public List<PartData> affectedParts = new List<PartData>();
    #region TransformData
        public bool translatable, rotatable, scalable;
        
        public Vector3 position;
        public float minPosX, maxPosX, minPosY, maxPosY;
        public float minAngle, maxAngle;
        public float currentAngle;
        public Vector3 scale;
        public float minScaleX, maxScaleX, minScaleY, maxScaleY;
    #endregion

    #region ShaderData
        public List<ShaderProperty> shaderProperties = new List<ShaderProperty>();
        public Dictionary<string, ShaderProperty> shadePropertyDict = new Dictionary<string, ShaderProperty>();
        public List<ShaderColor> shaderColors = new List<ShaderColor>();
    #endregion

    public Vector3 ClampedScale(Vector3 scaleIn){
        Vector3 clampedSize = new Vector3(Mathf.Clamp(scaleIn.x, minScaleX, maxScaleX), Mathf.Clamp(scaleIn.y, minScaleY, maxScaleY), 1);
        return clampedSize;
    }

    public float ClampedAngle(float angle){
        return Mathf.Clamp(angle, minAngle, maxAngle);
    }

    public virtual void ClampedPosition(Vector3 posIn){
        Vector3 clampedPos = new Vector3(Mathf.Clamp(posIn.x, minPosX, maxPosX), Mathf.Clamp(posIn.y, minPosY, maxPosY), 1);
    }

    public virtual void RelativeScale(Vector3 parentScale){
        scale = ClampedScale(scale);
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

    public bool significant = false;
    public bool active = false;
    public void SignificantPiece(float value){
        //if(!active)
            OnChangedMouthScaleEvent.Instance.Invoke(propertyValue - value);
            
       ///active = true;
    }

    public string pos, neg;
}



[System.Serializable]
public class ShaderColor{
    public string colorName;
    public Color colorValue;
}