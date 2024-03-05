using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip sound, sound2, soundDrip;

    public AudioSource aud;
    public int lastIntPlayed;

    public float numberInterval;
    void OnEnable(){
        //OnSelectedNewFacePartEvent.Instance.AddListener(PlaySound);
        //OnTranslatePartController.Instance.AddListener(UpdateSound);
        //OnDeselectedFacePartEvent.Instance.AddListener(StopSound);
        OnChangedShaderProperty.Instance.AddListener(PlaySound);
        SetCurrentShaderInterval.Instance.AddListener(SetInterval);
    }
    void OnDisable(){
        //OnSelectedNewFacePartEvent.Instance.RemoveListener(PlaySound);
        //OnTranslatePartController.Instance.RemoveListener(UpdateSound);
        //OnDeselectedFacePartEvent.Instance.RemoveListener(StopSound);
        OnChangedShaderProperty.Instance.RemoveListener(PlaySound);
        SetCurrentShaderInterval.Instance.RemoveListener(SetInterval);
    }

    void SetInterval(float interval){
        numberInterval = interval;
    }
    
    void PlaySound(float ignore){

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
