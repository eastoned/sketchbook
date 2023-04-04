using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnvironment : MonoBehaviour
{
    void Update(){
        Ray ray = new Ray(transform.position, Vector3.up);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 0.5f)){
            transform.position += hit.normal * 0.1f;
        }

        ray = new Ray(transform.position, -Vector3.up);
        if(Physics.Raycast(ray, out hit, 0.5f)){
            transform.position += hit.normal * 0.1f;
        }

        ray = new Ray(transform.position, Vector3.right);
        if(Physics.Raycast(ray, out hit, 0.5f)){
            transform.position += hit.normal * 0.1f;
        }

        ray = new Ray(transform.position, -Vector3.right);
        if(Physics.Raycast(ray, out hit, 0.5f)){
           transform.position += hit.normal * 0.1f;
        }

        ray = new Ray(transform.position, Vector3.forward);
        if(Physics.Raycast(ray, out hit, 0.5f)){
            transform.position += hit.normal * 0.1f;
        }

        ray = new Ray(transform.position, -Vector3.forward);
        if(Physics.Raycast(ray, out hit, 0.5f)){
            transform.position += hit.normal * 0.1f;
        }
    }

    // Update is called once per frame
    void OnDrawGizmos()
    {
        
        Gizmos.DrawRay(new Ray(transform.position, Vector3.up));
        Gizmos.DrawRay(new Ray(transform.position, -Vector3.up));

        Gizmos.DrawRay(new Ray(transform.position, Vector3.right));
        Gizmos.DrawRay(new Ray(transform.position, -Vector3.right));

        Gizmos.DrawRay(new Ray(transform.position, Vector3.forward));
        Gizmos.DrawRay(new Ray(transform.position, -Vector3.forward));    
    }
}
