using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class edible : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnMouseDrag()
    {

        transform.position = Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime * 5f);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
       
    }
}
