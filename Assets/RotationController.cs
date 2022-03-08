using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    /*
    public Vector3 _axis;

    [SerializeField]
    private float _rotSpeed;

    [SerializeField]
    private Vector3 _mouseCache;

    private Quaternion rot, _boneRot;


    public Transform _currentJoint;

    public float amount;
    public Vector3 _mousePos;
    public Vector3 _cameraVector;
    public Vector3 crossp;
    public float dot;*/

    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;

    public bool _spinning = false;


    
    private void OnMouseDown()
    {
        //BathroomManager._activeRotator = this;
        //Debug.Log(BathroomManager._activeRotator.name);
        //_mouseCache = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        //rot = transform.localRotation;
        //_boneRot = _currentController.localRotation;
        //amount = 0;
        
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, transform.position + transform.up);
       // Gizmos.DrawLine(transform.position, _mouseCache);
        //Gizmos.DrawLine(_mouseCache, _mousePos);
    }

    private void OnMouseUp()
    {
        _spinning = false;
    }

    private void OnMouseDrag()
    {
        _spinning = true;
        mPosDelta = Input.mousePosition - mPrevPos;

        if(Vector3.Dot(transform.up, Vector3.up) >= 0)
        {
            transform.Rotate(transform.up, -Vector3.Dot(mPosDelta, Camera.main.transform.forward), Space.World);

        }
        else
        {
            transform.Rotate(transform.up, Vector3.Dot(mPosDelta, Camera.main.transform.forward), Space.World);
        }

        //transform.Rotate(Camera.main.transform.right, Vector3.Dot(mPosDelta, Camera.main.transform.up), Space.World);

        mPrevPos = Input.mousePosition;
        /*

            _cameraVector = Camera.main.transform.up;
            _mousePos = Input.mousePosition;
            _mousePos = Camera.main.ScreenToWorldPoint(new Vector3(_mousePos.x, _mousePos.y, 1));
             //_lr1.SetPosition(0, _mouseCache);
             //_lr1.SetPosition(1, _worldSpaceMouse);

            Vector3 diff = new Vector3(_mouseCache.x - transform.position.x, _mouseCache.y - transform.position.y, _mouseCache.z - transform.position.z);
            //dot = DotProd(_mouseCache - _mousePos, transform.position);
             //_lr1.SetPosition(0, transform.position);

        //_lr2.SetPosition(0, transform.position);
        //_lr2.SetPosition(1, transform.position + (transform.up * 5f));


            crossp = CrossProd(transform.up, _mouseCache - _mousePos);
            
            dot = DotProd(crossp, transform.up);
        Debug.Log(crossp);
        amount = (crossp.x * crossp.y) + crossp.z;
        //transform.localRotation = rot * Quaternion.Euler(0, -amount * _rotSpeed, 0);

        //_currentController.localRotation = _boneRot * Quaternion.Euler(-amount * _axis);
        */
    }
    
    Vector3 CrossProd(Vector3 v, Vector3 w)
    {
        float xMul = v.y * w.z - v.z * w.y;
        float yMul = v.z * w.x - v.x * w.z;
        float zMul = v.x * w.y - v.y * w.x;
        Vector3 cross = new Vector3(xMul, yMul, zMul);

        return cross;
    }

    float DotProd(Vector3 v, Vector3 w)
    {
        float val = (v.x * w.x) + (v.y * w.y) + (v.z * w.z);
        return val;
    }

}
