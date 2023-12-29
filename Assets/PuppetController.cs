using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float rotSpeed;
    void OnMouseDrag(){
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    void Update(){

        if(Input.GetKey(KeyCode.A)){
            transform.Rotate(new Vector3(0, 0, rotSpeed*Time.deltaTime));
        }
        if(Input.GetKey(KeyCode.D)){
            transform.Rotate(new Vector3(0, 0, -rotSpeed*Time.deltaTime));
        }
    }
}
