using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseFollow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime * 4f);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        transform.localEulerAngles = new Vector3(0, 0, -5*transform.position.x);

        GetComponent<SpriteRenderer>().sortingOrder = -(int)transform.position.y;
    }
}
