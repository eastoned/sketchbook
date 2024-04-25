using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartTransformController : MonoBehaviour
{

    public enum TransformController{
        TRANSLATE,
        ROTATION,
        SCALE,
        NOTHING
    }

    public Texture2D icon;

    public TransformController controls;

    public Vector3 mouseDelta2;
    public Vector3 offset;
    public bool currentlyHeld = false;

    public PartController partInEdit;

    public Vector3 testInput;

    void Start(){
        if(icon != null){
            GetComponent<Renderer>().material.SetTexture("_IconTex", icon);
        }
    }
    
    void OnMouseDown(){
        if(CustomUtils.IsPointerOverUIObject())
            return;

        OnSetTransformCacheEvent.Instance.Invoke();
        mouseDelta2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mouseDelta2;
        
        
        currentlyHeld = true;
    }

    void OnMouseDrag(){
        if(!currentlyHeld){
            //if(CustomUtils.IsPointerOverUIObject())
              //  return;
        }

        mouseDelta2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        switch(controls){
            case TransformController.TRANSLATE:
                Debug.Log(transform.position + offset);
                transform.position = new Vector3(mouseDelta2.x, mouseDelta2.y, transform.position.z);
                if(partInEdit.customScaleAnchor != null){
                    //offset += transform.localPosition;
                }
                OnTranslatePartController.Instance.Invoke(partInEdit, transform.position + offset);
            break;
            case TransformController.ROTATION:
                transform.position = new Vector3(mouseDelta2.x, mouseDelta2.y, transform.position.z);
                OnRotatePartController.Instance.Invoke(transform.position);
            break;
            case TransformController.SCALE:
                transform.position = new Vector3(mouseDelta2.x, mouseDelta2.y, transform.position.z);
                OnScalePartController.Instance.Invoke(transform.position);
            break;
            case TransformController.NOTHING:
            break;
        }
    }

    void OnMouseUp(){
        currentlyHeld = false;
        //OnConfirmTransformPart.Instance.Invoke();
    }

    public void Disappear(){
        transform.localPosition = new Vector3(100, 100, 100);
    }

    public void UpdateControllerPositions(){
        if(partInEdit.customScaleAnchor != null){
            transform.position = partInEdit.customScaleAnchor.TransformPoint(new Vector3(0.5f, 1f, 0));
        }else{
            transform.position = partInEdit.transform.TransformPoint(new Vector3(0.5f, 0.5f, 0)); 
        }
        transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
    }

    void Update()
    {
        if(partInEdit != null){
            if(partInEdit.detached){
                    switch(controls){
                        case TransformController.ROTATION:
                        transform.position = partInEdit.transform.TransformPoint(new Vector3(0.5f, 0, 0));
                        transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
                        transform.localScale = Vector3.one * partInEdit.transform.localScale.y * 0.25f;
                    break;
                    case TransformController.SCALE:
                        transform.position = partInEdit.transform.TransformPoint(new Vector3(0.5f, 0.5f, 0));
                        transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
                        //transform.localScale = Vector3.one * partInEdit.transform.localScale.y * 0.25f;
                    break;
                    }
            }
        }
    }

}