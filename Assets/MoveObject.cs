using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4));
        //transform.position += Time.deltaTime* new Vector3(0, Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"));
    }
}
