using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPhoneTransform : MonoBehaviour
{
    [SerializeField]
    private Transform _phonePos;

    [SerializeField]
    private LikeMyBody _gm;

    private float _phoneOriginalDepth, _cameraOriginalDepth;

    private Vector3 _lookAtTarget;

    private void Start()
    {
        _phoneOriginalDepth = _phonePos.position.x;
        _cameraOriginalDepth = transform.position.x;
        _lookAtTarget = _gm.avg;
    }

    private void Update()
    {

        float _depthDelta = _phonePos.position.x - _phoneOriginalDepth;
        transform.position = new Vector3(_cameraOriginalDepth - _depthDelta, _phonePos.position.y, _phonePos.position.z);

        //_lookAtTarget = Vector3.Lerp(_lookAtTarget, _gm.avg, Time.deltaTime * 5f);

        //transform.LookAt(_lookAtTarget);
        //transform.rotation = Quaternion.Euler(_phonePos.eulerAngles.z, 90 - _phonePos.eulerAngles.y, _phonePos.eulerAngles.x);
        //transform.localEulerAngles = new Vector3(_phonePos.eulerAngles.z, 90 - _phonePos.eulerAngles.y, _phonePos.eulerAngles.x);
    }
}
