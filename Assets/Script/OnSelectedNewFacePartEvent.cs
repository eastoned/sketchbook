using UnityEngine;
using UnityEngine.Events;

public class OnSelectedNewFacePartEvent : UnityEvent<Transform>
{
    public static OnSelectedNewFacePartEvent Instance = new OnSelectedNewFacePartEvent();
}

public class OnHoveredNewFacePartEvent : UnityEvent<Transform>
{
    public static OnHoveredNewFacePartEvent Instance = new OnHoveredNewFacePartEvent();
}

public class OnSetTransformCacheEvent : UnityEvent
{
    public static OnSetTransformCacheEvent Instance = new OnSetTransformCacheEvent();
}