using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [SerializeField] private LineRenderer line;

    [SerializeField] private Transform obj1, obj2;

    [SerializeField] private AnimationCurve stretchCurve;

    [SerializeField] private float distance;

    private void Start(){
        line = GetComponent<LineRenderer>();
    }

    private void Update(){
        line.SetPosition(0, obj1.position);
        line.SetPosition(1, obj2.position);
        distance = Vector3.Distance(obj1.position, obj2.position);
        //line.widthMultiplier = stretchCurve.Evaluate(Vector3.Distance(obj1.position, obj2.position));
    }
}
