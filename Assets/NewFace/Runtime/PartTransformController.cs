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

        mouseDelta2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        OnHandDown(mouseDelta2);
    }

    public void OnHandDown(Vector3 pos)
    {
        OnSetTransformCacheEvent.Instance.Invoke();
        offset = transform.position - pos;
        currentlyHeld = true;
    }

    void OnMouseDrag(){
        if(!currentlyHeld){
            //if(CustomUtils.IsPointerOverUIObject())
              //  return;
        }

        mouseDelta2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        OnHandDrag(mouseDelta2);
    }

    public void OnHandDrag(Vector3 pos){
        switch(controls){
            case TransformController.TRANSLATE:
                transform.position = new Vector3(pos.x, pos.y, transform.position.z);
                if(partInEdit.customScaleAnchor != null){
                    //offset += transform.localPosition;
                }
                OnTranslatePartController.Instance.Invoke(partInEdit, transform.position + offset, true);
            break;
            case TransformController.ROTATION:
                transform.position = new Vector3(pos.x, pos.y, transform.position.z);
                OnRotatePartController.Instance.Invoke(transform.position);
            break;
            case TransformController.SCALE:
                transform.position = new Vector3(pos.x, pos.y, transform.position.z);
                OnScalePartController.Instance.Invoke(transform.position);
            break;
            case TransformController.NOTHING:
            break;
        }
    }

    void OnMouseUp(){
        OnHandUp();
        //OnConfirmTransformPart.Instance.Invoke();
    }
    public void OnHandUp(){
        currentlyHeld = false;
    }

    public void Disappear(){
        transform.localPosition = new Vector3(100, 100, 100);
    }

    public void UpdateControllerPositions()
    {
        switch(controls){
            case TransformController.ROTATION:
                if(partInEdit.customScaleAnchor != null)
                {
                    transform.position = partInEdit.customScaleAnchor.TransformPoint(new Vector3(0.5f, 0, 0));
                }
                else
                {
                    Debug.Log("moving bangs");
                    Debug.Log(transform.name + " : " + partInEdit.transform.position);
                    transform.position = partInEdit.transform.TransformPoint(new Vector3(0.5f, 0, 0));
                }
                transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
            break;

            case TransformController.SCALE:
                if(partInEdit.customScaleAnchor != null)
                {
                    transform.position = partInEdit.customScaleAnchor.TransformPoint(new Vector3(0.5f, 0.5f, 0));
                }
                else
                {
                    transform.position = partInEdit.transform.TransformPoint(new Vector3(0.5f, 0.5f, 0)); 
                }
                transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
            break;
        }
    }

    void Update()
    {
        if(partInEdit != null){
            if(partInEdit.detached){
                    switch(controls){
                        case TransformController.ROTATION:
                        transform.position = partInEdit.transform.TransformPoint(new Vector3(0.5f, 0, 0));
                        transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
                        //transform.localScale = Vector3.one * partInEdit.transform.localScale.y * 0.25f;
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