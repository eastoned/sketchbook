using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EyebrowShape : MonoBehaviour
{
    //has to be at same level or above eyes
    //
    LineRenderer line;

    [Range(0.01f, 0.5f)]
    [SerializeField] float eyebrowWidth;

    private void Start()
    {
        line = GetComponent<LineRenderer>();

    }
    private void OnValidate()
    {
        line.widthMultiplier = eyebrowWidth;
    }
}
