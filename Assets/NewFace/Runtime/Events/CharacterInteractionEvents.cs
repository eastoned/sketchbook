using UnityEngine;
using UnityEngine.Events;

public class OnCharacterCollisionEvent : UnityEvent
{
    public static OnCharacterCollisionEvent Instance = new OnCharacterCollisionEvent();
}