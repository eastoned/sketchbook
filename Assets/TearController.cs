using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearController : MonoBehaviour
{
    public Transform tearObject;

    void Update(){
        Vector3 eyeHeight = transform.TransformPoint(new Vector3(0, -.2f, 0f));
        Vector3 floorHeight = new Vector3(eyeHeight.x, -2f, -.4f);
        tearObject.position = (eyeHeight + floorHeight)/2f + Vector3.up*.3f;
        tearObject.localScale = new Vector3(.1f, eyeHeight.y - floorHeight.y, 1f);
    }
}
