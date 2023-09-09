using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UpdateLikes : MonoBehaviour
{
    [SerializeField]
    public CinemachineTargetGroup targetGroup;
    
    void Start(){
        targetGroup = GetComponent<CinemachineTargetGroup>();
        foreach(CinemachineTargetGroup.Target t in targetGroup.m_Targets){
            Debug.Log(t.target.name);
        }
    
    }

    public void LikeChange(Likable liked, float change){

        foreach(CinemachineTargetGroup.Target t in targetGroup.m_Targets){
            //targetGroup.FindMember
        }

        int index = targetGroup.FindMember(liked.transform);
        Debug.Log(index);
        targetGroup.m_Targets[index].weight = change;
    }
    
    public void AddLikable(Transform t){
        targetGroup.AddMember(t, 1f, 1f);
    }
}
