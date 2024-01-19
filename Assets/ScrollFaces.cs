using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollFaces : MonoBehaviour
{
    public Transform[] faces;

    public float startPos, currentPos;

    public float[] cacheFaces;

    void OnMouseDown(){
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        cacheFaces = new float[faces.Length];
        startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        for(int i = 0; i < faces.Length; i++){
            cacheFaces[i] = faces[i].transform.position.y;
        }
    }
    void OnMouseDrag(){
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - startPos;
        for(int i = 0; i < faces.Length; i++){
            faces[i].position = new Vector3(faces[i].position.x, cacheFaces[i] + currentPos, faces[i].position.z);
        }
    }
}
