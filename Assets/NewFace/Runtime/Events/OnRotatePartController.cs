using UnityEngine.Events;
using UnityEngine;

public class OnRotatePartController : UnityEvent<Vector3>
{
    public static OnRotatePartController Instance = new OnRotatePartController();
}