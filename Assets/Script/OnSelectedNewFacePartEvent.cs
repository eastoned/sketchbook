using UnityEngine;
using UnityEngine.Events;

public class OnSelectedNewPartEvent : UnityEvent<PartController>
{
    public static OnSelectedNewPartEvent Instance = new OnSelectedNewPartEvent();
}

public class OnHoveredNewPartEvent : UnityEvent<PartController>
{
    public static OnHoveredNewPartEvent Instance = new OnHoveredNewPartEvent();
}

public class OnSetTransformCacheEvent : UnityEvent
{
    public static OnSetTransformCacheEvent Instance = new OnSetTransformCacheEvent();
}