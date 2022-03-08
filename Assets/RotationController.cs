using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{

    public Vector3 _axis;

    [SerializeField]
    private float _rotSpeed;

    [SerializeField]
    private Vector3 _mouseCache;

    private Quaternion rot, _boneRot;

    public LineRenderer _lr1, _lr2;

    public Transform _currentController;

    public float amount;


    private void OnMouseDown()
    {
        _mouseCache = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        rot = transform.localRotation;
        _boneRot = _currentController.localRotation;
        amount = 0;
    }



    private void OnMouseDrag()
    {
        Vector3 _mousePos = Input.mousePosition;

        Vector3 _worldSpaceMouse = Camera.main.ScreenToWorldPoint(new Vector3(_mousePos.x, _mousePos.y, 1));
        _lr1.SetPosition(0, _mouseCache);
        _lr1.SetPosition(1, _worldSpaceMouse);

        _lr2.SetPosition(0, transform.position);
        _lr2.SetPosition(1, transform.position + (transform.up * 5f));


        Debug.Log(CrossProd(transform.up, _mouseCache - _worldSpaceMouse));
        //Debug.Log(_mousePos - _mouseCache);
        //float amp = Mathf.Sqrt(Mathf.Pow(diff.x, 2) + Mathf.Pow(diff.y, 2));
        //Debug.Log(amp);
        amount = CrossProd(transform.up, _mouseCache - _worldSpaceMouse).x * _rotSpeed;

        //transform.localEulerAngles += _axis * amount;
        //transform.Rotate(_axis, amount);
        transform.localRotation = rot * Quaternion.Euler(0, -amount, 0);

        _currentController.localRotation = _boneRot * Quaternion.Euler(-amount * _axis);
        //transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, amount, transform.localEulerAngles.z);
        //transform.Rotate(_axis, amount);
        //rotation axis based on mouse drag input

    }

    Vector3 CrossProd(Vector3 v, Vector3 w)
    {
        float xMul = v.y * w.z - v.z * w.y;
        float yMul = v.z * w.x - v.x * w.z;
        float zMul = v.x * w.y - v.y * w.x;
        Vector3 cross = new Vector3(xMul, yMul, zMul);

        return cross;
    }

}
