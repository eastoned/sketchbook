using UnityEngine.Events;
using UnityEngine;

public class OnScalePartController : UnityEvent<Vector3>
{
    public static OnScalePartController Instance = new OnScalePartController();
}