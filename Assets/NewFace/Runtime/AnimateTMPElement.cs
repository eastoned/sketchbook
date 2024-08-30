using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class AnimateTMPElement : MonoBehaviour
{

    [Range(0f,1f)]
    [SerializeField]
    private float textVisible;

    public float textFactor, revealFactor = 20f;
    public TextMeshProUGUI textMesh;
    public AnimationCurve positionCurve, textRevealCurve;
    public Rigidbody2D rb2D;
    public SpringJoint2D sj2D;

    public void InitializeBubble(string text, float speed)
    {
        StartCoroutine(AnimateTextBubble(text, speed));
    }

    private IEnumerator AnimateTextBubble(string text, float speed)
    {
        UpdateTextVisibility(0f);
        textMesh.text = text;
        float journey = 0;

        while(journey < text.Length)
        {
            journey += Time.deltaTime * speed;
            float percent = Mathf.Clamp01(journey/text.Length);
            //float positionPercent = positionCurve.Evaluate(percent);
            float textRevealPercent = textRevealCurve.Evaluate(percent);
            UpdateTextVisibility(textRevealPercent);
                /*
                if(speakTime * amountofwords > 2f)
                {
                    if(Random.Range(0f, 1f) < .5f)
                    {
                        OnTriggerAudioOneShot.Instance.Invoke("Beep");
                    }else{
                        OnTriggerAudioOneShot.Instance.Invoke("Beep2");
                    }
                    
                    speakTime = 0f;
                }
                */
            //speakTime += Time.deltaTime;    
            //transform.position = origin + new Vector3(0, positionPercent * 50f, 0);
            yield return new WaitForSeconds(.01f);
        }

        float journey2 = 0;
        while(journey2 < 2f)
        {
            journey2 += Time.deltaTime * speed;
            float percent = Mathf.Clamp01(journey2/2f);
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, percent);
            yield return new WaitForSeconds(.01f);
        }
            
        Destroy(this.gameObject);
        yield return null;
    }

    private void UpdateTextVisibility(float textToShow)
    {
        if(textMesh != null)
        {
            textMesh.ForceMeshUpdate();  
            TMP_TextInfo textInfo = textMesh.textInfo;

            if(textInfo != null)
            {
                if(textInfo.meshInfo != null)
                {
                    Color32[][] originalColors = new Color32[textInfo.meshInfo.Length][];

                    for (int i = 0; i < originalColors.Length; i++)
                    {
                        Color32[] theColors = textInfo.meshInfo[i].colors32;
                        originalColors[i] = new Color32[theColors.Length];
                        Array.Copy(theColors, originalColors[i], theColors.Length);
                    }

                    for(int j = 0; j < textInfo.characterCount; j++)
                    {
                        TMP_CharacterInfo charInfo = textInfo.characterInfo[j];
                        if(charInfo.isVisible)
                        {
                            Color32[] destColors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;

                            Color32 theColor = Color32.Lerp(new Color32(0,0,0,255), new Color32(255,255,255,255), textToShow*1.5f + ((float)-j)/textInfo.characterCount + .5f);
                            destColors[charInfo.vertexIndex + 0] = theColor;
                            destColors[charInfo.vertexIndex + 1] = theColor;
                            destColors[charInfo.vertexIndex + 2] = theColor;
                            destColors[charInfo.vertexIndex + 3] = theColor;
                        }
                    }
                }
                textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            }
        }

    }
}
