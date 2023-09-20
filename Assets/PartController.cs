using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PartController : MonoBehaviour
{

    void OnMouseOver(){
        OnSelectedNewFacePartEvent.Instance.Invoke(transform);
    }   

}
