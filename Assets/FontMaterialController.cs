using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontMaterialController : MonoBehaviour
{
    public Material mat;
    [Range(-6f, 6f)]
    public float dilation;
    public float timer;

    public AnimationCurve textOpacity;

    private void OnEnable(){
        SnapEvent.Instance.AddListener(FadeInText);
    }
    private void OnDisable(){
        SnapEvent.Instance.RemoveListener(FadeInText);
    }

    void OnValidate(){
        mat.SetFloat("_FaceDilate", dilation);
    }

    void FadeInText(){
        timer = 0;
        StartCoroutine(FadeInTextRoutine(4f));
    }

    IEnumerator FadeInTextRoutine(float time){
        while(timer < time){
            timer += Time.deltaTime;
            mat.SetFloat("_FaceDilate", textOpacity.Evaluate(timer));
            yield return null;
        }
        
    }
}
