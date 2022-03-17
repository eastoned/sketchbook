using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPhoneTransform : MonoBehaviour
{
    [SerializeField]
    private Transform _phonePos;

    private float _phoneOriginalDepth, _cameraOriginalDepth;



    private void Start()
    {
        _phoneOriginalDepth = _phonePos.position.x;
        _cameraOriginalDepth = transform.position.x;
    }

    private void Update()
    {

        float _depthDelta = _phonePos.position.x - _phoneOriginalDepth;
        transform.position = new Vector3(_cameraOriginalDepth - _depthDelta, _phonePos.position.y, _phonePos.position.z);
        transform.rotation = Quaternion.Euler(_phonePos.eulerAngles.z, 90 - _phonePos.eulerAngles.y, _phonePos.eulerAngles.x);
        //transform.localEulerAngles = new Vector3(_phonePos.eulerAngles.z, 90 - _phonePos.eulerAngles.y, _phonePos.eulerAngles.x);
    }
}
