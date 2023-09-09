using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMover : MonoBehaviour
{
    Rigidbody rb;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        rb.AddForce(movement * 2f, ForceMode.Acceleration);
    }
}
