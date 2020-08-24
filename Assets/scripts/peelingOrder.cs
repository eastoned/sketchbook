using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class peelingOrder : MonoBehaviour
{

    public PeelPull[] order;
    //private float[] transparencyVal;
    public static int currentPeel;
    public static int orderLength;

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        orderLength = order.Length;

        //transparencyVal = new float[orderLength];
        for (int i = 0; i < order.Length; i++)
        {
            //transparencyVal[i] = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < order.Length; i++)
        {
         
            if (i == currentPeel)
            {
                order[i].canInput = true;
               // transparencyVal[i] = 1f;
            }else
            {
                order[i].canInput = false;
                //transparencyVal[i] = 0.1f;
            }

                //order[i].gameObject.GetComponent<Renderer>().materials[1].SetFloat("_Transparency", Mathf.Lerp(order[i].gameObject.GetComponent<Renderer>().materials[1].GetFloat("_Transparency"), transparencyVal[i], Time.deltaTime));
                //order[i].gameObject.GetComponent<Renderer>().materials[0].SetFloat("_Transparency", Mathf.Lerp(order[i].gameObject.GetComponent<Renderer>().materials[0].GetFloat("_Transparency"), transparencyVal[i], Time.deltaTime));

            }



    }
}
