using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneController : MonoBehaviour
{

    [SerializeField]
    private Rigidbody _phoneRB;

    [SerializeField]
    private Camera _cam;

    [SerializeField]
    private float _zoomSpeed;

    // Update is called once per frame
    void Update()
    {
        _cam.fieldOfView += _zoomSpeed * Input.mouseScrollDelta.y;
    }
}
