using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongBall : MonoBehaviour
{
    public Vector3 direction;
    public float amount;
    public long startTime;

    private void Start()
    {
        direction = GiveRandomVector3Upwards();
        startTime = System.DateTime.Now.Ticks;
    }

    private void Update()
    {
        
        //transform.Translate(direction * amount);
    }

    private void OnMouseDown()
    {
        Debug.Log(System.DateTime.Now.Ticks-startTime);
        startTime = System.DateTime.Now.Ticks;
        direction = GiveRandomVector3Upwards();
        transform.Translate(direction * amount);
    }

    Vector3 GiveRandomVector3Upwards()
    {
        Vector3 direction = Random.onUnitSphere;
        direction = new Vector3(direction.x, Mathf.Abs(direction.y), direction.z);
        return direction;
    }
}
