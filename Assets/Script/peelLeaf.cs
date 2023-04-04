using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class peelLeaf : MonoBehaviour
{
    private Renderer renderer;
    private Rigidbody rb;


    public float dragX, dragY;
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();

        //transform.position += new Vector3(Random.Range(-3f, 3f), Random.Range(-5f, 5f), 0);
    }

    void OnMouseDrag()
    {

        //transform.position = Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime * 5f);
       // transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            dragX += Input.GetAxis("Mouse X") ;
            dragY += Input.GetAxis("Mouse Y") ;



        Debug.Log(dragX);
        if(Mathf.Abs(dragX) >= 0.1f){
           // collider.enabled = false;
          //  rb.isKinematic = false;
        }
    }

    private void Update()
    {
        dragX = Mathf.Lerp(dragX, 0, Time.deltaTime/5f);
        dragY = Mathf.Lerp(dragY, 0, Time.deltaTime/5f);
        renderer.material.SetFloat("_HorAmount", dragX);
        renderer.material.SetFloat("_VertAmount", dragY);

    }


}
