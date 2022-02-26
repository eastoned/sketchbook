using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneTransformController : MonoBehaviour
{


    [SerializeField]
    private float _depthMovement = 0;

    [SerializeField]
    private float _fwdSpeed = 1;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _depthMovement = -1;
        }else if (Input.GetKey(KeyCode.S))
        {
            _depthMovement = 1;
        }
        else
        {
            _depthMovement = 0;
        }

        _depthMovement *= (_fwdSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x + _depthMovement, transform.position.y, transform.position.z); 
    }

    private void OnMouseDrag()
    {
        Debug.Log(Input.mousePosition);
        
    }
}
