using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class callNewGrid : MonoBehaviour
{
    public tileGenerator tileReset;
    public Vector3 rotateHandle;

    private void Start()
    {
        rotateHandle = transform.GetChild(0).transform.localEulerAngles;
    }
    // Start is called before the first frame update
    private void OnMouseDown()
    {
        tileReset.newGrid = true;
        rotateHandle = new Vector3(0, 0, 140);
        Debug.Log("new grid");
    }

    public void Update()
    {
        transform.GetChild(0).transform.localEulerAngles = rotateHandle;
        rotateHandle = Vector3.Lerp(rotateHandle, Vector3.zero, Time.deltaTime);
    }
}
