using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooController : MonoBehaviour
{

    private Transform bT;
    // Start is called before the first frame update
    void Start()
    {
        bT = transform;
    }

    // Update is called once per frame
    void Update()
    {
        bT.Translate(Vector3.forward * Time.deltaTime * 3f * Input.GetAxis("Vertical"), Space.Self);
        bT.Translate(Vector3.right * Time.deltaTime * 3f * Input.GetAxis("Horizontal"), Space.Self);
        
    }
}
