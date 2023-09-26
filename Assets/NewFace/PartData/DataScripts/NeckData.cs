using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeckData", menuName = "ScriptableObjects/Neck", order = 4)]
public class NeckData : PartData{

    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.GetAbsoluteScale().x;
    }

}