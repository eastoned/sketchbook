using UnityEngine.Events;
using UnityEngine;

public class OnTriggerAudioOneShot : UnityEvent<string>
{
    public static OnTriggerAudioOneShot Instance = new OnTriggerAudioOneShot();
}
