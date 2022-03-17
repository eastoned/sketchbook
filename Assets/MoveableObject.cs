using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{

    public bool _softerNudge = false;
    public bool _activeMover =false;

    [SerializeField]
    private ObjectMover _mover;

    private void OnMouseDown()
    {
        if (!_softerNudge) {
            _mover._activeMover = this;
            gameObject.layer = 10;
            //transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4));
           // RaycastHit _hit;
           // Debug.Log(Vector3.Normalize(Camera.main.transform.position - transform.position));
            //Debug.DrawRay(Camera.main.transform.position, Vector3.Normalize(transform.position - Camera.main.transform.position) * 10f, Color.yellow);
            /*if (Physics.Raycast(transform.position, Vector3.Normalize(transform.position - Camera.main.transform.position), out _hit, 10f))
            {

                Debug.DrawRay(transform.position, Vector3.Normalize(transform.position - Camera.main.transform.position) * _hit.distance, Color.yellow);
                transform.position = _hit.point;
            }*/
        
        } else {
            transform.position += Time.deltaTime * 10f * Camera.main.transform.right * Input.GetAxis("Mouse X");
            transform.position += Time.deltaTime * 10f * Camera.main.transform.up * Input.GetAxis("Mouse Y");//;Time.deltaTime *10f* new Vector3(0, Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X")); 
        }
        
        //
    }
    private void OnMouseUp()
    {
        _mover._activeMover = null;
        gameObject.layer = 0;
    }
}
