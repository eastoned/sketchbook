using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LimbScore : MonoBehaviour
{
    [SerializeField]
    private TwoBoneIKConstraint _constraint;
        
    [SerializeField]
    private Likable _score;

    [SerializeField]
    public float scorepos;
    private void Update()
    {

        scorepos = _score._likeScore/5;

        _constraint.weight = scorepos;

       
    }

}
