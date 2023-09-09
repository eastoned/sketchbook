using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class Likable : MonoBehaviour
{

    public UpdateLikes updater;

    [SerializeField]
    public float _likeScore;

    [SerializeField]
    private float _likeAmount;

    public NavMeshAgent nma;


    public bool liked = false;
    public bool destSet = false;
    float time;

    private void Update()
    {
        

        if(_likeScore > 0)
        {
            _likeScore -= Time.deltaTime;
            _likeScore = Mathf.Clamp(_likeScore, 0, 10);
            updater.LikeChange(this, _likeScore);
        }

        if(!destSet){
            nma.SetDestination(new Vector3(Random.Range(-24f, 24f), 0, Random.Range(-24f, 24f)));
            destSet = true;
        }

        if(ReachedDestinationOrGaveUp(nma)){
            destSet = false;
        }

        //transform.Translate(transform.forward * Time.deltaTime * 3f, Space.Self);
        //time += Time.deltaTime;
        //transform.Rotate(Mathf.PerlinNoise(time * 0.001f, .5f) - 0.5f, Mathf.PerlinNoise(.25f, time* 0.001f) - 0.5f, Mathf.PerlinNoise(.1f, time* 0.001f) - 0.5f);
        
    }

    public bool ReachedDestinationOrGaveUp(NavMeshAgent navMeshAgent) {
        if (!navMeshAgent.pathPending) {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f) {
                    return true;
                    } } }
                    return false; 
        }

    private void OnMouseOver()
    {

        if(!liked){
            AddToBase();
            return;
        }

        IncreaseWeight();
        
    }

    private void IncreaseWeight(){
        _likeScore += _likeAmount;
        updater.LikeChange(this, _likeScore);
    }

    private void AddToBase(){
        updater.AddLikable(this.transform);
        liked = true;
    }

}
