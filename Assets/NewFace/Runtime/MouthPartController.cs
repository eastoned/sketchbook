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

    public float textSpeed = 1f;

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

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        Test();
    }

    public IEnumerator TranslatePlayerActionData(PlayerActionData pad)
    {
        yield return null;
        if(pad.actionType == CharacterActionData.ActionType.BREAKCHANGE)
        {
            //yield return SpeakText("You broke my " + pad.partName + ".", 2f);
        }
        else if(Mathf.Abs(pad.positionChange.y) > 0.05f || Mathf.Abs(pad.positionChange.x) > 0.05f)
        {
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
        if(!NuFaceManager.canShareFeedback)
        {
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
        if(Random.Range(0f, 1f) < 0.5f)
        {
            Speak("I'm so happy!");
        }else
        {
           Speak("Hello!!"); 
        }
    }

    public void Speak(string text)
    {
        if(canSpeak)
        {
            GameObject bubble = Instantiate(speechBubble, transform.position, Quaternion.identity, canvas);
            AnimateTMPElement textAnimator = bubble.GetComponent<AnimateTMPElement>();
            textAnimator.sj2D.connectedBody = rb2D;
            textAnimator.sj2D.connectedAnchor += new Vector2(Random.Range(-1f, 1f) ,0);
            textAnimator.InitializeBubble(text, textSpeed);
            StartCoroutine(AnimateMouthToFollowText(text, textSpeed));
        }
    }

    private IEnumerator AnimateMouthToFollowText(string text, float speed)
    {
        float mouthRadius = GetSingleShaderFloat("_MouthRadius");
        float mouthOpen = GetSingleShaderFloat("_MouthOpen");
        float teethTop = GetSingleShaderFloat("_TeethTop");
        float teethBottom = GetSingleShaderFloat("_TeethBottom");
        float tongueRadius = GetSingleShaderFloat("_TongueRadius");
        float tongueScale = GetSingleShaderFloat("_TongueScale");
        float tongueHeight = GetSingleShaderFloat("_TongueHeight");

        float count = 0f;
        while (count < text.Length)
        {
            UpdateSingleShaderFloatUnsafe("_MouthRadius", Mathf.PerlinNoise(Time.time, count + mouthRadius));
            UpdateSingleShaderFloatUnsafe("_MouthOpen", Mathf.PerlinNoise(Time.time * 2f, count + mouthOpen));
            UpdateSingleShaderFloatUnsafe("_TeethTop", Mathf.PerlinNoise(Time.time, count + teethTop));
            UpdateSingleShaderFloatUnsafe("_TeethBottom", Mathf.PerlinNoise(Time.time, count + teethBottom));
            UpdateSingleShaderFloatUnsafe("_TongueRadius", Mathf.PerlinNoise(Time.time, count + tongueRadius));
            UpdateSingleShaderFloatUnsafe("_TongueScale", Mathf.PerlinNoise(Time.time, count + tongueScale));
            UpdateSingleShaderFloatUnsafe("_TongueHeight", Mathf.PerlinNoise(Time.time, count + tongueHeight));
            UpdateRenderPropBlock();

            count += Time.deltaTime * speed;
            yield return new WaitForSeconds(0.01f);
        }

        UpdateSingleShaderFloatUnsafe("_MouthRadius", mouthRadius);
        UpdateSingleShaderFloatUnsafe("_MouthOpen", mouthOpen);
        UpdateSingleShaderFloatUnsafe("_TeethTop", teethTop);
        UpdateSingleShaderFloatUnsafe("_TeethBottom", teethBottom);
        UpdateSingleShaderFloatUnsafe("_TongueRadius", tongueRadius);
        UpdateSingleShaderFloatUnsafe("_TongueScale", tongueScale);
        UpdateSingleShaderFloatUnsafe("_TongueHeight", tongueHeight);
        UpdateRenderPropBlock();

        yield return null;
    }
}
