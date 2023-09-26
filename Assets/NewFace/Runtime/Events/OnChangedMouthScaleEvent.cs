using UnityEngine;
using UnityEngine.Events;

public class OnChangedMouthScaleEvent : UnityEvent<float>
{
    public static OnChangedMouthScaleEvent Instance = new OnChangedMouthScaleEvent();
}
