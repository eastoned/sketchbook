using UnityEngine.Events;
using UnityEngine;

public class OnTranslatePartController : UnityEvent<BodyPartController, Vector3, bool>
{
    public static OnTranslatePartController Instance = new OnTranslatePartController();
}