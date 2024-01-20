using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollFaces : MonoBehaviour
{
    public Transform[] faces;

    public float startPos, currentPos;

    public float[] cacheFaces;

    public Transform face1;

    public bool visible = false;

    void OnMouseUp(){
        //StartCoroutine(AnimateFacePos(.5f, new Vector3(0, -.5f, -1f), new Vector3(0, -3f, -1f)));
    }

    public void ToggleFacePos(){
        if(visible){
            StartCoroutine(AnimateFacePos(.5f, new Vector3(0, 0f, -1f), new Vector3(0, -5f, -1f)));
            visible = false;
        }else{
            StartCoroutine(AnimateFacePos(.5f, new Vector3(0, -5f, -1f), new Vector3(0, 0, -1f)));
            visible = true;
        }
    }

    private IEnumerator AnimateFacePos(float animLength, Vector3 startPos, Vector3 endPos){
        float animationTime = 0;
        while(animationTime < animLength){
            float interval = Mathf.Clamp01(animationTime/animLength);
            face1.position = Vector3.Lerp(startPos, endPos, interval);
            animationTime += Time.deltaTime;
            yield return null;
        }
    }

    void OnMouseDown(){
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        //StartCoroutine(AnimateFacePos(.5f, new Vector3(0, -3f, -1f), new Vector3(0, -0.5f, -1f)));
        //cacheFaces = new float[faces.Length];
        //startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        for(int i = 0; i < faces.Length; i++){
           // cacheFaces[i] = faces[i].transform.position.y;
        }
    }
    void OnMouseDrag(){
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        //currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - startPos;
        for(int i = 0; i < faces.Length; i++){
            //faces[i].position = new Vector3(faces[i].position.x, cacheFaces[i] + currentPos, faces[i].position.z);
        }
    }
}
