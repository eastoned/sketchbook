using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMeshVertices : MonoBehaviour
{

    public Vector3[] vertexPositions = new Vector3[10];
    Mesh mesh;
    LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
       mesh = GetComponent<MeshFilter>().sharedMesh;
        lr = GetComponent<LineRenderer>();
       vertexPositions = new Vector3[mesh.vertexCount];
        lr.positionCount = mesh.vertexCount;
       vertexPositions = mesh.vertices;
        lr.SetPositions(vertexPositions);
    }

}
