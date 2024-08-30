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

public class OnBreakPart : UnityEvent<PlayerActionData>
{
    public static OnBreakPart Instance = new OnBreakPart();
}

public class OnCurrentJointBreak : UnityEvent
{
    public static OnCurrentJointBreak Instance = new OnCurrentJointBreak();
}

public class OnCurrentJointRepair : UnityEvent<BodyPartController>
{
    public static OnCurrentJointRepair Instance = new OnCurrentJointRepair();
}

public class OnTickleEvent : UnityEvent
{
    public static OnTickleEvent Instance = new OnTickleEvent();
}