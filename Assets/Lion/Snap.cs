using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;


public class Snap : MonoBehaviour
{
    
    public float timeHeld;
    public Vector2 distanceTraveled;
    public float totalDistance;
    public Vector2 deltaDistance;
    public AudioSource snap;

    public float[] pastDistanceDeltaMag;

    public int timeSamples;
    public float velocityThreshold;
    public int counter;
    private RectTransform rTransform;
    void Start(){
        rTransform = GetComponent<RectTransform>();
        pastDistanceDeltaMag = new float[timeSamples];
    }
    void Update(){
        transform.position = Input.mousePosition;
        if(Input.GetMouseButtonDown(0)){
            timeHeld = 0;
            counter = 0;
            distanceTraveled = Vector2.zero;
        }
        if(Input.GetMouseButton(0)){
            timeHeld += Time.deltaTime;
            deltaDistance = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            pastDistanceDeltaMag[counter] = Mathf.Abs(deltaDistance.magnitude);
            if(counter < timeSamples-1){
                counter += 1;
            }else{
                counter = 0;
            }
            distanceTraveled += deltaDistance;
        }
        if(Input.GetMouseButtonUp(0)){
            if(VerifyThreshold()){
                if(!snap.isPlaying){
                    snap.Play();
                }
                SnapEvent.Instance.Invoke();
            }
            rTransform.sizeDelta = Vector2.zero;
        }
        
        totalDistance = distanceTraveled.magnitude;
        
        rTransform.sizeDelta = new Vector2(GetAverageSize()*50, GetAverageSize()*50);
                
    }

    private bool VerifyThreshold(){
        for(int i = 0; i < timeSamples; i++){
            if(pastDistanceDeltaMag[i] < velocityThreshold){
                return false;
            }
        }
        return true;
    }
    private float GetAverageSize(){
        float total = 0;
        for(int i = 0; i < timeSamples; i++){
            total += pastDistanceDeltaMag[i];
        }
        total/=timeSamples;
        return total;
    }
}
