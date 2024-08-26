using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerFaceController : FaceController
{
    public BodyPartController currentPC;
    [SerializeField]
    private BodyPartController hoveredPC;
    public Transform cube;
    public Vector3 positionCache, scaleCache;
    public float angleCache;
    public PartTransformController rotationController, scaleController;

    [SerializeField]
    private Material colliderMaterial;

    public float currentChange = 0f;

    public override void OnEnable()
	{
        base.OnEnable();
        OnHoveredNewFacePartEvent.Instance.AddListener(SetMaterialOutline);
        OnSelectedNewFacePartEvent.Instance.AddListener(SetTransformControllers);
        OnSetTransformCacheEvent.Instance.AddListener(SetTransformCache);
        OnDeselectedFacePartEvent.Instance.AddListener(RemoveMaterialOutlineFromPreviousHover);
        OnDeselectedFacePartEvent.Instance.AddListener(DisappearControllers);
        OnTranslatePartController.Instance.AddListener(SetPartPosition);
        OnRotatePartController.Instance.AddListener(SetPartRotation);
        OnScalePartController.Instance.AddListener(SetPartScale);
        OnChangePartShaderProperty.Instance.AddListener(SetPartShaderProperty);
    }

    public override void OnDisable(){
        base.OnDisable();
        OnHoveredNewFacePartEvent.Instance.RemoveListener(SetMaterialOutline);
        OnSelectedNewFacePartEvent.Instance.RemoveListener(SetTransformControllers);
        OnSetTransformCacheEvent.Instance.RemoveListener(SetTransformCache);
        OnDeselectedFacePartEvent.Instance.RemoveListener(RemoveMaterialOutlineFromPreviousHover);
        OnDeselectedFacePartEvent.Instance.RemoveListener(DisappearControllers);
        OnTranslatePartController.Instance.RemoveListener(SetPartPosition);
        OnRotatePartController.Instance.RemoveListener(SetPartRotation);
        OnScalePartController.Instance.RemoveListener(SetPartScale);
        OnChangePartShaderProperty.Instance.RemoveListener(SetPartShaderProperty);
    }

    public void SetMaterialOutline(BodyPartController hoveredPart)
    {
       RemoveMaterialOutlineFromPreviousHover();

        if(hoveredPart != null)
        {
            hoveredPart.AddHoveredMaterial(colliderMaterial);
            hoveredPC = hoveredPart;
        }
    }

    private void RemoveMaterialOutlineFromPreviousHover()
    {
        if(hoveredPC != null)
        {
            hoveredPC.ResetMaterial();
        }
    }

    private void SetTransformCache(){
        if(currentPC != null){

            if(currentPC.translatable)
                positionCache = currentPC.pd.relativeToParentPosition;
            
            if(currentPC.rotatable)
                angleCache = currentPC.pd.relativeToParentAngle;
            
            if(currentPC.scalable)
                scaleCache = currentPC.pd.relativeToParentScale;
        }
    }

    private void DisappearControllers(){
        rotationController.partInEdit = null;
        rotationController.Disappear();
        scaleController.partInEdit = null;
        scaleController.Disappear();
    }

    private void SetTransformControllers(BodyPartController selectedPC)
    {

        if(currentPC != selectedPC)
        {
            currentPC = selectedPC;
            foreach(BodyPartController bpc in partControllers)
            {
                if(bpc == selectedPC)
                {
                    eyeTarget = EyeTarget.MOUSE;
                }
            }
            //cube.position = currentPC.transform.position;
        }

        if(currentPC.rotatable){
            rotationController.partInEdit = currentPC;
            rotationController.UpdateControllerPositions();
        }else{
            rotationController.partInEdit = null;
            rotationController.Disappear();
        }
            
        if(currentPC.scalable){
            scaleController.partInEdit = currentPC;
            scaleController.UpdateControllerPositions();
        }else{
            scaleController.partInEdit = null;
            scaleController.Disappear();
        }
    }

    private void UpdateControllers()
    {

        if(currentPC.rotatable)
        {
            rotationController.UpdateControllerPositions();
        }
            
        if(currentPC.scalable)
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

    bool startedTickling = false;
    private void SetPartPosition(BodyPartController translatingPC, Vector3 pos, bool mirror)
    {
        //each part has a relative position to other objects
        if(!translatingPC.detached)
        {
            float flip = translatingPC.flippedXAxis? -1f : 1f;
            
            translatingPC.transform.position = new Vector3(pos.x, pos.y, translatingPC.transform.position.z);
            
            Vector3 absPos = new Vector3(pos.x*flip, pos.y, translatingPC.transform.position.z);
            //if(translatingPC.sj2D != null)
                //translatingPC.sj2D.connectedAnchor = Vector3.Lerp(translatingPC.sj2D.connectedAnchor, new Vector3(pos.x, pos.y, translatingPC.transform.position.z), Time.deltaTime);

            //turn on clamped position
            //translatingPC.pd.SetClampedPosition(absPos);
            
            if(translatingPC.mirroredPart != null){
                if(!translatingPC.mirroredPart.detached && mirror){
                    translatingPC.mirroredPart.UpdateAllTransformValues();
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
            UpdateControllers();
        }
        else
        {
            translatingPC.transform.position = new Vector3(pos.x, pos.y, translatingPC.transform.position.z);
        }

        if(!canRandomHeadPos)
        {
            canRandomHeadPos = true;
        }
    
        //translatingPC.UpdateAllTransformValues();
    }

    private void SetPartScale(BodyPartController scalingPC, Vector3 pos)
    {
        //pos -= currentPC.transform.position;
        //currentPC.UpdatePosition();

        Vector3 diff = scalingPC.transform.InverseTransformDirection(scalingPC.transform.position - pos)*2f;
        
        if(scalingPC.customScaleAnchor != null)
        {
            diff = scalingPC.customScaleAnchor.InverseTransformDirection(scalingPC.customScaleAnchor.position - pos)*2f;
            diff = new Vector3(diff.x/2f, diff.y/2f, diff.z);
        }
        
        diff = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), 1);
        
        scalingPC.transform.localScale = new Vector3(diff.x, diff.y, 1f);
            
        if(!currentPC.detached){
            //currentPC.pd.SetClampedScale(diff);
            UpdateControllers();
        }

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

        float angle = Mathf.Atan2(pos.y - currentPC.transform.position.y, pos.x - currentPC.transform.position.x) * Mathf.Rad2Deg;

        if(currentPC.flippedXAxis)
        {
            currentPC.transform.rotation = Quaternion.Euler(0f, 0f, angle + 180f);
            ///currentPC.pd.relativeToParentAngle = -angle + 180f;
        }
        else
        {
            currentPC.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            //currentPC.pd.relativeToParentAngle = angle;
        }
            
        //currentPC.pd.relativeToParentAngle = currentPC.pd.GetClampedAngle(angle, currentPC.flippedXAxis);
        
        if(!currentPC.detached){
            UpdateControllers();
        }
            //currentPC.UpdateAllTransformValues();

        if(currentPC.mirroredPart != null){
            if(!currentPC.mirroredPart.detached)
            {
                currentPC.mirroredPart.UpdateAllTransformValues();
            }
        }
        
    }

}
