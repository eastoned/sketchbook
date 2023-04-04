using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTarget : MonoBehaviour
{
    public Transform _target;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime);
    }
}
