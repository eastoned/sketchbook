using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMesh : MonoBehaviour
{

    public Vector3[] newVerts;
    public Vector2[] newUV;

    public int[] newTris;

    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.Clear();

        int x = 0;

        for (int i = -1; i < 2; i++){
            for (int j = -1; j < 2; j++){
                newVerts[x] = new Vector3(i, j, 0);
                x++;
            }
        }

        mesh.vertices = newVerts;
        mesh.uv = newUV;
        mesh.triangles = newTris;
        mesh.name = "Custom";
    }

    // Update is called once per frame
    void Update()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        mesh.vertices = newVerts;
        mesh.uv = newUV;
        mesh.triangles = newTris;
        //Mesh mesh = GetComponent<MeshFilter>().mesh;
    }
}
