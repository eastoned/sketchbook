using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class houseGame : MonoBehaviour
{

    public List<GameObject> housePieces;
    public bool gamedone;
    // Start is called before the first frame update
    void Start()
    {
        gamedone = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(checkCompletion()){
            gamedone = true;
            for (int i = 0; i < housePieces.Count; i++)
            {
                housePieces[i].GetComponent<moveSprite>().Move();
            }
        }
    }

    bool checkCompletion(){
        for (int i = 0; i < housePieces.Count; i++)
        {
            if (!housePieces[i].GetComponent<moveSprite>().done)
            {
                return false;
            }
        }
        return true;
    }
}
