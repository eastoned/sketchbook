using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private Material handMaterial;
    [SerializeField] private Transform handTransform;

    [SerializeField] private bool hoveringOnEditablePart = false;
    private Coroutine fingerAnimation;

    enum InteractableState
    {
        OPEN = 0,
        HOVERED = 1,
        CLICKED = 2
    }
    [SerializeField] private InteractableState state;

    void Start()
    {
        Cursor.visible = false;
    }

    void OnEnable()
    {
        OnHoveredNewFacePartEvent.Instance.AddListener(HoveringOnEditablePart);
    }

    void OnDisable()
    {
        OnHoveredNewFacePartEvent.Instance.RemoveListener(HoveringOnEditablePart);
    }

    void HoveringOnEditablePart(PartController part)
    {
        if(part != null)
        {
            hoveringOnEditablePart = true;
        }
        else
        {
            hoveringOnEditablePart = false;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        //Hand is clicking
        if(Input.GetMouseButton(0))
        {
            handMaterial.SetFloat("_Finger1", 0);
            handMaterial.SetFloat("_Finger2", 0);
            handMaterial.SetFloat("_Finger3", 0);
            handMaterial.SetFloat("_Finger4", 0);
            //UpdateInteractableState(InteractableState.CLICKED);
        }
        else if(hoveringOnEditablePart)
        {
            handMaterial.SetFloat("_Finger1", Mathf.PingPong(Time.time*1.5f, 1));
            handMaterial.SetFloat("_Finger2", Mathf.PingPong(Time.time*1.5f + .25f, 1));
            handMaterial.SetFloat("_Finger3", Mathf.PingPong(Time.time*1.5f + .5f, 1));
            handMaterial.SetFloat("_Finger4", Mathf.PingPong(Time.time*1.5f + .75f, 1));
            //UpdateInteractableState(InteractableState.HOVERED);
        }
        else
        {
            handMaterial.SetFloat("_Finger1", 1);
            handMaterial.SetFloat("_Finger2", 0);
            handMaterial.SetFloat("_Finger3", 0);
            handMaterial.SetFloat("_Finger4", 0);
            //UpdateInteractableState(InteractableState.OPEN);
        }

        handTransform.position = Input.mousePosition;
    }

    void UpdateInteractableState(InteractableState newState)
    {
        if(state == newState)
        {
            // State is the same
            return;
        }

        switch(newState)
        {
            case InteractableState.OPEN:
                StopCoroutine(fingerAnimation);
                
            break;
            case InteractableState.HOVERED:
                if(fingerAnimation == null)
                    fingerAnimation = StartCoroutine(FingerAnimation());
            break;
            case InteractableState.CLICKED:
                StopCoroutine(fingerAnimation);
                
            break;
        }
    }

    private IEnumerator FingerAnimation()
    {   
        for(;;)
        {
            
            yield return null;
        }
    }
}
