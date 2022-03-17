using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapeTest : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer _skin;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i < 6; i++)
        {
            _skin.SetBlendShapeWeight(i, 100*Mathf.PerlinNoise(i, Time.time));
        }
    }
}
