using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class RequestChange{
    public string requestMessage;
    public string successMessage;
    public PartController partToChange;
    public Vector2 positionDelta;
    public Vector2 scaleDelta;

    [Range(-90f, 90f)]
    public float angleDelta;

    public List<ShaderRequest> shaderRequests;
    private ShaderCache[] shaderPropertyCache;

    private Vector2 initialPosition, initialScale;
    private float initialAngle;


    private bool positionFulfilled, scaleFulfilled, rotationFulfilled, shadersFulfilled;

    public bool successIfAnyConditionMet = false;

    public void SetCache(PartData pd){
        initialPosition = new Vector2(pd.absolutePosition.x, pd.absolutePosition.y);
        initialScale = new Vector2(pd.absoluteScale.x, pd.absoluteScale.y);
        initialAngle = pd.currentAngle;
        shaderPropertyCache = new ShaderCache[shaderRequests.Count];
        for(int i = 0; i < shaderRequests.Count; i++){
            shaderPropertyCache[i] = new ShaderCache(shaderRequests[i].shaderVariable, pd.shaderProperties[shaderRequests[i].shaderVariable].propertyValue);
        }
    }

    public void SetListenersForCorrectEvent(){
        positionFulfilled = true;
        scaleFulfilled = true;
        rotationFulfilled = true;
        shadersFulfilled = true;

        if(!positionDelta.magnitude.Equals(0f)){
            Debug.Log("Request needs a position change");
            OnTranslatePartController.Instance.AddListener(CheckPositionChange);
            positionFulfilled = false;
        }

        if(!scaleDelta.magnitude.Equals(0f)){
            Debug.Log("Request needs a scale change");
            //OnScalePartController.Instance.AddListener();
            scaleFulfilled = false;
        }

        if(!angleDelta.Equals(0f)){
            Debug.Log("Request needs a rotation change");
            //OnRotatePartController.Instance.AddListener();
            rotationFulfilled = false;
        }

        if(shaderRequests.Count > 0){
            Debug.Log("Request needs a shader change");
            OnChangedShaderProperty.Instance.AddListener(CheckShaderChange);
            shadersFulfilled = false;
        }
    }

    public void CheckShaderChange(float val){
        int shaderConditionFulfilled = 0;
        for(int i = 0; i < shaderRequests.Count; i++){
            if(shaderRequests[i].valueDelta > 0f){
                if(partToChange.pd.shaderProperties[shaderRequests[i].shaderVariable].propertyValue - shaderPropertyCache[i].shaderValue >= shaderRequests[i].valueDelta){
                    Debug.Log("Shader positive change fulfilled");
                    shaderConditionFulfilled++;
                }
            }else if(shaderRequests[i].valueDelta < 0f){
                if(partToChange.pd.shaderProperties[shaderRequests[i].shaderVariable].propertyValue - shaderPropertyCache[i].shaderValue <= shaderRequests[i].valueDelta){
                    Debug.Log("Shader negative change fulfilled");
                    shaderConditionFulfilled++;
                }
            }
        }

        if(shaderConditionFulfilled == shaderRequests.Count){
            shadersFulfilled = true;
        }else{
            shadersFulfilled = false;
        }
    }
 
    public void CheckPositionChange(Vector3 partPos){
        bool xConditionFulfilled = false;
        bool yConditionFulfilled = false;
        if(!positionDelta.x.Equals(0f)){
            if(positionDelta.x > 0f){
                if(partToChange.pd.absolutePosition.x - initialPosition.x >= positionDelta.x){
                    Debug.Log("X Condition is fulfilled");
                    xConditionFulfilled = true;
                }
            }else{
                if(partToChange.pd.absolutePosition.x - initialPosition.x <= positionDelta.x){
                    Debug.Log("X Condition is fulfilled");
                    xConditionFulfilled = true;
                }
            }
        }else{
            xConditionFulfilled = true;
        }

        if(!positionDelta.y.Equals(0f)){
            if(positionDelta.y > 0f){
                if(partToChange.pd.absolutePosition.y - initialPosition.y >= positionDelta.y){
                    Debug.Log("Y Condition is fulfilled");
                    yConditionFulfilled = true;
                }
            }else{
                if(partToChange.pd.absolutePosition.y - initialPosition.y <= positionDelta.y){
                    Debug.Log("Y Condition is fulfilled");
                    yConditionFulfilled = true;
                }
            }
        }else{
            yConditionFulfilled = true;
        }
        
        if(xConditionFulfilled && yConditionFulfilled){
            positionFulfilled = true;
        }else{
            positionFulfilled = false;
        }

    }

    public bool CheckTotalRequestFulfilled(){
        if(positionFulfilled && scaleFulfilled && rotationFulfilled && shadersFulfilled){
            OnTranslatePartController.Instance.RemoveListener(CheckPositionChange);
            OnChangedShaderProperty.Instance.RemoveListener(CheckShaderChange);
            return true;
        }

        return false;
    }
}

public class ShaderCache{
    public int shaderID;
    public float shaderValue;
    public ShaderCache(int id, float val){
        shaderID = id;
        shaderValue = val;
    }
}

public class RequestTarget{
    public string requestMessage;
    public PartData partToChange;
    public Vector2 positionTarget;
    public Vector2 scaleTarget;
    [Range(-90f, 90f)]
    public float angleTarget;


    public bool RequestFulfilled(){
        return true;
    }
}

[System.Serializable]
public class ShaderRequest{
    public int shaderVariable;
    public string valueName;
    [Range(-1f, 1f)]
    public float valueDelta;

    public ShaderRequest(int shaderVar, string shaderName, float value){
        shaderVariable = shaderVar;
        valueName = shaderName;
        valueDelta = value;
    }
}
