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
            if(CustomUtils.IsPointerOverUIObject())
                return;
        }

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

    void Update(){

        if(partInEdit != null){
                switch(controls){
                    case TransformController.ROTATION:
                    transform.localPosition = partInEdit.transform.TransformPoint(new Vector3(0.5f, 0, 0));
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
                    transform.localScale = Vector3.one * partInEdit.transform.localScale.y * 0.25f;
                break;
                case TransformController.SCALE:
                    transform.localPosition = partInEdit.transform.TransformPoint(new Vector3(0.5f, 0.5f, 0));
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
                    transform.localScale = Vector3.one * partInEdit.transform.localScale.y * 0.25f;
                break;
            }
        }

        /*
        if(gravity){
            transform.localPosition += velocity * Time.deltaTime;
            velocity -= new Vector3(0, .1f, 0);
        }

        if (transform.localPosition.y < -2.25f){
            gravity = false;
            velocity = Vector3.zero;
            transform.localPosition = new Vector3(1, -1.5f, -0.25f);
        }
        if(controls == TransformController.NOTHING){
            if(mouth.OverlapPoint(transform.localPosition)){
                Debug.Log("Eating");
            }else{
                //Debug.Log("Not Eating");
            }
        }*/
        
    }

/*
    void OnMouseUp(){
        currentlyHeld = false;
        //OnConfirmTransformPart.Instance.Invoke();

        if(mouth.OverlapPoint(transform.localPosition)){
            Debug.Log("Eating");
            gravity = false;
            velocity = Vector3.zero;
            transform.localPosition = new Vector3(1, -1.5f, -0.25f);
        }else{
            if(controls == TransformController.NOTHING){
                gravity = true;
            }
        }

        
    }*/
}
