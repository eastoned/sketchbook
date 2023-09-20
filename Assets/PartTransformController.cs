using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartTransformController : MonoBehaviour
{
    public Vector3 mouseDelta2;
    void OnMouseDrag(){
        mouseDelta2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        transform.localPosition = new Vector3(mouseDelta2.x, mouseDelta2.y, 0f);
        OnTranslatePartController.Instance.Invoke(transform.localPosition);
    }

}
