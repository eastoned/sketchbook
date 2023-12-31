using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip sound, sound2;

    public AudioSource aud;
    void OnEnable(){
        OnSelectedNewFacePartEvent.Instance.AddListener(PlaySound);
        OnTranslatePartController.Instance.AddListener(UpdateSound);
        OnDeselectedFacePartEvent.Instance.AddListener(StopSound);
    }
    void OnDisable(){
        OnSelectedNewFacePartEvent.Instance.RemoveListener(PlaySound);
        OnTranslatePartController.Instance.RemoveListener(UpdateSound);
        OnDeselectedFacePartEvent.Instance.RemoveListener(StopSound);
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
