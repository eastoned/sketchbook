using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeShape : MonoBehaviour
{
    LineRenderer line;
    public float avg;
    int pointCount;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        pointCount = line.positionCount;
    }
    void GetPointsAboveAverageHeightThreshold()
    {

    }

    private void Update()
    {
        float total = 0;
        for(int i = 0; i < pointCount; i++)
        {
            total += line.GetPosition(i).y;
        }
        avg = total / pointCount;
        
    }
}
