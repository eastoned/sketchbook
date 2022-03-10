using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BoneController : MonoBehaviour
{

    private RotationController _xAxisControl, _yAxisControl;

    //[SerializeField]
    //private Transform _updownIcon, _rightleftIcon, _fwdBackIcon;

    [SerializeField]
    private GameObject _axis;

    //[SerializeField]
    //private RotationController _xAxis;

    public Vector3 _offset;

    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;

    public bool X, Y, Z = false;

    private float strength;
    private Vector3 dir;

    public Collider _col;

    private Transform _ogParent;

    private void Start()
    {
        strength = Random.Range(1f, 5f);
        dir = Random.onUnitSphere;
        _ogParent = transform.parent;
        _col = GetComponent<Collider>();
       // _xAxis = _rightleftIcon.GetComponent<RotationController>();

        //GameObject Y = Instantiate(_axis, transform.position, transform.rotation);
        //GameObject X = Instantiate(_axis, transform.position, transform.rotation * Quaternion.Euler(90, 0, 0));
        //GameObject Z = Instantiate(_axis, transform.position, transform.rotation * Quaternion.Euler(90, 90, 0));

        //Y.transform.SetParent(transform.parent);
        //X.transform.SetParent(Y.transform);
        //Z.transform.SetParent(X.transform);
        //transform.SetParent(Z.transform);
    }
    /*
    private void OnMouseEnter()
    {
        if (!_xAxis._spinning)
        {
            BathroomManager.ChangeCurrentBone(this);
           // _rightleftIcon.position = transform.position;// + _offset;
            //_updownIcon.position = transform.position;
            //_fwdBackIcon.position = transform.position;
            //_updownIcon.localEulerAngles = transform.eulerAngles + new Vector3(0, 0, 90);
            //_fwdBackIcon.localEulerAngles = transform.eulerAngles + new Vector3(90, 0, 90);
           // _rightleftIcon.localEulerAngles = transform.eulerAngles;

            //_rightleftIcon.SetParent(transform.parent);
            //_fwdBackIcon.SetParent(_rightleftIcon);
            //_updownIcon.SetParent(_fwdBackIcon);
           // transform.SetParent(_rightleftIcon);
        }

        //_xAxisControl._currentController = transform;
        //_xAxisControl._axis = _axis1;
        //_yAxisControl._currentController = transform;
        //_yAxisControl._axis = _axis2;
    }

    private void OnMouseOver()
    {
        if (!_xAxis._spinning)
        {
            BathroomManager.ChangeCurrentBone(this);
            //_rightleftIcon.position = transform.position;// + _offset;
            //_updownIcon.position = transform.position;
            //_fwdBackIcon.position = transform.position;
            //_updownIcon.localEulerAngles = transform.eulerAngles + new Vector3(0, 0, 90);
            //_fwdBackIcon.localEulerAngles = transform.eulerAngles + new Vector3(90, 0, 90);
            //_rightleftIcon.localEulerAngles = transform.eulerAngles;

            //_rightleftIcon.SetParent(transform.parent);
            //_fwdBackIcon.SetParent(_rightleftIcon);
            //_updownIcon.SetParent(_fwdBackIcon);
            //transform.SetParent(_rightleftIcon);

            
        }
    }

    private void OnMouseExit()
    {
        // transform.parent.SetParent(null);
        //transform.SetParent(_ogParent);
        ///BathroomManager._activeBone = null;
        //Debug.Log(BathroomManager._activeBone.name);
        
    }
    */

    private void OnMouseDrag()
    {
        mPosDelta = Input.mousePosition - mPrevPos;

        
        /*
        if (Vector3.Dot(transform.up, Vector3.up) >= 0)
        {
            transform.Rotate(transform.up, -Vector3.Dot(mPosDelta, Camera.main.transform.forward), Space.World);

        }
        else
        {
            transform.Rotate(transform.up, Vector3.Dot(mPosDelta, Camera.main.transform.forward), Space.World);
        }*/

        transform.Rotate(Camera.main.transform.up, Vector3.Dot(mPosDelta, Camera.main.transform.forward), Space.World);

        transform.Rotate(Camera.main.transform.right, Vector3.Dot(mPosDelta, Camera.main.transform.up), Space.World);

        mPrevPos = Input.mousePosition;
    }

    private void Update()
    {
        transform.Rotate(Time.deltaTime * (strength * dir));
    }
    public void ResetParent()
    {
        //transform.SetParent(_ogParent);
    }

}
