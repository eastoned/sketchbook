using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomManager : MonoBehaviour
{
    public RotationController _activeRotator;
    public static BoneController _activeBone;
    public Transform _body;

    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _body.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            _body.transform.position = new Vector3(_body.transform.position.x, _body.transform.position.y, 0);
        }
    }

    public static void ChangeCurrentBone(BoneController _bone)
    {
        if (_activeBone != null)
        {
            _activeBone._col.enabled = true;
            _activeBone.ResetParent();
        }
        _activeBone = _bone;
        _activeBone._col.enabled = false;
        
    }

}
