using UnityEngine.Events;
using UnityEngine;

public class OnSendRemarkToSpeech : UnityEvent<string>
{
    public static OnSendRemarkToSpeech Instance = new OnSendRemarkToSpeech();
}
