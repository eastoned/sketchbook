using UnityEngine;

[CreateAssetMenu(fileName = "NeckData", menuName = "ScriptableObjects/Neck", order = 2)]
public class NeckData : PartData{

    public override void SetScaleBounds(PartData parentData)
    {
        maxScaleX = parentData.GetAbsoluteScale().x;
    }

}