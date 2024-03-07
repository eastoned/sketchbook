using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip sound, sound2, soundDrip, soundSlide;

    public AudioSource aud;
    public int lastIntPlayed;

    public float numberInterval;
    void OnEnable(){
        //OnSelectedNewFacePartEvent.Instance.AddListener(PlaySound);
        //OnTranslatePartController.Instance.AddListener(UpdateSound);
        //OnDeselectedFacePartEvent.Instance.AddListener(StopSound);
        OnChangedShaderProperty.Instance.AddListener(PlaySound);
        SetCurrentShaderInterval.Instance.AddListener(SetInterval);
        OnSlideShaderProperty.Instance.AddListener(SlideSound);
    }
    void OnDisable(){
        //OnSelectedNewFacePartEvent.Instance.RemoveListener(PlaySound);
        //OnTranslatePartController.Instance.RemoveListener(UpdateSound);
        //OnDeselectedFacePartEvent.Instance.RemoveListener(StopSound);
        OnChangedShaderProperty.Instance.RemoveListener(PlaySound);
        SetCurrentShaderInterval.Instance.RemoveListener(SetInterval);
        OnSlideShaderProperty.Instance.RemoveListener(SlideSound);
    }

    void SetInterval(float interval){
        numberInterval = interval;
    }
    void SlideSound(float pitch){
        aud.clip = soundSlide;
        aud.pitch = 1 + (pitch-.5f);
        if(!aud.isPlaying){
            aud.Play();
        }
    }
    
    void PlaySound(float ignore){
        aud.clip = soundDrip;
        Debug.Log(((ignore * numberInterval) % 1) < 0.25f);
        if(((ignore * numberInterval) % 1) < 0.25f && Mathf.FloorToInt(ignore*numberInterval) != lastIntPlayed){
            aud.Stop();
            Debug.Log(Mathf.FloorToInt(ignore*numberInterval));
            lastIntPlayed = Mathf.FloorToInt(ignore*numberInterval);
            aud.pitch = 2f + (ignore - 0.5f);
            aud.PlayOneShot(soundDrip);
        }
        //if(!aud.isPlaying)
            
    }

    void PlaySound(Transform ignore){
        if(!aud.isPlaying)
            aud.PlayOneShot(sound);
    }
    
    void UpdateSound(Vector3 pos){
        //aud.pitch = pos.y;
        if(!aud.isPlaying){
            aud.loop = true;
            aud.clip = sound2;
            
            aud.Play();
        }
    }

    void StopSound(){
        aud.loop = false;
        if(aud.isPlaying)
            aud.Stop();
    }
}
