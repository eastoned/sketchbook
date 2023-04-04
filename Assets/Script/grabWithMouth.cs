using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabWithMouth : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name);
        other.transform.position = Vector3.Lerp(other.transform.position, transform.position, 5f* Time.deltaTime);
    }
}
