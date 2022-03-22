using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LikeMyBody : MonoBehaviour
{

    [SerializeField]
    private Likable[] _likables;

    [SerializeField]
    private Likable[] _likableRot;

    [SerializeField]
    public Vector3 avg;

    [SerializeField]
    private Transform _likedCenter;

    [SerializeField]
    private Transform _rightHand, _leftHand;
    

    // Update is called once per frame
    void Update()
    {
        avg = Vector3.zero;
        float totalLikes =0;

        for(int i=0; i < _likables.Length; i++)
        {

            totalLikes += _likables[i]._likeScore;
        }
        

        for (int i = 0; i < _likables.Length; i++)
        {
            avg += (_likables[i].transform.position * (_likables[i]._likeScore/totalLikes));
        }
        //avg /= _likables.Length;

        //if right hand is z greater its -90, else 90
        
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.DrawWireSphere(avg, .5f);
    }
}
