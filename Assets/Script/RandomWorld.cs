using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWorld : MonoBehaviour
{
    LineRenderer lr;
    [SerializeField]int points;
    public Vector3[] pointArray = new Vector3[10];
    float[] speeds = new float[10];
    public float overallSpeed;
    // Start is called before the first frame update
    void Start()
    {
        pointArray = new Vector3[points*3];
        speeds = new float[points];
        lr = GetComponent<LineRenderer>();
        lr.positionCount = points * 3;
        for(int i = 0; i < points*3; i+=3)
        {
            pointArray[i] = new Vector3(Random.Range(-5f, 5f), -1, Random.Range(-5f, 5f));
            pointArray[i + 1] = pointArray[i] + new Vector3(0, Random.Range(1, 10), 0);
            pointArray[i + 2] = pointArray[i];
            
            Debug.Log(i);
        }
        for(int i = 0; i < points; i++)
        {
            speeds[i] = Random.value;
        }
        lr.SetPositions(pointArray);

    }

    void Update()
    {
        /*for (int i = 1; i < points; i += 3)
        {
            pointArray[i] = Vector3.Lerp(pointArray[i], pointArray[i-1], overallSpeed*speeds[i]*Time.deltaTime);

        }
        lr.SetPositions(pointArray);*/
    }

}
