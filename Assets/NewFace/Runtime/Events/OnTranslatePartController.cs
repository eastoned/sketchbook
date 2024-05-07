using UnityEngine.Events;
using UnityEngine;

public class OnTranslatePartController : UnityEvent<PartController, Vector3, bool>
{
    public static OnTranslatePartController Instance = new OnTranslatePartController();
}
