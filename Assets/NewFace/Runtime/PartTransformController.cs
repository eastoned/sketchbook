using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PartTransformController : MonoBehaviour
{

    public enum TransformController
    {
        TRANSLATE,
        ROTATION,
        SCALE,
        CUSTOMIZE,
        NOTHING
    }

    public Texture2D icon;

    public TransformController controls;

    public Vector3 mouseDelta2;
    public Vector3 offset;
    public bool currentlyHeld = false;

    public PartController partInEdit;
    public BodyPartController bodyPartInEdit;
    public Coroutine UpdateBodyPartControllersRoutine;

    public UnityEvent m_MyEvent;

    void Start()
    {
        if(icon != null)
        {
            GetComponent<Renderer>().material.SetTexture("_IconTex", icon);
        }

        if (m_MyEvent == null)
            m_MyEvent = new UnityEvent();

    }

    public void UpdateActivePart(PartController partToEdit)
    {

        bodyPartInEdit = null;
        partInEdit = null;

        if(UpdateBodyPartControllersRoutine != null)
        {
            StopCoroutine(UpdateBodyPartControllersRoutine);
        }

        if(partToEdit == null)
        {
            Disappear();
            return;
        }

        BodyPartController bpc = partToEdit.GetComponent<BodyPartController>();

        if(bpc != null)
        {
            bodyPartInEdit = bpc;
            UpdateBodyPartControllersRoutine = StartCoroutine(UpdateControllerPositionRoutine());
        }
        else
        {
            partInEdit = partToEdit;
        }

        UpdateControllerPositions();
    }

    private IEnumerator UpdateControllerPositionRoutine()
    {
        for(;;)
        {
            UpdateBodyPartControllerPositions();

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

        if(partInEdit == null && bodyPartInEdit == null)
            return;

        switch(controls)
        {
            case TransformController.CUSTOMIZE:
                m_MyEvent.Invoke();
            break;
        }
        
        offset = transform.position - pos;
        currentlyHeld = true;
    }

    void OnMouseDrag()
    {
        if(!currentlyHeld)
        {
            //if(CustomUtils.IsPointerOverUIObject())
              //  return;
        }

        mouseDelta2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        OnHandDrag(mouseDelta2);
    }

    public void OnHandDrag(Vector3 pos)
    {
    
        if(bodyPartInEdit == null)
            return;

        switch(controls)
        {
            case TransformController.TRANSLATE:
                transform.position = new Vector3(pos.x, bodyPartInEdit.lockedYaxis ? transform.position.y : pos.y, transform.position.z);
                Vector3 displacement = transform.position + (bodyPartInEdit.lockedYaxis ? Vector3.zero : offset);
                OnTranslatePartController.Instance.Invoke(bodyPartInEdit, displacement, true);
                break;
            case TransformController.ROTATION:
                Vector3 supposePos = new Vector3(pos.x, pos.y, transform.position.z);
                OnRotatePartController.Instance.Invoke(supposePos);
                break;
            case TransformController.SCALE:
                Vector3 supposeSclPos = new Vector3(pos.x, pos.y, transform.position.z);
                OnScalePartController.Instance.Invoke(bodyPartInEdit, supposeSclPos);
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

        if(partInEdit == null && bodyPartInEdit == null)
        {
            Disappear();
            return;
        }

        if(partInEdit != null)
        {
            if(controls != TransformController.CUSTOMIZE)
            {
                Disappear();
                return;
            }

            transform.position = partInEdit.transform.TransformPoint(new Vector3(-.45f, 0, 0)); 
            transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
        }
        else if(bodyPartInEdit != null)
        {
            UpdateBodyPartControllerPositions();
        }

    }

    public void UpdateBodyPartControllerPositions()
    {
        switch(controls)
        {
            case TransformController.ROTATION:
                if(bodyPartInEdit.customScaleAnchor != null)
                {
                    transform.position = bodyPartInEdit.customScaleAnchor.TransformPoint(bodyPartInEdit.rotateControllerPos);
                }
                else
                {
                    transform.position = bodyPartInEdit.transform.TransformPoint(bodyPartInEdit.rotateControllerPos);
                }
                transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
                break;

            case TransformController.SCALE:
                if(bodyPartInEdit.customScaleAnchor != null)
                {
                    transform.position = bodyPartInEdit.customScaleAnchor.TransformPoint(bodyPartInEdit.scaleControllerPos);
                }
                else
                {
                    transform.position = bodyPartInEdit.transform.TransformPoint(bodyPartInEdit.scaleControllerPos); 
                }
                transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
                break;

            case TransformController.CUSTOMIZE:
                if(bodyPartInEdit.customScaleAnchor != null)
                {
                    transform.position = bodyPartInEdit.customScaleAnchor.TransformPoint(new Vector3(-.45f, 0, 0));
                }
                else
                {
                    transform.position = bodyPartInEdit.transform.TransformPoint(new Vector3(-.45f, 0, 0)); 
                }
                transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
                break;
        }
    }
}