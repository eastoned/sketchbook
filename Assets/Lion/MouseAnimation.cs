using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MouseAnimation : MonoBehaviour
{
    private Animator anim;

    void Start(){
        if(!anim){
            anim = GetComponent<Animator>();
        }
        anim.StartPlayback();
    }

    void OnMouseDown(){
        anim.StopPlayback();
    }

    void OnMouseExit(){
        anim.StartPlayback();
    }
}
