using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControlSpriteStretch : MonoBehaviour
{

    public Material mat;
    public bool horizontal;
    float xDisplacement, yDisplacement, xThresh, yThresh;

    float tarX = 0;
    float tarY = 0;
    // Start is called before the first frame update
    void Start()
    {
        horizontal = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if movment is updown send to y value

        //if movment is leftright sent to x value
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                //Debug.Log(hit.collider.name);
                tarX = hit.textureCoord.x;
                tarY = hit.textureCoord.y;
                Debug.Log(hit.textureCoord);
            }

        }

        xThresh = Mathf.Lerp(xThresh, tarX, Time.deltaTime);
        yThresh = Mathf.Lerp(yThresh, tarY, Time.deltaTime);
    }

    void OnMouseDrag()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
        {
            horizontal = true;
        }
        else
        {
            horizontal = false;
        }

        if (horizontal)
        {

            xDisplacement -= mouseX;
        }
        else{
            yDisplacement -= mouseY;

        }
        mat.SetFloat("_xAmount", xDisplacement);
        mat.SetFloat("_yAmount", yDisplacement);
        mat.SetFloat("_xThresh", xThresh);
        mat.SetFloat("_yThresh", yThresh);

    }
}
