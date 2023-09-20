using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditMesh : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter mf;

    public Vector3 testValue;

    public List<HeadController> controls = new List<HeadController>();


    private void Start(){
        mf = GetComponent<MeshFilter>();
        mesh = new Mesh{
            name = "Edited Mesh",
            vertices = mf.sharedMesh.vertices,
            triangles = mf.sharedMesh.triangles,
            uv = mf.sharedMesh.uv
        };
        Debug.Log(mesh.isReadable);
        mf.mesh = mesh;
    }

    void OnMouseOver(){
        for(int i = 0; i < controls.Count; i ++){
            controls[i].transform.position = transform.localToWorldMatrix.MultiplyPoint(mesh.vertices[i]);
            //controls[i].
        }
    }

    void Update(){
        //UpdateMeshPositions(1, testValue);
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        for(int i = 0; i < mesh.vertexCount; i++){
             vertices[i] = transform.worldToLocalMatrix.MultiplyPoint(controls[i].transform.localPosition);
        }

       mesh.vertices = vertices;
    }

    void UpdateMeshPositions(int vertex, Vector3 pos){
        mesh.vertices[vertex] = pos;
        mf.mesh = mesh;
    }

    private void OnDrawGizmos(){
        for(int i = 0; i < mesh.vertexCount; i++){
            
            Gizmos.DrawWireSphere(transform.localToWorldMatrix.MultiplyPoint(mesh.vertices[i]), 0.05f);
        }
    }
}
