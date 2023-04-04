using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateSprites : MonoBehaviour
{
    public GameObject spawnThing;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 400; i++){
            GameObject whisp = Instantiate(spawnThing, new Vector3(Random.Range(-16.5f, 16.5f), Random.Range(-30, 30), 0), Quaternion.identity);
            whisp.GetComponent<SpriteRenderer>().sortingOrder = -(int)whisp.transform.position.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
