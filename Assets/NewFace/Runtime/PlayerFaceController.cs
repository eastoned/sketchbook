using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerFaceController : FaceController
{
    public PartController activePC;
    public PartController hoveredPC;

    public BodyPartController activeBPC;
    public Transform cube;
    public Vector3 positionCache, scaleCache;
    public float angleCache;
    public PartTransformController rotationController, scaleController;

    public float currentChange = 0f;

    public override void OnEnable()
	{
        base.OnEnable();
        OnHoveredNewFacePartEvent.Instance.AddListener(SetDashedOutline);
        OnSelectedNewFacePartEvent.Instance.AddListener(SetSolidOutline);
        OnCurrentJointRepair.Instance.AddListener(SetTransformControllers);
        OnTranslatePartController.Instance.AddListener(SetPartPosition);
        OnRotatePartController.Instance.AddListener(SetPartRotation);
        OnScalePartController.Instance.AddListener(SetPartScale);
        OnChangePartShaderProperty.Instance.AddListener(SetPartShaderProperty);
        OnCurrentJointBreak.Instance.AddListener(DisappearControllers);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        OnHoveredNewFacePartEvent.Instance.RemoveListener(SetDashedOutline);
        OnSelectedNewFacePartEvent.Instance.RemoveListener(SetSolidOutline);
        OnCurrentJointRepair.Instance.RemoveListener(SetTransformControllers);
        OnTranslatePartController.Instance.RemoveListener(SetPartPosition);
        OnRotatePartController.Instance.RemoveListener(SetPartRotation);
        OnScalePartController.Instance.RemoveListener(SetPartScale);
        OnChangePartShaderProperty.Instance.RemoveListener(SetPartShaderProperty);
        OnCurrentJointBreak.Instance.RemoveListener(DisappearControllers);
    }

    public void SetDashedOutline(PartController hoveredPart)
    {
        if(hoveredPC != null && hoveredPC != activePC)
            hoveredPC.UpdatePartOutline(PartController.MousedState.NONE);
        
        hoveredPC = hoveredPart;

        if(hoveredPC != activePC)
            hoveredPC.UpdatePartOutline(PartController.MousedState.HOVERED);

    }

    private void SetSolidOutline(PartController selectedPC)
    {
        if(activePC != null)
            activePC.UpdatePartOutline(PartController.MousedState.NONE);

        activePC = selectedPC;
        activePC.UpdatePartOutline(PartController.MousedState.SELECTED);
            //cube.position = currentPC.transform.position;
        
        BodyPartController selectedBPC = selectedPC.GetComponent<BodyPartController>();
        
        SetTransformControllers(selectedBPC);
    }

    private void SetTransformControllers(BodyPartController selectedBPC)
    {

        if(selectedBPC == null || selectedBPC.detached)
        {
            rotationController.partInEdit = null;
            rotationController.Disappear();
            scaleController.partInEdit = null;
            scaleController.Disappear();
            return;
        }

        if(activeBPC != selectedBPC)
        {
            if(selectedBPC.rotatable)
            {
                rotationController.partInEdit = selectedBPC;
                rotationController.UpdateControllerPositions();
            }else{
                rotationController.partInEdit = null;
                rotationController.Disappear();
            }
                
            if(selectedBPC.scalable)
            {
                scaleController.partInEdit = selectedBPC;
                scaleController.UpdateControllerPositions();
            }else{
                scaleController.partInEdit = null;
                scaleController.Disappear();
            }
        
            activeBPC = selectedBPC;
        }
    }

    private void DisappearControllers()
    {
        rotationController.partInEdit = null;
        rotationController.Disappear();
        scaleController.partInEdit = null;
        scaleController.Disappear();
    }

    private void UpdateControllers()
    {
        if(activeBPC.rotatable)
        {
            rotationController.UpdateControllerPositions();
        }
            
        if(activeBPC.scalable)
        {
            scaleController.UpdateControllerPositions();
        }
    }

    private void UpdatePartAttachmentStatus(BodyPartController pc, bool status){
        pc.UpdateAttachmentStatus(status);
        //Instantiate(blood, pc.transform.position, Quaternion.identity);
    }

    private void SetPartShaderProperty(BodyPartController editingPC, string shaderPropertyName, float shaderValue)
    {
        Debug.Log("updating shader on: " + editingPC.name);
        if(!canRandomShaders)
        {
            canRandomShaders =true;
        }
        if(editingPC != null)
        {
            editingPC.UpdateSingleShaderFloat(shaderPropertyName, shaderValue);
            editingPC.UpdateRenderPropBlock();
        }
    }

    private void SetPartPosition(BodyPartController translatingBPC, Vector3 pos, bool mirror)
    {
        //each part has a relative position to other objects
        if(!translatingBPC.detached)
        {
            float flip = translatingBPC.flippedXAxis? -1f : 1f;
            
            translatingBPC.transform.position = new Vector3(pos.x, pos.y, translatingBPC.transform.position.z);
            
            Vector3 absPos = new Vector3(pos.x*flip, pos.y, translatingBPC.transform.position.z);
            //if(translatingPC.sj2D != null)
                //translatingPC.sj2D.connectedAnchor = Vector3.Lerp(translatingPC.sj2D.connectedAnchor, new Vector3(pos.x, pos.y, translatingPC.transform.position.z), Time.deltaTime);

            //turn on clamped position
            //translatingPC.pd.SetClampedPosition(absPos);
            
            if(translatingBPC.mirroredPart != null){
                if(!translatingBPC.mirroredPart.detached && mirror){
                    translatingBPC.mirroredPart.UpdateAllTransformValues();
                }
            }

            /*
            if(translatingPC.pd.IsPositionOutsideMaximum(absPos))
            {
                translatingPC.ShakePiece(absPos.magnitude*10f, 0.25f);

                if(!startedTickling)
                {
                    OnTickleEvent.Instance.Invoke();
                    startedTickling = true;
                }
                if(absPos.magnitude > translatingPC.pd.breakAmount)
                {
                    Debug.Log("Reached limit so break");
                    UpdatePartAttachmentStatus(translatingPC, true);
                }
                    
            }*/
        }
        else
        {
            translatingBPC.transform.position = new Vector3(pos.x, pos.y, translatingBPC.transform.position.z);
        }

        if(!canRandomHeadPos)
        {
            canRandomHeadPos = true;
        }
    
        //translatingPC.UpdateAllTransformValues();
    }

    private void SetPartScale(BodyPartController scalingBPC, Vector3 pos)
    {
        //pos -= currentPC.transform.position;
        //currentPC.UpdatePosition();

        Vector3 diff = scalingBPC.transform.InverseTransformDirection(scalingBPC.transform.position - pos)*2f;
        
        if(scalingBPC.customScaleAnchor != null)
        {
            diff = scalingBPC.customScaleAnchor.InverseTransformDirection(scalingBPC.customScaleAnchor.position - pos)*2f;
            diff = new Vector3(diff.x/2f, diff.y/2f, diff.z);
        }
        
        diff = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), 1);
        
        scalingBPC.transform.localScale = new Vector3(diff.x, diff.y, 1f);

        if(!canRandomHeadScale)
        {
            canRandomHeadScale = true;
        }

        //currentPC.UpdateScale();
        //currentPC.UpdateDependencies();
    }

    private void SetPartRotation(Vector3 pos)
    {
        //pos -= transform.localPosition;

        float angle = Mathf.Atan2(pos.y - activeBPC.transform.position.y, pos.x - activeBPC.transform.position.x) * Mathf.Rad2Deg;

        if(activeBPC.flippedXAxis)
        {
            activeBPC.transform.rotation = Quaternion.Euler(0f, 0f, angle + 180f);
            ///currentPC.pd.relativeToParentAngle = -angle + 180f;
        }
        else
        {
            activeBPC.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            //currentPC.pd.relativeToParentAngle = angle;
        }
            
        //currentPC.pd.relativeToParentAngle = currentPC.pd.GetClampedAngle(angle, currentPC.flippedXAxis);
            //currentPC.UpdateAllTransformValues();

        if(activeBPC.mirroredPart != null){
            if(!activeBPC.mirroredPart.detached)
            {
                activeBPC.mirroredPart.UpdateAllTransformValues();
            }
        }
        
    }

}
