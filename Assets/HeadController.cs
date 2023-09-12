using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class HeadController : MonoBehaviour
{

    public float lowEnd, highEnd;
    public float mouseDelta;
    public float interval;

    public enum DirAxis{
        UpDown,
        LeftRight
    }
    public DirAxis axis;

    public FaceController fc;

    void OnMouseDrag(){

        if(axis == DirAxis.UpDown){
            mouseDelta = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        
            transform.localPosition = new Vector3(0, Mathf.Clamp(mouseDelta, lowEnd, highEnd), 0.1f);
            interval = Mathf.InverseLerp(lowEnd, highEnd, mouseDelta);
            fc.UpdateHeightFromTransformTool(interval);
        }else if (axis == DirAxis.LeftRight){
            mouseDelta = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        
            transform.localPosition = new Vector3(Mathf.Clamp(mouseDelta, lowEnd, highEnd), 0, 0.1f);
            interval = Mathf.InverseLerp(lowEnd, highEnd, mouseDelta);
            fc.UpdateWidthFromTransformTool(interval);
        }
        
    }

    public void UpdatePosition(float valueChange){
        float change = Mathf.Lerp(lowEnd, highEnd, valueChange);
        if(axis == DirAxis.UpDown){
            transform.localPosition = new Vector3(0, change, 0.1f);
        }else if (axis == DirAxis.LeftRight){
            transform.localPosition = new Vector3(change, 0, 0.1f);
        }
    }

}
