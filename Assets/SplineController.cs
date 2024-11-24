using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class SplineController : MonoBehaviour
{
    [SerializeField] private SplineContainer sc;
    public List<GameObject> controllers;

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < sc.Spline.Knots.Count(); i++)
        {
            //sc.Spline.Knots[i]
            //sc.Spline.Knots.ElementAt(i).Position = controllers[i].transform.position;
        }
    }
}
