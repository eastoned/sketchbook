using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearerController : MonoBehaviour
{

    public bool turning;
    Transform bT;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        bT = transform;
        for(;;){
            if(Random.Range(0f, 1f) < 0.1f){
                if(!turning){
                    StartCoroutine("TurnAround");
                }
            }

            yield return new WaitForSeconds(1f);
        }
        
    }


    public IEnumerator TurnAround(){
        turning = true;
        float startRotation = bT.eulerAngles.y;
        float endRotation = startRotation + 180.0f;
        float t = 0.0f;
        while ( t  < 5f )
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / 5f) % 360.0f;
            bT.eulerAngles = new Vector3(bT.eulerAngles.x, yRotation, bT.eulerAngles.z);
            yield return null;
        }
        
        turning = false;
    }
}
