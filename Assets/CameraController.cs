using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector2 mousePos;
    public float amount;

    // Update is called once per frame
    void Update()
    {
        mousePos = new Vector2(Input.mousePosition.x/Screen.width - 0.5f, Input.mousePosition.y/Screen.height - 0.5f);

        transform.localEulerAngles = new Vector3(-mousePos.y * amount, mousePos.x * amount, 0);
    }
}
