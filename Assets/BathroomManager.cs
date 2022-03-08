using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomManager : MonoBehaviour
{
    public RotationController _activeRotator;
    public static BoneController _activeBone;

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
