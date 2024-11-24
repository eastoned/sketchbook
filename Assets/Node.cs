using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> inputNodes;
    public List<Node> outputNodes;

    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        if(inputNodes.Count > 0)
        {
            foreach(Node n in inputNodes)
            {
                transform.localScale += Vector3.one * n.speed * Time.deltaTime;
            }
        }

        if(outputNodes.Count > 0)
        {
            foreach(Node n in outputNodes)
            {
                transform.localScale -= Vector3.one * n.speed * Time.deltaTime;
            }
        }
    }
}
