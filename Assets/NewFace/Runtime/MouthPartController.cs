using System.Collections;
using System.Collections.Generic;
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

    public AnimationCurve mouthAnimation;

    public float textSpeed = 1f;

    public Viseme[] mouthVisemes = new Viseme[10];

    public VisemeValue currentViseme;

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

        //Test();
    }

    //[ContextMenu("Save Viseme")]
    public void SaveViseme()
    {
        float mouthOpen = rend.sharedMaterial.GetFloat("_MouthOpen");
        float teethTop = rend.sharedMaterial.GetFloat("_TeethTop");
        float teethBottom = rend.sharedMaterial.GetFloat("_TeethBottom");
        float tongueRadius = rend.sharedMaterial.GetFloat("_TongueRadius");
        float tongueScale = rend.sharedMaterial.GetFloat("_TongueScale");
        float tongueHeight = rend.sharedMaterial.GetFloat("_TongueHeight");

        MouthVisemeShader[] mouthShaderProperties = {
            new MouthVisemeShader("_MouthOpen", mouthOpen),
            new MouthVisemeShader("_TeethTop", teethTop),
            new MouthVisemeShader("_TeethBottom", teethBottom),
            new MouthVisemeShader("_TongueRadius", tongueRadius),
            new MouthVisemeShader("_TongueScale", tongueScale),
            new MouthVisemeShader("_TongueHeight", tongueHeight),
        };

        foreach(Viseme vis in mouthVisemes)
        {
            if(vis.currentViseme == currentViseme)
            {
                vis.mouthProperties = mouthShaderProperties;
            }
        }

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
        Speak("Hello!!");
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
        float mouthOpen = GetSingleShaderFloat("_MouthOpen");
        float teethTop = GetSingleShaderFloat("_TeethTop");
        float teethBottom = GetSingleShaderFloat("_TeethBottom");
        float tongueRadius = GetSingleShaderFloat("_TongueRadius");
        float tongueScale = GetSingleShaderFloat("_TongueScale");
        float tongueHeight = GetSingleShaderFloat("_TongueHeight");

        float mouthOpenLerp = mouthOpen; 
        float teethTopLerp = teethTop; 
        float teethBottomLerp = teethBottom; 
        float tongueRadiusLerp = tongueRadius; 
        float tongueScaleLerp = tongueScale; 
        float tongueHeightLerp = tongueHeight; 

        float count = 0f;
        while (count < text.Length)
        {
            float percent = Mathf.Clamp01(count/text.Length);
            float textRevealPercent = mouthAnimation.Evaluate(percent);

            if(text.Length * textRevealPercent < text.Length - 1)
            {
                char spokenString = text[(int)(text.Length * textRevealPercent)];
                int key = (int)Mathf.Repeat((float)spokenString, 10);
                Debug.Log(key);
                Viseme currentVisibleVisime = mouthVisemes[key];

                mouthOpenLerp = Mathf.Lerp(mouthOpenLerp, currentVisibleVisime.mouthProperties[0].shaderValue, speed * Time.deltaTime);
                teethTopLerp = Mathf.Lerp(teethTopLerp, currentVisibleVisime.mouthProperties[1].shaderValue, speed * Time.deltaTime);
                teethBottomLerp = Mathf.Lerp(teethBottomLerp, currentVisibleVisime.mouthProperties[2].shaderValue, speed * Time.deltaTime);
                tongueRadiusLerp = Mathf.Lerp(tongueRadiusLerp, currentVisibleVisime.mouthProperties[3].shaderValue, speed * Time.deltaTime);
                tongueScaleLerp = Mathf.Lerp(tongueScaleLerp, currentVisibleVisime.mouthProperties[4].shaderValue, speed * Time.deltaTime);
                tongueHeightLerp = Mathf.Lerp(tongueHeightLerp, currentVisibleVisime.mouthProperties[5].shaderValue, speed * Time.deltaTime);

                UpdateSingleShaderFloatUnsafe("_MouthOpen", mouthOpenLerp);
                UpdateSingleShaderFloatUnsafe("_TeethTop", teethTopLerp);
                UpdateSingleShaderFloatUnsafe("_TeethBottom", teethBottomLerp);
                UpdateSingleShaderFloatUnsafe("_TongueRadius", tongueRadiusLerp);
                UpdateSingleShaderFloatUnsafe("_TongueScale", tongueScaleLerp);
                UpdateSingleShaderFloatUnsafe("_TongueHeight", tongueHeightLerp);
                UpdateRenderPropBlock();
            }
            count += Time.deltaTime * speed;
            
            
            

            yield return null;
        }

        UpdateSingleShaderFloatUnsafe("_MouthOpen", mouthOpen);
        UpdateSingleShaderFloatUnsafe("_TeethTop", teethTop);
        UpdateSingleShaderFloatUnsafe("_TeethBottom", teethBottom);
        UpdateSingleShaderFloatUnsafe("_TongueRadius", tongueRadius);
        UpdateSingleShaderFloatUnsafe("_TongueScale", tongueScale);
        UpdateSingleShaderFloatUnsafe("_TongueHeight", tongueHeight);
        UpdateRenderPropBlock();

        yield return null;
    }

    [System.Serializable]
    public class Viseme
    {
        public VisemeValue currentViseme;

        public MouthVisemeShader[] mouthProperties = {
            new MouthVisemeShader("_MouthOpen", 0),
            new MouthVisemeShader("_TeethTop", 0),
            new MouthVisemeShader("_TeethBottom", 0),
            new MouthVisemeShader("_TongueRadius", 0),
            new MouthVisemeShader("_TongueScale", 0),
            new MouthVisemeShader("_TongueHeight", 0),
            };

        public Viseme(VisemeValue visVal, MouthVisemeShader[] shaderProperties)
        {
            currentViseme = visVal;
            mouthProperties = shaderProperties;
        }

    }
        [System.Serializable]
        public class MouthVisemeShader
        {
            public string shaderParam;
            public float shaderValue;
            public MouthVisemeShader(string name, float value)
            {
                shaderParam = name;
                shaderValue = value;
            }
        }

            public enum VisemeValue
        {
            Uh,
            Ah,
            Ee,
            D,
            S,
            F,
            M,
            L,
            Wo,
            Oo,
            R
        }
}
