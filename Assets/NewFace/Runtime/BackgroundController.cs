using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundController : MonoBehaviour
{
    void OnMouseDown(){
        OnMouseClickEvent.Instance.Invoke();
        
        if(IsPointerOverUIObject())
            return;
            
        OnDeselectedFacePartEvent.Instance.Invoke();
    }

    private bool IsPointerOverUIObject() {
         PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
         eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
         List<RaycastResult> results = new List<RaycastResult>();
         EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
         return results.Count > 0;
     }

    void OnMouseEnter(){
        OnHoveredNewFacePartEvent.Instance.Invoke(null);
    }
}
