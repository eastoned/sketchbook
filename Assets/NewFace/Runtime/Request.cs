using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Schema;
using UnityEngine;

[System.Serializable]
public class RequestChange{
    public string requestMessage;
    public PartData partToChange;
    public Vector2 positionDelta;
    public Vector2 scaleDelta;

    [Range(-90f, 90f)]
    public float angleDelta;

    public List<ShaderRequest> shaderRequests;

    private Vector2 initialPosition, initialScale;
    private float initialAngle;

    private bool positionFulfilled, scaleFulfilled, rotationFulfilled;

    public void SetTransformCache(PartData pd){
        initialPosition = new Vector2(pd.absolutePosition.x, pd.absolutePosition.y);
        initialScale = new Vector2(pd.absoluteScale.x, pd.absoluteScale.y);
        initialAngle = pd.currentAngle;  
    }

    public void SetListenersForCorrectEvent(){
        positionFulfilled = true;
        scaleFulfilled = true;
        rotationFulfilled = true;

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
    }
 
    public void CheckPositionChange(Vector3 partPos){
        bool xConditionFulfilled = false;
        bool yConditionFulfilled = false;
        if(!positionDelta.x.Equals(0f)){
            if(positionDelta.x > 0f){
                if(partToChange.absolutePosition.x - initialPosition.x >= positionDelta.x){
                    Debug.Log("X Condition is fulfilled");
                    xConditionFulfilled = true;
                }
            }else{
                if(partToChange.absolutePosition.x - initialPosition.x <= positionDelta.x){
                    Debug.Log("X Condition is fulfilled");
                    xConditionFulfilled = true;
                }
            }
        }else{
            xConditionFulfilled = true;
        }

        if(!positionDelta.y.Equals(0f)){
            if(positionDelta.y > 0f){
                if(partToChange.absolutePosition.y - initialPosition.y >= positionDelta.y){
                    Debug.Log("Y Condition is fulfilled");
                    yConditionFulfilled = true;
                }
            }else{
                if(partToChange.absolutePosition.y - initialPosition.y <= positionDelta.y){
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
        if(positionFulfilled && scaleFulfilled && rotationFulfilled){
            OnTranslatePartController.Instance.RemoveListener(CheckPositionChange);
            return true;
        }

        return false;
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
    public string variableName;
    public float valueDelta;

    public ShaderRequest(string name, float value){
        variableName = name;
        valueDelta = value;
    }
}
