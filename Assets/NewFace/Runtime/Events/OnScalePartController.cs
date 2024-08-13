using UnityEngine.Events;
using UnityEngine;

public class OnScalePartController : UnityEvent<PartController, Vector3>
{
    public static OnScalePartController Instance = new OnScalePartController();
}