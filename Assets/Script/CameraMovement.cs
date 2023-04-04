using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float mouseSens = 50f;
    public float speed = 5f;
    public float fwdSpeed, rightSpeed;
    // Update is called once per frame
    void Update()
    {
        //dir = Vector3.zero;
        fwdSpeed = 0;

        if (Input.GetKey(KeyCode.W))
        {
            fwdSpeed += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            fwdSpeed -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rightSpeed = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rightSpeed = 1;
        }
        Debug.DrawRay(transform.position, transform.forward);
        //transform.Translate(dir * speed * Time.deltaTime);
        transform.localEulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime, 0);
    }
}
