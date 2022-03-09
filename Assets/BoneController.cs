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

    public bool X, Y, Z = false;

    public Collider _col;

    private Transform _ogParent;

    private void Start()
    {
        _ogParent = transform.parent;
        _col = GetComponent<Collider>();
       // _xAxis = _rightleftIcon.GetComponent<RotationController>();

        GameObject Y = Instantiate(_axis, transform.position, transform.rotation);
        GameObject X = Instantiate(_axis, transform.position, transform.rotation * Quaternion.Euler(90, 0, 0));
        GameObject Z = Instantiate(_axis, transform.position, transform.rotation * Quaternion.Euler(90, 90, 0));

        Y.transform.SetParent(transform.parent);
        X.transform.SetParent(Y.transform);
        Z.transform.SetParent(X.transform);
        transform.SetParent(Z.transform);
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
    public void ResetParent()
    {
        //transform.SetParent(_ogParent);
    }

}
