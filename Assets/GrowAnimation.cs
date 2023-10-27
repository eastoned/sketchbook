using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowAnimation : MonoBehaviour
{
    [Range(0,1)]
    public float growth;

    public Material mat;
    public Transform scale;

    void OnValidate(){
        mat.SetFloat("_Growth", growth);
        //scale.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, growth);
    }
}
