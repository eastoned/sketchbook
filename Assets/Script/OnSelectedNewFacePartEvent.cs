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