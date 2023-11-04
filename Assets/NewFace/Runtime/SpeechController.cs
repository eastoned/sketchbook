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

    public PartController mouth;


    public int currentSpeak;
    public string[] sppeech;

    public float upperLip, lowerLip, mouthRadius;

    private Coroutine SpeakingRoutine;

    void OnEnable()
	{
        OnChangedMouthScaleEvent.Instance.AddListener(MouthSpeech);
        OnSelectedNewFacePartEvent.Instance.AddListener(PartMention);
    }

    void OnDisable(){
        OnChangedMouthScaleEvent.Instance.RemoveListener(MouthSpeech);
        OnSelectedNewFacePartEvent.Instance.AddListener(PartMention);
    }

    void PartMention(Transform part){
        if(SpeakingRoutine != null){
            //StopCoroutine(SpeakingRoutine);
        }
        string plural = "it";
        if(part.name[part.name.Length-1].Equals(char.Parse("s"))){
            plural = "them";
        }

        SpeakingRoutine = StartCoroutine(Speak("You have selected my " + part.name + ".<br>Please make " + plural + " beautiful.", 2f));
    }

    void MouthSpeech(float value){
        
        if(SpeakingRoutine != null){
            //StopCoroutine(SpeakingRoutine);
        }

        if(value > 0){
            SpeakingRoutine = StartCoroutine(Speak("I can be quieter if you want.", 8f));
        }else{
            SpeakingRoutine = StartCoroutine(Speak("Do you want me to speak up?", 6f));
        }
        
    }

    public void SpeakText(string text, float animLength){
        SpeakingRoutine = StartCoroutine(Speak(text, animLength));
    }

    IEnumerator Speak(string text, float value){
        mouthRadius = mouth.pd.shadePropertyDict["_MouthRadius"].propertyValue;
        
        if(mouthRadius>0.05f){
            GameObject bubble = Instantiate(speechBubble, Camera.main.WorldToScreenPoint(mouthPos.position), Quaternion.identity, canvas);
            bubble.transform.localScale = Vector3.zero;
            
            int spaceCounter = 0;
            for(int i = 0; i < text.Length; i++){
                
                if(char.IsWhiteSpace(text[i])){
                    spaceCounter++;
                    
                }
            }
            spaceCounter *= 2;
            spaceCounter += 1;
            
            upperLip = mouth.pd.shadePropertyDict["_MouthLipTop"].propertyValue;
            lowerLip = mouth.pd.shadePropertyDict["_MouthLipBottom"].propertyValue;
            
            float randomOffset = Random.Range(-150, 150);
            float journey = 0;
            while(journey < value){
                journey = journey + Time.deltaTime;

                    
                
                
                    float percent = Mathf.Clamp01(journey/value);
                    float scalePercent = scaleCurve.Evaluate(percent);
                    float translatePercent = translateCurve.Evaluate(percent);
                    bubble.GetComponentInChildren<TextMeshProUGUI>().text = text.Substring(0, Mathf.CeilToInt(text.Length*translatePercent));
                    bubble.transform.position = Vector3.Lerp(Camera.main.WorldToScreenPoint(mouthPos.position), Camera.main.WorldToScreenPoint(mouthPos.position) + new Vector3(50f, randomOffset, 0), translatePercent);
                    bubble.transform.localScale = Vector3.Lerp(new Vector3(0f, 1f, 1f), new Vector3(1f, 1f, 1f), scalePercent);
                
                // mouth.pd.shadePropertyDict["_MouthLipTop"].SetValue(upperLip + (.5f * Mathf.Sin(percent * Mathf.PI * 5f)));
                // mouth.pd.shadePropertyDict["_MouthLipTop"].SetValue(lowerLip + (.5f * Mathf.Sin(percent * Mathf.PI * 5f)));
                    mouth.UpdateSingleShaderValue("_MouthLipTop", upperLip + (.25f * Mathf.Sin(percent * Mathf.PI * spaceCounter*2)));
                    mouth.UpdateSingleShaderValue("_MouthLipBottom", lowerLip + (.25f * Mathf.Sin(percent * Mathf.PI * spaceCounter*2)));
                    mouth.UpdateAllShadersValue();
                
                yield return null;
            }
            mouth.UpdateSingleShaderValue("_MouthLipTop", upperLip);
            mouth.UpdateSingleShaderValue("_MouthLipBottom", lowerLip);
            mouth.UpdateAllShadersValue();
            Destroy(bubble);
        }
    }
}
