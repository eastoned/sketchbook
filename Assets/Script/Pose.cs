using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PoseScriptableObject", order = 1)]
public class Pose : ScriptableObject
{
    public string poseName;

    public Vector3 _lookAtPos;
    public Vector3 _leftHandPos, _rightHandPos;
    public Quaternion _leftHandRot, _rightHandRot;
    public Vector3 _leftFootPos, _rightFootPos;
    public Quaternion _leftFootRot, _rightFootRot;
    public Vector3 _hipsPos;
    public Quaternion _hipsRot;

}
