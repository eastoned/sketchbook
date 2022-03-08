using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BoneController : MonoBehaviour
{

    private RotationController _xAxisControl, _yAxisControl;

    [SerializeField]
    private Transform _updownIcon, _rightleftIcon;

    [SerializeField]
    private RotationController _xAxis;

    public Vector3 _offset;

    public Vector3 _axis1, _axis2;

    public Collider _col;

    private Transform _ogParent;

    private void Start()
    {
        _ogParent = transform.parent;
        _col = GetComponent<Collider>();
        _xAxis = _rightleftIcon.GetComponent<RotationController>();
    }

    private void OnMouseEnter()
    {
        if (!_xAxis._spinning)
        {
            BathroomManager.ChangeCurrentBone(this);
            //two buttons with a scrubbing effect
            //_updownIcon.position = transform.position + _offset;
            _rightleftIcon.position = transform.position;// + _offset;
            //_updownIcon.localEulerAngles = transform.eulerAngles + new Vector3(0, 0, 90);
            _rightleftIcon.localEulerAngles = transform.eulerAngles;

            _rightleftIcon.SetParent(transform.parent);
            transform.SetParent(_rightleftIcon);

            
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
            _rightleftIcon.position = transform.position;// + _offset;
            //_updownIcon.localEulerAngles = transform.eulerAngles + new Vector3(0, 0, 90);
            _rightleftIcon.localEulerAngles = transform.eulerAngles;

            _rightleftIcon.SetParent(transform.parent);
            transform.SetParent(_rightleftIcon);

            
        }
    }

    private void OnMouseExit()
    {
        // transform.parent.SetParent(null);
        //transform.SetParent(_ogParent);
        ///BathroomManager._activeBone = null;
        //Debug.Log(BathroomManager._activeBone.name);
        
    }

    public void ResetParent()
    {
        transform.SetParent(_ogParent);
    }

}
