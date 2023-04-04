using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Likable : MonoBehaviour
{
    [SerializeField]
    public float _likeScore;

    [SerializeField]
    private float _likeAmount = 5f;

    [SerializeField]
    public Transform _limbGoal;

    private void Update()
    {
        _likeScore = Mathf.Clamp(_likeScore, 0, 10);

        if(_likeScore > 0)
        {
            _likeScore -= Time.deltaTime;
        }
    }

    private void OnMouseDown()
    {
        _likeScore += _likeAmount;
        
    }

}
