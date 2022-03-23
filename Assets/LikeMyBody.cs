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

    [SerializeField]
    private Pose _pose;

    [SerializeField]
    private Transform[] _pieces = new Transform[6];

    public bool _posed = false;
    

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

        if (_posed)
        {


            _pieces[0].localPosition = Vector3.Lerp(_pieces[0].localPosition, _pose._lookAtPos, Time.deltaTime);

            _pieces[1].localPosition = Vector3.Lerp(_pieces[1].localPosition, _pose._leftHandPos, Time.deltaTime);
            _pieces[1].localRotation = Quaternion.Slerp(_pieces[1].localRotation, _pose._leftHandRot, Time.deltaTime);

            _pieces[2].localPosition = Vector3.Lerp(_pieces[2].localPosition, _pose._rightHandPos, Time.deltaTime);
            _pieces[2].localRotation = Quaternion.Slerp(_pieces[2].localRotation, _pose._rightHandRot, Time.deltaTime);

            _pieces[3].localPosition = Vector3.Lerp(_pieces[3].localPosition, _pose._leftFootPos, Time.deltaTime);
            _pieces[3].localRotation = Quaternion.Slerp(_pieces[3].localRotation, _pose._leftFootRot, Time.deltaTime);

            _pieces[4].localPosition = Vector3.Lerp(_pieces[4].localPosition, _pose._rightFootPos, Time.deltaTime);
            _pieces[4].localRotation = Quaternion.Slerp(_pieces[4].localRotation, _pose._rightFootRot, Time.deltaTime);

            _pieces[5].localPosition = Vector3.Lerp(_pieces[5].localPosition, _pose._hipsPos, Time.deltaTime);
            _pieces[5].localRotation = Quaternion.Slerp(_pieces[5].localRotation, _pose._hipsRot, Time.deltaTime);
        }
        
    }



    private void OnDrawGizmos()
    {
        
        Gizmos.DrawWireSphere(avg, .5f);
    }
}
