using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenColliders : MonoBehaviour
{

    public Transform leftWall, rightWall, ceiling, floor;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScreenColliders();
    }

    void UpdateScreenColliders()
    {
        transform.localScale = new Vector3((float)Screen.width/Screen.height * 4, 4, 1);
        ceiling.localScale = transform.localScale;
        floor.localScale = transform.localScale;
        leftWall.position = new Vector3(transform.localScale.x/2f + 2f, 0, 101);
        rightWall.position = new Vector3(-leftWall.position.x, 0, 101);
    }
}
