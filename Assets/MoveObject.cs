using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{

    public bool _softerNudge = false;
    private void OnMouseDrag()
    {
        if (!_softerNudge) { transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4)); } else {
            transform.position += Time.deltaTime * 10f * Camera.main.transform.right * Input.GetAxis("Mouse X");
            transform.position += Time.deltaTime * 10f * Camera.main.transform.up * Input.GetAxis("Mouse Y");//;Time.deltaTime *10f* new Vector3(0, Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X")); 
        }
        
        //
    }
}
