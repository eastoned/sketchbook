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

    public IEnumerator TranslatePlayerActionData(PlayerActionData pad)
    {
        if(Mathf.Abs(pad.positionChange.y) > 0.05f || Mathf.Abs(pad.positionChange.x) > 0.05f){
            Debug.Log("speak about my parts");
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
            yield return SpeakText("You changed my " + pad.partName + ".", Random.Range(1.5f, 2.5f));
            yield return SpeakText("You must have thought my " + pad.partName + totalChange + ".", totalChange.Length/8f);
        }

    }

    void PartMention(Transform part){
        if(!NuFaceManager.canShareFeedback){
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
        StartCoroutine(SpeakText(text, spaceCounter/2f));
    }

    public IEnumerator SpeakText(string text, float animLength){
        Debug.Log("returning speak ienumer");
        return Speak(text, animLength);
    }

    private IEnumerator Speak(string text, float value){
        Debug.Log("Starting the speak routine");
        mouthRadius = mouth.GetSingleShaderFloat("_MouthOpen");
        float mouthOpen = mouth.GetSingleShaderFloat("_MouthRadius"); 
        
        if(mouthRadius>0.05f && mouthOpen > 0.05f){
            Debug.Log("mouth is big enought");
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
            
            float randomOffset = Random.Range(150, 350);
            float journey = 0;
            int amountofwords = text.Length;
            AnimateTMPElement textAnimator = bubble.GetComponentInChildren<AnimateTMPElement>();
            textAnimator.SetOriginalText(text);
            float speakTime = 0;
            while(journey < value){
                journey += Time.deltaTime;
                float percent = Mathf.Clamp01(journey/value);
                float scalePercent = scaleCurve.Evaluate(percent);
                float translatePercent = translateCurve.Evaluate(percent);
                if(speakTime * amountofwords > 2f){
                    if(Random.Range(0f, 1f) < .5f){
                        OnTriggerAudioOneShot.Instance.Invoke("Beep");
                    }else{
                        OnTriggerAudioOneShot.Instance.Invoke("Beep2");
                    }
                    
                    speakTime = 0f;
                }
                speakTime += Time.deltaTime;
                //Debug.Log(text.Length*translatePercent);
                if(text[Mathf.FloorToInt((text.Length-1)*translatePercent)].Equals("e")){
                    mouth.UpdateSingleShaderFloat("_MouthOpen", 0f);
                }else{
                    mouth.UpdateSingleShaderFloat("_MouthOpen", Random.Range(0f, 1f) * mouthRadius);
                }
                mouth.UpdateRenderPropBlock();
                
                ///if(text[Mathf.CeilToInt(text.Length*translatePercent)].Equals(" ")){
                //Debug.Log(text[Mathf.CeilToInt(text.Length*translatePercent)]);
                //}
                
                textAnimator.UpdateTextVisibility(translatePercent * text.Length);
                //bubble.GetComponentInChildren<TextMeshProUGUI>().text = text;///text.Substring(0, Mathf.FloorToInt(text.Length*translatePercent));
                bubble.transform.position = new Vector3(Screen.width/2f, Screen.height * .75f, 0);//Vector3.Lerp(Camera.main.WorldToScreenPoint(mouthPos.position), Camera.main.WorldToScreenPoint(mouthPos.position) + new Vector3(0, randomOffset, 0), translatePercent);
                bubble.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), scalePercent);
                yield return null;
            }
            mouth.UpdateSingleShaderFloat("_MouthOpen", mouthRadius);
            mouth.UpdateRenderPropBlock();
            Destroy(bubble);
        }else{
            Debug.Log("mouth is too small");
        }
        
    }
}
