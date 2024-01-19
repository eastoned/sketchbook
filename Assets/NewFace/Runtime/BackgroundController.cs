using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundController : MonoBehaviour
{
    void OnMouseDown(){
        OnMouseClickEvent.Instance.Invoke();
        
        if(EventSystem.current.IsPointerOverGameObject())
            return;
            
        OnDeselectedFacePartEvent.Instance.Invoke();
    }

    void OnMouseEnter(){
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        if(Input.GetMouseButton(0))
            return;

        OnHoveredNewFacePartEvent.Instance.Invoke(transform);
    }
}
