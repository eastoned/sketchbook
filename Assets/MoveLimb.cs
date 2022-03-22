using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLimb : MonoBehaviour
{

    public bool _isMoving = false;
    public float _initialDepth;

    public bool _isCam = false;

    private void OnMouseDown()
    {
        _isMoving = !_isMoving;
        _initialDepth = transform.position.z - Camera.main.transform.position.z;
    }

    private void Update()
    {
        if (_isMoving)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _initialDepth));
            if (!_isCam)
            {
                transform.position = point;
                //transform.LookAt(point - Camera.main.transform.position);
                
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, point, Time.deltaTime);
            }
            

            
        }
    }

    
}
