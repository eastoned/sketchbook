using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectByDistance : MonoBehaviour
{

    public GameObject currentPeel;

    public GameObject[] peelObjects;

    void Start()
    {
        int amount = GameObject.FindGameObjectsWithTag("Peelable").Length;
        Debug.Log(amount);

        peelObjects = GameObject.FindGameObjectsWithTag("Peelable");

        for (int i = 0; i < peelObjects.Length; i++)
        {
           
                peelObjects[i].GetComponent<PeelPull>().enabled = false;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            Debug.Log(worldPos);
            float smallestDistance = 100;

            for(int i = 0; i < peelObjects.Length; i++)
            {
                if(Vector3.Distance(worldPos, peelObjects[i].transform.position) < smallestDistance)
                {
                    currentPeel = peelObjects[i];
                    smallestDistance = Vector3.Distance(worldPos, peelObjects[i].transform.position);
                }
            }

            for (int i = 0; i < peelObjects.Length; i++)
            {
                if(currentPeel != peelObjects[i])
                {
                    peelObjects[i].GetComponent<PeelPull>().enabled = false;
                }
                else
                {
                    peelObjects[i].GetComponent<PeelPull>().enabled = true;
                }
            }

            Debug.Log(currentPeel.name);
        }

        if (Input.GetMouseButtonUp(0))
        {
            for (int i = 0; i < peelObjects.Length; i++)
            {

                peelObjects[i].GetComponent<PeelPull>().enabled = false;

            }
        }
    }
}
