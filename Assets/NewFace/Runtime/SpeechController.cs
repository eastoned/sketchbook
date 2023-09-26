using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechController : MonoBehaviour
{

    public GameObject speechBubble;
    public Transform canvas;
    public Transform mouthPos;

    public AnimationCurve scaleCurve, translateCurve;

    void OnEnable()
	{
        OnChangedMouthScaleEvent.Instance.AddListener(MouthSpeech);
        
    }

    void OnDisable(){
        OnChangedMouthScaleEvent.Instance.RemoveListener(MouthSpeech);
    }

    void MouthSpeech(float value){
        
        if(value > 0){
            StartCoroutine(Speak("I can be quieter if you want.", 8f));
        }else{
            StartCoroutine(Speak("Do you want me to speak up?", 6f));
        }

        
    }

    IEnumerator Speak(string text, float value){
        GameObject bubble = Instantiate(speechBubble, Camera.main.WorldToScreenPoint(mouthPos.position), Quaternion.identity, canvas);
        bubble.transform.localScale = Vector3.zero;
        bubble.GetComponentInChildren<TextMeshProUGUI>().text = text;
        float journey = 0;
        while(journey < value){
            journey = journey + Time.deltaTime;
            
            float percent = Mathf.Clamp01(journey/value);
            float scalePercent = scaleCurve.Evaluate(percent);
            float translatePercent = translateCurve.Evaluate(percent);
            bubble.transform.position = Vector3.Lerp(Camera.main.WorldToScreenPoint(mouthPos.position), Camera.main.WorldToScreenPoint(mouthPos.position) + new Vector3(0, 100f, 0), translatePercent);
            bubble.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, scalePercent);
            yield return null;
        }
        Destroy(bubble);
    }
}
