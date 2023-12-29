using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundController : MonoBehaviour
{
    void OnMouseDown(){
        OnMouseClickEvent.Instance.Invoke();
        
        if(EventSystem.current.IsPointerOverGameObject() || EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return;
            
        OnDeselectedFacePartEvent.Instance.Invoke();
    }
}
