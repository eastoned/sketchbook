using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartTransformController : MonoBehaviour
{

    public enum TransformController{
        TRANSLATE,
        ROTATION,
        SCALE
    }

    public Texture2D icon;

    
    public TransformController controls;

    public Vector3 mouseDelta2;
    public Vector3 offset;

    void Start(){
        if(icon != null){
            GetComponent<Renderer>().material.SetTexture("_IconTex", icon);
        }
    }
    
    void OnMouseDown(){
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        mouseDelta2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.localPosition - mouseDelta2;
    }

    void OnMouseDrag(){
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        mouseDelta2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.localPosition = new Vector3(mouseDelta2.x, mouseDelta2.y, transform.localPosition.z);
        

        switch(controls){
            case TransformController.TRANSLATE:
                
                    OnTranslatePartController.Instance.Invoke(transform.localPosition + offset);
                
            break;
            case TransformController.ROTATION:
                
                    OnRotatePartController.Instance.Invoke(transform.localPosition);
                
            break;
            case TransformController.SCALE:
                
                    OnScalePartController.Instance.Invoke(transform.localPosition);
                
            break;
        }
        
    }

    void OnMouseUp(){
        OnConfirmTransformPart.Instance.Invoke();
    }

}
