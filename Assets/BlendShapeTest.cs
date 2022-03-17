using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapeTest : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer _skin;
    private float[] _times = new float[6];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            _times[i] = Random.Range(.25f, 2f);
        }
         }
    // Update is called once per frame
    void Update()
    {
        for(int i=0; i < 6; i++)
        {
            if(_times[i] < 0)
            {
                
                _skin.SetBlendShapeWeight(i, Random.Range(0, 4) * 25);
                _times[i] = Random.Range(.25f, 2f);
            }
            _skin.SetBlendShapeWeight(i, _skin.GetBlendShapeWeight(i) + (Mathf.PerlinNoise(i, Time.time)-0.5f));
            _times[i] -= Time.deltaTime;
        }
    }
}
