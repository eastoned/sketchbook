using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{

    [SerializeField]
    public MoveableObject _activeMover;
    

    // Update is called once per frame
    void Update()
    {

        int layerMask = 1 << 10;
        layerMask = ~layerMask;

        if (_activeMover != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4));
            RaycastHit _hit;
            if (Physics.Raycast(Camera.main.transform.position, Vector3.Normalize(mousePos - Camera.main.transform.position), out _hit, 10f, layerMask))
            {

                Debug.DrawRay(Camera.main.transform.position, Vector3.Normalize(mousePos - Camera.main.transform.position) * _hit.distance, Color.yellow);
                _activeMover.transform.position = _hit.point;
            }
        }
    }
}
