using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DatamoshEffect : MonoBehaviour
{

    public Material DMmat;
    public Vector4 amount = new Vector4(0, 0, 0, 0);
    public Vector3 screenPoint = new Vector3(0, 0, 0);
    Vector3 screenPos;
    public bool setDir;
    public Color col, currentCol;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.MotionVectors;
        Shader.SetGlobalInt("_Button", 0);
        setDir = false;
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, DMmat);
    }

    void Update()
    {
        //Shader.SetGlobalInt("_Button", Input.GetButton("Fire1") ? 1 : 0);
        if (Input.GetMouseButtonDown(0))
        {
            Shader.SetGlobalInt("_Button", 1);
            screenPoint = Input.mousePosition;

            screenPoint.y /= Screen.height;
            screenPoint.x /= Screen.width;
            DMmat.SetVector("_MousePosition", screenPoint);
            Debug.Log("help");
            col = Random.ColorHSV();


        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shader.SetGlobalInt("_Button", 0);
        }

        if (Input.GetMouseButtonUp(0))
        {
            setDir = false;

        }

        if (Input.GetMouseButton(0))
        {
            screenPos = Input.mousePosition;
            screenPos.x /= Screen.width;
            screenPos.y /= Screen.height;
            //currentCol = Color.Lerp(currentCol, col, Time.deltaTime/1000);
            amount.x = 0;
            amount.y = 0;
            //left right translation
            if(Mathf.Abs(Input.GetAxis("Mouse X")) > Mathf.Abs(Input.GetAxis("Mouse Y")))
            {

                amount.x += (screenPoint.x - screenPos.x);
                //left moving
                if (!setDir)
                {
                    if (Input.GetAxis("Mouse X") < 0)
                    {
                        Debug.Log("left");
                        amount.w = 0;
                        setDir = true;
                    }
                    //right moving
                    else if (Input.GetAxis("Mouse X") > 0)
                    {
                        Debug.Log("right");
                        amount.w = 1;
                        setDir = true;
                    }
                }

                //up down translation
            }else if (Mathf.Abs(Input.GetAxis("Mouse X")) < Mathf.Abs(Input.GetAxis("Mouse Y")))
            {
                amount.y -= (screenPoint.y - screenPos.y);

                //down translation
                if (!setDir)
                {
                    if (Input.GetAxis("Mouse Y") < 0)
                    {
                        Debug.Log("down");
                        amount.z = 0;
                        setDir = true;
                    }
                    //up translation
                    else if (Input.GetAxis("Mouse Y") > 0)
                    {
                        Debug.Log("up");
                        amount.z = 1;
                        setDir = true;
                    }
                }

            }
            DMmat.SetVector("_MouseDirection", amount);

        }

    }

}
