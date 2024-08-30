using System.Collections;
using UnityEngine;

public class PartTransformController : MonoBehaviour
{

    public enum TransformController
    {
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

    public BodyPartController partInEdit;

    public Vector3 testInput;

    void Start()
    {
        if(icon != null)
        {
            GetComponent<Renderer>().material.SetTexture("_IconTex", icon);
        }

        StartCoroutine(UpdateControllers());
    }

    private IEnumerator UpdateControllers()
    {
        for(;;)
        {
            if(partInEdit != null && !partInEdit.detached)
            {
                    switch(controls)
                    {
                        case TransformController.ROTATION:
                        
                        transform.position = partInEdit.transform.TransformPoint(partInEdit.rotateControllerPos);
                        transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
                        //transform.localScale = Vector3.one * partInEdit.transform.localScale.y * 0.25f;
                    break;
                    case TransformController.SCALE:
                        transform.position = partInEdit.transform.TransformPoint(partInEdit.scaleControllerPos);
                        transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
                        //transform.localScale = Vector3.one * partInEdit.transform.localScale.y * 0.25f;
                    break;
                    }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
    
    void OnMouseDown()
    {
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

    void OnMouseDrag()
    {
        if(!currentlyHeld){
            //if(CustomUtils.IsPointerOverUIObject())
              //  return;
        }

        mouseDelta2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        OnHandDrag(mouseDelta2);
    }

    public void OnHandDrag(Vector3 pos)
    {
        switch(controls){
            case TransformController.TRANSLATE:
                transform.position = new Vector3(pos.x, partInEdit.lockedYaxis ? transform.position.y : pos.y, transform.position.z);
                
                if(partInEdit.customScaleAnchor != null)
                {
                    //offset += transform.localPosition;
                }
                
                Vector3 displacement = transform.position + (partInEdit.lockedYaxis ? Vector3.zero : offset);
                
                OnTranslatePartController.Instance.Invoke(partInEdit, displacement, true);
                
            break;
            case TransformController.ROTATION:
                Vector3 supposePos = new Vector3(pos.x, pos.y, transform.position.z);
                OnRotatePartController.Instance.Invoke(supposePos);
            break;
            case TransformController.SCALE:
                Vector3 supposeSclPos = new Vector3(pos.x, pos.y, transform.position.z);
                OnScalePartController.Instance.Invoke(partInEdit, supposeSclPos);
            break;
            case TransformController.NOTHING:
            break;
        }
    }

    void OnMouseUp()
    {
        OnHandUp();
        //OnConfirmTransformPart.Instance.Invoke();
    }
    public void OnHandUp()
    {
        currentlyHeld = false;
    }

    public void Disappear()
    {
        transform.localPosition = new Vector3(100, 100, 100);
    }

    public void UpdateControllerPositions()
    {
        switch(controls){
            case TransformController.ROTATION:
                if(partInEdit.customScaleAnchor != null)
                {
                    transform.position = partInEdit.customScaleAnchor.TransformPoint(partInEdit.rotateControllerPos);
                }
                else
                {
                    //Debug.Log("moving bangs");
                    //Debug.Log(transform.name + " : " + partInEdit.transform.position);
                    transform.position = partInEdit.transform.TransformPoint(partInEdit.rotateControllerPos);
                }
                transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
            break;

            case TransformController.SCALE:
                if(partInEdit.customScaleAnchor != null)
                {
                    transform.position = partInEdit.customScaleAnchor.TransformPoint(partInEdit.scaleControllerPos);
                }
                else
                {
                    transform.position = partInEdit.transform.TransformPoint(partInEdit.scaleControllerPos); 
                }
                transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
            break;
        }
    }
}