using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscleDriver : MonoBehaviour
{
    private Animator _anim;
    private HumanPoseHandler _poseHandler;
    private HumanPose _humanPose;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        Vector3 current_position = transform.position;
        _poseHandler = new HumanPoseHandler(_anim.avatar, transform);
        _humanPose = new HumanPose();
        //_humanPose.bodyPosition = current_position;
        _poseHandler.GetHumanPose(ref _humanPose);
        for (int i =0; i < _humanPose.muscles.Length; i++)
        {
            //_humanPose.muscles[i] = Random.Range(-1f, 1f);
        }
        
        _poseHandler.SetHumanPose(ref _humanPose);
    }

    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < _humanPose.muscles.Length; i++)
        {
            _humanPose.muscles[i] = (Mathf.PerlinNoise(Time.time, i)*2) - 1;
        }

        _poseHandler.SetHumanPose(ref _humanPose);
    }
}
