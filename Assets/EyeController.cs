using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeController : MonoBehaviour
{
    [SerializeField]
    private Transform _leftEyeControl, _rightEyeControl;

    private Vector3 _leftEyePosCache, _leftEyeSizeCache, _rightEyePosCache, _rightEyeSizeCache;

    [SerializeField]
    private float _posXScale, _posYScale, _sizeXScale, _sizeYScale;

    [SerializeField]
    private Material _eyeMaterial;

    private float _eyeTimer;

    float _leftEyePosX;
    float _leftEyePosY;
    float _leftEyeSizeX;
    float _leftEyeSizeY;

    float _rightEyePosX;
    float _rightEyePosY;
    float _rightEyeSizeX;
    float _rightEyeSizeY;

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
        _eyeTimer = Random.Range(.2f, 2f);
        _eyeMaterial.SetFloat("_LPosX", 0.25f);
        _eyeMaterial.SetFloat("_LPosY", 0.5f);

        _eyeMaterial.SetFloat("_LSizeX", 0.1f);
        _eyeMaterial.SetFloat("_LSizeY", 0.15f);

        _leftEyePosCache = _leftEyeControl.localPosition;
        _leftEyeSizeCache = _leftEyeControl.localScale;

        _rightEyePosCache = _rightEyeControl.localPosition;
        _rightEyeSizeCache = _rightEyeControl.localScale;
    }

    // Update is called once per frame
    private void Update()
    {/*
        float _leftEyePosX = 0.25f - (_posXScale * (_leftEyePosCache.x - _leftEyeControl.localPosition.x));
        _leftEyePosX = Mathf.Clamp(_leftEyePosX, 0, 0.5f);
        float _leftEyePosY = 0.5f - (_posYScale * (_leftEyePosCache.y - _leftEyeControl.localPosition.y));
        _leftEyePosY = Mathf.Clamp(_leftEyePosY, 0.3f, 0.75f);

        float _leftEyeSizeX = .1f - (_sizeXScale*(_leftEyeSizeCache.x - _leftEyeControl.localScale.x));
        _leftEyeSizeX = Mathf.Clamp(_leftEyeSizeX, .01f, .25f);
        float _leftEyeSizeY = .15f - ( _sizeYScale*(_leftEyeSizeCache.y - _leftEyeControl.localScale.y));
        _leftEyeSizeY = Mathf.Clamp(_leftEyeSizeY, 0.02f, .25f);

        float _rightEyePosX = 0.75f - (_posXScale * (_rightEyePosCache.x - _rightEyeControl.localPosition.x));
        _rightEyePosX = Mathf.Clamp(_rightEyePosX, 0.5f, 1f);
        float _rightEyePosY = 0.5f - (_posYScale * (_rightEyePosCache.y - _rightEyeControl.localPosition.y));
        _rightEyePosY = Mathf.Clamp(_rightEyePosY, 0.3f, 0.75f);

        float _rightEyeSizeX = .1f - (_sizeXScale * (_rightEyeSizeCache.x - _rightEyeControl.localScale.x));
        _rightEyeSizeX = Mathf.Clamp(_rightEyeSizeX, .01f, .25f);
        float _rightEyeSizeY = .15f - (_sizeYScale * (_rightEyeSizeCache.y - _rightEyeControl.localScale.y));
        _rightEyeSizeY = Mathf.Clamp(_rightEyeSizeY, 0.02f, .25f);*/


        /*if(_eyeTimer < 0) {
            _leftEyePosX = Random.Range(0f, .5f);
            _leftEyePosY = Random.Range(.3f, .75f);
            _leftEyeSizeX = Random.Range(.01f, .25f);
            _leftEyeSizeY = Random.Range(.02f, .25f);
            _eyeTimer = Random.Range(.2f, 2f);
        }*/

        _leftEyePosX = Mathf.PerlinNoise(Time.time, 0);
        _leftEyePosY = Mathf.PerlinNoise(Time.time, 1);
        _leftEyeSizeX = Mathf.PerlinNoise(Time.time, 2);
        _leftEyeSizeY = Mathf.PerlinNoise(Time.time, 3);

        
        _leftEyePosY = Mathf.Lerp(.3f, .75f, _leftEyePosY);
        _leftEyeSizeX = Mathf.Lerp(.01f, .25f, _leftEyeSizeX);
        _leftEyeSizeY = Mathf.Lerp(.02f, .25f, _leftEyeSizeY);

        _rightEyePosX = Mathf.Lerp(.5f, 1f, _leftEyePosX);
        _rightEyePosY = _leftEyePosY;
        _rightEyeSizeX = _leftEyeSizeX;
        _rightEyeSizeY = _leftEyeSizeY;

        _eyeTimer -= Time.deltaTime;

        _leftEyePosX = Mathf.Lerp(0f, .5f, _leftEyePosX);

        _eyeMaterial.SetFloat("_LPosX", _leftEyePosX);
        _eyeMaterial.SetFloat("_LPosY", _leftEyePosY);
        _eyeMaterial.SetFloat("_LSizeX", _leftEyeSizeX);
        _eyeMaterial.SetFloat("_LSizeY", _leftEyeSizeY);

        _eyeMaterial.SetFloat("_RPosX", _rightEyePosX);
        _eyeMaterial.SetFloat("_RPosY", _rightEyePosY);
        _eyeMaterial.SetFloat("_RSizeX", _rightEyeSizeX);
        _eyeMaterial.SetFloat("_RSizeY", _rightEyeSizeY);
    }
}
