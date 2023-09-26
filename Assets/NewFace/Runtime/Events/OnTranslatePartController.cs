using UnityEngine.Events;
using UnityEngine;

public class OnTranslatePartController : UnityEvent<Vector3>
{
    public static OnTranslatePartController Instance = new OnTranslatePartController();
}
