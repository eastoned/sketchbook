using UnityEngine.Events;
using UnityEngine;

public class OnTranslatePartController : UnityEvent<PartController, Vector3>
{
    public static OnTranslatePartController Instance = new OnTranslatePartController();
}
