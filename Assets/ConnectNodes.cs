using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectNodes : MonoBehaviour
{
    [SerializeField] private LineRenderer lr;
    private bool lineInitialized = false;
    private Node originNode;
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if(hit.transform.CompareTag("Node"))
                {
                    if(!lineInitialized)
                    {
                        //Set initial drag
                        InitializeLineRenderer(hit.transform.GetComponent<Node>());
                        return;
                    }
                    else
                    {
                        CompleteLineRenderer(hit.transform.GetComponent<Node>());
                        return;
                    }
                }  
            }
            ResetLineRenderer();
        }

        if(lineInitialized)
        {
            lr.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position);
        }
    }

    private void InitializeLineRenderer(Node n)
    {
        lr.SetPosition(0, n.transform.position);
        originNode = n;

        if(!lineInitialized)
            lineInitialized = true;
    }

    private void CompleteLineRenderer(Node n)
    {
        if(n != originNode)
        {
            lr.SetPosition(1, n.transform.position);
            n.inputNodes.Add(originNode);
            originNode.outputNodes.Add(n);
        }
        else
        {
            ResetLineRenderer();
        }
        lineInitialized = false;
    }

    private void ResetLineRenderer()
    {
        lr.SetPosition(0, Vector3.zero);
        lr.SetPosition(1, Vector3.zero);
    }
}

public class Connection
{

}
