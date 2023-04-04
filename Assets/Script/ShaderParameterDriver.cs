using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderParameterDriver : MonoBehaviour
{

    [SerializeField]
    private Renderer _rend;

    [SerializeField]
    private string _param;

    [SerializeField]
    [Range(0, 1)]
    private float _gameTime;

    // Update is called once per frame
    void Update()
    {
        _gameTime += Time.deltaTime;
        _rend.material.SetFloat(_param, _gameTime/25f);
    }
}
