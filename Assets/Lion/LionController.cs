using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionController : MonoBehaviour
{

    public Animator Tail,head;
    public float age;
    public AnimationCurve ageToResponsiveness, ageToSpeed, ageToRespond;

    private void OnEnable(){
        SnapEvent.Instance.AddListener(Wave);
    }
    private void OnDisable(){
        SnapEvent.Instance.RemoveListener(Wave);
    }

    private IEnumerator Start(){
        for(;;){
            age = Time.time;
            //Tail.SetTrigger("Wave");
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
            
            yield return new WaitForSeconds(Random.Range(1f, 10f));
            head.SetBool("Up", false);
        }
    }

    private void Wave(){
        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine(){
        yield return new WaitForSeconds(ageToResponsiveness.Evaluate(age));
        Tail.speed = ageToSpeed.Evaluate(age);
        Tail.SetTrigger("Wave");
        yield return new WaitForSeconds(ageToResponsiveness.Evaluate(age));
        if(head){
            if(Random.Range(0f, 1f) < ageToRespond.Evaluate(age)){
                head.speed = ageToSpeed.Evaluate(age);
                head.SetBool("Up", true);
            }
        }
    }
}
