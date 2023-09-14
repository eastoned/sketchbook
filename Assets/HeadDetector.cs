using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadDetector : MonoBehaviour
{

    public List<MeshRenderer> headControls = new List<MeshRenderer>();

    void OnMouseOver(){
        Debug.Log("Hovering over head");
        foreach(MeshRenderer mr in headControls){
            mr.enabled = true;
        }
    }

    void OnMouseExit(){
        foreach(MeshRenderer mr in headControls){
            mr.enabled = false;
        }
    }
}
