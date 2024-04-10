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
        offset = transform.localPosition - mouseDelta2;
        currentlyHeld = true;
    }

    void OnMouseDrag(){
        if(!currentlyHeld){
            //if(CustomUtils.IsPointerOverUIObject())
              //  return;
        }

        mouseDelta2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = new Vector3(mouseDelta2.x, mouseDelta2.y, transform.position.z);

        switch(controls){
            case TransformController.TRANSLATE:
                OnTranslatePartController.Instance.Invoke(partInEdit, transform.position);
            break;
            case TransformController.ROTATION:
                OnRotatePartController.Instance.Invoke(transform.position);
            break;
            case TransformController.SCALE:
                //Debug.Log(transform.position);
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

    void Update()
    {
        if(partInEdit != null){
                switch(controls){
                    case TransformController.ROTATION:
                    transform.position = partInEdit.transform.TransformPoint(new Vector3(0.5f, 0, 0));
                    transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
                    transform.localScale = Vector3.one * partInEdit.transform.localScale.y * 0.25f;
                break;
                case TransformController.SCALE:
                    transform.position = partInEdit.transform.TransformPoint(new Vector3(0.5f, 0.5f, 0));
                    transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
                    transform.localScale = Vector3.one * partInEdit.transform.localScale.y * 0.25f;
                break;
            }
        }
    }

}