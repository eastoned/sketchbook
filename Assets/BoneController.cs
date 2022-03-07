using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BoneController : MonoBehaviour
{

    [SerializeField]
    private Transform _leftIcon, _rightIcon;

    [SerializeField]
    private Transform _updownIcon, _rightleftIcon;

    public Vector3 _iconOffset, _offset;

    public Vector3 _axis;

    private void OnMouseEnter()
    {
        //four different buttons
        /*_leftIcon.position = transform.position + _iconOffset;
        _iconOffset.z *= -1;
        _rightIcon.position = transform.position + _iconOffset;

        _leftIcon.eulerAngles = new Vector3(0, 90, 0);
        _rightIcon.eulerAngles = new Vector3(0, 90, 0);

        GameObject _tempTransform = new GameObject();
        _tempTransform.transform.position = (_leftIcon.position + _rightIcon.position) / 2;
        _leftIcon.SetParent(_tempTransform.transform);
        _rightIcon.SetParent(_tempTransform.transform);
        _tempTransform.transform.eulerAngles = new Vector3(-transform.eulerAngles.z, 0, 0);
        _leftIcon.parent = null;
        _rightIcon.parent = null;
        Destroy(_tempTransform);*/

        //two buttons with a scrubbing effect
        _updownIcon.position = transform.position + _offset;
        _rightleftIcon.position = transform.position + _offset;
        _updownIcon.localEulerAngles = transform.eulerAngles + new Vector3(0, 0,90);
        _rightleftIcon.localEulerAngles = transform.eulerAngles;
    }

    private void OnMouseOver()
    {
        //Debug.Log("mouse over joint");


        
        /*
        if (Input.GetMouseButtonDown(0))
        {
            transform.localEulerAngles += 15*_axis;
        }
        if (Input.GetMouseButtonDown(1))
        {
            transform.localEulerAngles -= 15 * _axis;
        }
        */

    }


}
