using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscleDriver : MonoBehaviour
{
    private Animator _anim;
    private HumanPoseHandler _poseHandler;
    private HumanPose _humanPose;
    int _musIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        //Vector3 current_position = transform.position;
        _poseHandler = new HumanPoseHandler(_anim.avatar, transform);
        _humanPose = new HumanPose();
        
        //_humanPose.bodyPosition = current_position;
        _poseHandler.GetHumanPose(ref _humanPose);
        Debug.Log(_humanPose.muscles.Length);
        for (int i =0; i < _humanPose.muscles.Length; i++)
        {
            //_humanPose.muscles[i] = Random.Range(-1f, 1f);
        }
        
        _poseHandler.SetHumanPose(ref _humanPose);
    }

    // Update is called once per frame
    void Update()
    {

        

        if (Input.GetMouseButtonDown(0))
        {
           // _musIndex = Random.Range(0, _humanPose.muscles.Length);
        }

        if (Input.GetMouseButton(0))
        {
            Debug.Log(((Input.mousePosition.x/Screen.width)*2)-1);
            float _musStrength = ((Input.mousePosition.x / Screen.width) * 2) - 1;
           // _humanPose.muscles[_musIndex] = _musStrength;
        }

        for (int i = 0; i < _humanPose.muscles.Length; i++)
        {
            _humanPose.muscles[i] = _humanPose.muscles[i];
        }
        _humanPose.bodyPosition = _anim.bodyPosition;
        _humanPose.bodyRotation = _anim.bodyRotation;
        _poseHandler.SetHumanPose(ref _humanPose);
    }
}
