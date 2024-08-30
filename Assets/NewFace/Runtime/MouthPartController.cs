using System.Collections;
using UnityEngine;

public class MouthPartController : BodyPartController
{
    public GameObject speechBubble;
    public Transform canvas;

    public int currentSpeak;

    public float upperLip, lowerLip, mouthRadius;

    private IEnumerator speakingRoutine;

    public bool canSpeak = true;

    public static float timeSinceLastRemark;

    void OnEnable()
	{
        OnChangedMouthScaleEvent.Instance.AddListener(MouthSpeech);
    }

    void OnDisable()
    {
        OnChangedMouthScaleEvent.Instance.RemoveListener(MouthSpeech);
    }

    void UpdateCanSpeak(float value)
    {
        Debug.Log("updating speaking status");
        canSpeak = value > 0.05f;
    }

    public IEnumerator TranslatePlayerActionData(PlayerActionData pad)
    {
        yield return null;
        if(pad.actionType == CharacterActionData.ActionType.BREAKCHANGE){
            //yield return SpeakText("You broke my " + pad.partName + ".", 2f);
        }
        else if(Mathf.Abs(pad.positionChange.y) > 0.05f || Mathf.Abs(pad.positionChange.x) > 0.05f)
        {
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
            //yield return SpeakText("You changed my " + pad.partName + ".", Random.Range(1.5f, 2.5f));
            //yield return SpeakText("You must have thought my " + pad.partName + totalChange + ".", totalChange.Length/8f);
        }

    }

    void PartMention(Transform part)
    {
        if(!NuFaceManager.canShareFeedback){
        //if(SpeakingRoutine != null){
            //StopCoroutine(SpeakingRoutine);
        //}
        string plural = "it";
        if(part.name[part.name.Length-1].Equals(char.Parse("s"))){
            plural = "them";
        }

        //SpeakingRoutine = StartCoroutine(Speak("You have selected my " + part.name + ".<br>Please make " + plural + " beautiful.", 2f));
        }
    }

    void MouthSpeech(float value){
        
        //if(SpeakingRoutine != null){
            //StopCoroutine(SpeakingRoutine);
        //}

        if(value > 0){
           // SpeakingRoutine = StartCoroutine(Speak("I can be quieter if you want.", 8f));
        }else{
           // SpeakingRoutine = StartCoroutine(Speak("Do you want me to speak up?", 6f));
        }
        
    }
    
    [ContextMenu("Test")]
    public void Test()
    {
        StartCoroutine(SpeakRoutine());
    }

    public IEnumerator SpeakRoutine()
    {
        Speak("Hello! My name is Easton and I'm so happy to be here!");
        yield return null;
    }

    public void Speak(string text)
    {
        if(canSpeak)
        {
            GameObject bubble = Instantiate(speechBubble, transform.position, Quaternion.identity, canvas);
            AnimateTMPElement textAnimator = bubble.GetComponent<AnimateTMPElement>();
            textAnimator.sj2D.connectedBody = rb2D;
            textAnimator.InitializeBubble(text);
        }
    }
}
