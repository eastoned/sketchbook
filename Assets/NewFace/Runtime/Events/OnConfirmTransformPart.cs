using UnityEngine.Events;
using UnityEngine;
public class OnConfirmTransformPart : UnityEvent<PlayerActionData>
{
    public static OnConfirmTransformPart Instance = new OnConfirmTransformPart();
}

public class OnBeginTransformPart : UnityEvent
{
    public static OnBeginTransformPart Instance = new OnBeginTransformPart();
}