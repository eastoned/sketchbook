using UnityEngine.Events;
using UnityEngine;

public class OnScalePartController : UnityEvent<BodyPartController, Vector3>
{
    public static OnScalePartController Instance = new OnScalePartController();
}