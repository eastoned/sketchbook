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
        mouseDelta2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.localPosition - mouseDelta2;
    }

    void OnMouseDrag(){
        mouseDelta2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(NuFaceManager.money > 0){
            transform.localPosition = new Vector3(mouseDelta2.x, mouseDelta2.y, transform.localPosition.z);
        }

        switch(controls){
            case TransformController.TRANSLATE:
                if(NuFaceManager.money > 0){
                    OnTranslatePartController.Instance.Invoke(transform.localPosition + offset);
                }
            break;
            case TransformController.ROTATION:
                if(NuFaceManager.money > 0){
                    OnRotatePartController.Instance.Invoke(transform.localPosition);
                }
            break;
            case TransformController.SCALE:
                if(NuFaceManager.money > 0){
                    OnScalePartController.Instance.Invoke(transform.localPosition);
                }
            break;
        }
        
    }

    void OnMouseUp(){
        OnConfirmTransformPart.Instance.Invoke();
    }

}
