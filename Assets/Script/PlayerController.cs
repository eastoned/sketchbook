using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public enum PlayerState
    {
        Idle,
        Growling,
        Barking,
        Whining
    }
    public Collider2D growlDetector, barkDetector;
    private void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collider");
    }
}
