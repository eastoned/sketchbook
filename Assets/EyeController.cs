using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeController : MonoBehaviour
{
    [SerializeField]
    private Transform _leftEyeControl, _rightEyeControl;

    private Vector3 _leftEyePosCache, _leftEyeSizeCache;

    [SerializeField]
    private float _posXScale, _posYScale;

    [SerializeField]
    private Material _eyeMaterial;

    //mapping params
    /*
    left eye
    x pos 0 0.5
    y pos 0.3 0.75
    width .01 .25
    height .02 .25

    right eye
    x pos .75 1
    y pos 0.3 0.75
    width .01 .25
    height .02 .25

    left sphere


        _LPosX
        _LPosY
        _LSizeX
        _LSizeY
        
        _RPosX
        _RPosY
        _RSizeX
        _RSizeY
    */

    private void Start()
    {
        _eyeMaterial.SetFloat("_LPosX", 0.25f);
        _eyeMaterial.SetFloat("_LPosY", 0.5f);

        _eyeMaterial.SetFloat("_LSizeX", 0.1f);
        _eyeMaterial.SetFloat("_LSizeY", 0.15f);

        _leftEyePosCache = _leftEyeControl.localPosition;
        _leftEyeSizeCache = _leftEyeControl.localScale;
    }

    // Update is called once per frame
    private void Update()
    {
        float _leftEyePosX = _posXScale*(_leftEyePosCache.x - _leftEyeControl.localPosition.x);
        float _leftEyePosY = _posYScale * (_leftEyePosCache.y - _leftEyeControl.localPosition.y);
        
        _eyeMaterial.SetFloat("_LPosX", 0.25f - _leftEyePosX);
        _eyeMaterial.SetFloat("_LPosY", 0.5f - _leftEyePosY);
    }
}
