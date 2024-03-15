using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector2 mousePos;
    public float amount;

    public float zoom = 2f;

    void Start(){
        zoom = 2f;
        Camera.main.orthographicSize = zoom;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = new Vector2(Input.mousePosition.x/Screen.width - 0.5f, Input.mousePosition.y/Screen.height - 0.5f);

        //transform.localEulerAngles = new Vector3(-mousePos.y * amount, mousePos.x * amount*2f, 0);

        zoom -= Input.GetAxis("Mouse ScrollWheel");
        zoom = Mathf.Clamp(zoom, 0.5f, 2f);
        
        Camera.main.orthographicSize = zoom;
    }
}
