using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartTransformController : MonoBehaviour
{

    public enum TransformController{
        TRANSLATE,
        ROTATION,
        SCALE
    }

    
    public TransformController controls;

    public Vector3 mouseDelta2;

    void Start(){
        
    }

    void OnMouseEnter(){

    }

    void OnMouseExit(){
        
    }


    void OnMouseDrag(){
        mouseDelta2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        transform.localPosition = new Vector3(mouseDelta2.x, mouseDelta2.y, transform.localPosition.z);

        switch(controls){
            case TransformController.TRANSLATE:
                OnTranslatePartController.Instance.Invoke(transform.localPosition);
            break;
            case TransformController.ROTATION:
                OnRotatePartController.Instance.Invoke(transform.localPosition);
            break;
            case TransformController.SCALE:
                OnScalePartController.Instance.Invoke(transform.localPosition);
            break;
        }
        
    }

}
