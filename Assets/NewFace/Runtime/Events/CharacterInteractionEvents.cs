using UnityEngine;
using UnityEngine.Events;

public class OnCharacterCollisionEvent : UnityEvent<BodyPartController, BodyPartController>
{
    public static OnCharacterCollisionEvent Instance = new OnCharacterCollisionEvent();
}