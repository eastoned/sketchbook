using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "PartData", menuName = "ScriptableObjects/PartData", order = 3)]
public class PartData : ScriptableObject
{
    #region TransformData
        public bool translatable, rotatable, scalable;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    
    #endregion

    #region ShaderData
        public List<ShaderProperty> shaderProperties = new List<ShaderProperty>();
        public List<ShaderColor> shaderColors = new List<ShaderColor>();
    #endregion
    
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