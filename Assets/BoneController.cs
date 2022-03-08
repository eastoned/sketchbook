using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BoneController : MonoBehaviour
{

    private RotationController _xAxisControl, _yAxisControl;

    [SerializeField]
    private Transform _updownIcon, _rightleftIcon;

    public Vector3 _offset;

    public Vector3 _axis1, _axis2;

    private Transform _ogParent;

    private void Start()
    {
        _ogParent = transform.parent;
    }

    private void OnMouseEnter()
    {

        //two buttons with a scrubbing effect
        _updownIcon.position = transform.position + _offset;
        _rightleftIcon.position = transform.position + _offset;
        _updownIcon.localEulerAngles = transform.eulerAngles + new Vector3(0, 0, 90);
        _rightleftIcon.localEulerAngles = transform.eulerAngles;
        

        BathroomManager._activeBone = this;

        //_xAxisControl._currentController = transform;
        _xAxisControl._axis = _axis1;
        //_yAxisControl._currentController = transform;
        _yAxisControl._axis = _axis2;
    }

    private void OnMouseExit()
    {
        // transform.parent.SetParent(null);
        //transform.SetParent(_ogParent);
        ///BathroomManager._activeBone = null;
        //Debug.Log(BathroomManager._activeBone.name);
        
    }

}
