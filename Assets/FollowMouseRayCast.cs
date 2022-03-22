using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouseRayCast : MonoBehaviour
{
    Ray _ray, _ray1, _ray2, _ray3, _ray4;
    public float sampleDist;
    void Update()
    {
        RaycastHit _hit;

        Plane plane = new Plane(Vector3.up, 0);
        float _dist;
        

        int layerMask = 1 << 10;
        layerMask = ~layerMask;
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        _ray1 = Camera.main.ScreenPointToRay(Input.mousePosition + (sampleDist * Vector3.up));
        _ray2 = Camera.main.ScreenPointToRay(Input.mousePosition + (sampleDist * Vector3.down));
        _ray3 = Camera.main.ScreenPointToRay(Input.mousePosition + (sampleDist * Vector3.right));
        _ray4 = Camera.main.ScreenPointToRay(Input.mousePosition + (sampleDist * Vector3.left));
        //_ray = new Ray(mousePos, Camera.main.transform.forward);
        if (Physics.Raycast(_ray, out _hit, 1000, layerMask))
        {
            transform.position = _hit.point;
            RaycastHit[] samples = new RaycastHit[4];
            Vector3 norm = Vector3.zero;
            norm += _hit.normal;
            if (Physics.Raycast(_ray1, out samples[0], 1000, layerMask)){
                norm += samples[0].normal;
            }
            if (Physics.Raycast(_ray2, out samples[1], 1000, layerMask)){
                norm += samples[1].normal;
            }
            if (Physics.Raycast(_ray3, out samples[2], 1000, layerMask)){
                norm += samples[2].normal;
            }
            if (Physics.Raycast(_ray4, out samples[3], 1000, layerMask)){
                norm += samples[3].normal;
            }


            transform.up = norm/5;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(_ray);
        Gizmos.DrawRay(_ray1);
        Gizmos.DrawRay(_ray2);
        Gizmos.DrawRay(_ray3);
        Gizmos.DrawRay(_ray4);

    }
}
