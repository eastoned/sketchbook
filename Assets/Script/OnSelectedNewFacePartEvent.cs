using UnityEngine;
using UnityEngine.Events;

public class OnSelectedNewFacePartEvent : UnityEvent<BodyPartController>
{
    public static OnSelectedNewFacePartEvent Instance = new OnSelectedNewFacePartEvent();
}

public class OnHoveredNewFacePartEvent : UnityEvent<BodyPartController>
{
    public static OnHoveredNewFacePartEvent Instance = new OnHoveredNewFacePartEvent();
}

public class OnSetTransformCacheEvent : UnityEvent
{
    public static OnSetTransformCacheEvent Instance = new OnSetTransformCacheEvent();
}