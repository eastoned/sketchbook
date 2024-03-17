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

    public static float timeSinceLastRemark;

    void OnEnable()
	{
        OnChangedMouthScaleEvent.Instance.AddListener(MouthSpeech);
        OnSendRemarkToSpeech.Instance.AddListener(SpeakEvent);
        //OnSelectedNewFacePartEvent.Instance.AddListener(PartMention);
    }

    void OnDisable(){
        OnChangedMouthScaleEvent.Instance.RemoveListener(MouthSpeech);
        OnSendRemarkToSpeech.Instance.RemoveListener(SpeakEvent);
       // OnSelectedNewFacePartEvent.Instance.AddListener(PartMention);
    }

    private IEnumerator Start(){
        for(;;){
            timeSinceLastRemark += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator TranslatePlayerActionData(PlayerActionData pad)
    {
        
        
        if(Mathf.Abs(pad.positionChange.y) > 0.05f || Mathf.Abs(pad.positionChange.x) > 0.05f){
            yield return SpeakText("You changed my " + pad.partName + ".", 2f);
            string verticalChange = "";
            string horizontalChange = "";
            string totalChange = "";

            if(pad.partName.EndsWith("s")){
            //is plural
                totalChange += " were ";
            }else{
                totalChange += " was ";
            }
            bool bothFlagged = false;

            if(pad.positionChange.y > 0.05f){
                bothFlagged = true;
                verticalChange = "too low";
            }else if (pad.positionChange.y < -0.05f){
                bothFlagged = true;
                verticalChange = "too high";
            }
            totalChange += verticalChange;

            if(pad.positionChange.x > 0.05f){
                if(bothFlagged){
                    totalChange += " and ";
                }
                horizontalChange = "too close together";
            }else if (pad.positionChange.x < -0.05f){
                if(bothFlagged){
                    totalChange += " and ";
                }
                horizontalChange = "too far apart";
            }
            totalChange += horizontalChange;

            yield return SpeakText("You must have thought my " + pad.partName + totalChange + ".", 4f);

        }
        //
       
    }

    void PartMention(Transform part){
        if(!NuFaceManager.couldBeShared){
        if(SpeakingRoutine != null){
            //StopCoroutine(SpeakingRoutine);
        }
        string plural = "it";
        if(part.name[part.name.Length-1].Equals(char.Parse("s"))){
            plural = "them";
        }

        SpeakingRoutine = StartCoroutine(Speak("You have selected my " + part.name + ".<br>Please make " + plural + " beautiful.", 2f));
        }
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

    public void SpeakEvent(string text){
        int spaceCounter = 0;
            for(int i = 0; i < text.Length; i++){
                
                if(char.IsWhiteSpace(text[i])){
                    spaceCounter++;
                    
                }
            }
        spaceCounter += 1;
        timeSinceLastRemark = Random.Range(0f, 2f);
        SpeakText(text, spaceCounter/2f);
    }

    public Coroutine SpeakText(string text, float animLength){
        return StartCoroutine(Speak(text, animLength));
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
            
            float randomOffset = Random.Range(0, 150);
            float journey = 0;
            while(journey < value){
                journey += Time.deltaTime;
                float percent = Mathf.Clamp01(journey/value);
                float scalePercent = scaleCurve.Evaluate(percent);
                float translatePercent = translateCurve.Evaluate(percent);
                bubble.GetComponentInChildren<TextMeshProUGUI>().text = text.Substring(0, Mathf.CeilToInt(text.Length*translatePercent));
                bubble.transform.position = Vector3.Lerp(Camera.main.WorldToScreenPoint(mouthPos.position), Camera.main.WorldToScreenPoint(mouthPos.position) + new Vector3(0, randomOffset, 0), translatePercent);
                bubble.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), scalePercent);
                
                yield return null;
            }
            Destroy(bubble);
        }
    }
}
