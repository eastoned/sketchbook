using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerFaceController : FaceController
{
    public PartController currentPC;
    private Transform currentTransform;
    public Transform cube;
    public Vector3 positionCache, scaleCache;
    public float angleCache;
    public PartTransformController rotationController, scaleController;

    [SerializeField] private Material colliderMaterial;

    public float currentChange = 0f;
    public SpeechController sc;
    private MaterialPropertyBlock block;
    public Renderer tear1, tear2;

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
        //OnConfirmTransformPart.Instance.AddListener(Comment);
        //OnSelectedNewFacePartEvent.Instance.AddListener(PopInScale);
        block = new MaterialPropertyBlock();
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
        //OnConfirmTransformPart.Instance.RemoveListener(Comment);
        //OnSelectedNewFacePartEvent.Instance.RemoveListener(PopInScale);
    }

    public void SetMaterialOutline(Transform hoveredTransform)
    {
       RemoveMaterialOutlineFromPreviousHover();

        if(hoveredTransform != null)
        {
            hoveredTransform.GetComponent<Renderer>().sharedMaterials = new Material[2]{hoveredTransform.GetComponent<Renderer>().sharedMaterials[0], colliderMaterial};
            currentTransform = hoveredTransform;
        }
    }

    private void PopOutScale(){
        //Debug.Log("pop");
        //currentPC.ScalePieces(1f, .2f, scalePopCurve);
    }

    private void PopInScale(Transform ignore){
        //Debug.Log("pop");
        //ignore.GetComponent<PartController>().ScalePieces(-1f, .2f, scalePopCurve);
    }

    private void RemoveMaterialOutlineFromPreviousHover()
    {
        if(currentTransform != null)
        {
            currentTransform.GetComponent<Renderer>().sharedMaterials = new Material[1]{currentTransform.GetComponent<Renderer>().sharedMaterials[0]};
        }
    }

    private void SetTransformCache(){
        if(currentPC != null){
            if(currentPC.translatable){
                positionCache = currentPC.pd.relativeToParentPosition;
            }
            if(currentPC.rotatable){
                angleCache = currentPC.pd.relativeToParentAngle;
            }
            if(currentPC.scalable){
                scaleCache = currentPC.pd.relativeToParentScale;
            }
        }
    }

    private void DisappearControllers(){
        rotationController.partInEdit = null;
        rotationController.Disappear();
        scaleController.partInEdit = null;
        scaleController.Disappear();
    }

    private void SetTransformControllers(PartController selectedPC){

        if(currentPC != selectedPC){
            currentPC = selectedPC;
            cube.position = currentPC.transform.position;
        }

        if(currentPC.rotatable){
            rotationController.partInEdit = currentPC;
        }else{
            rotationController.partInEdit = null;
            rotationController.Disappear();
        }
            
        if(currentPC.scalable){
            scaleController.partInEdit = currentPC;
        }else{
            scaleController.partInEdit = null;
            scaleController.Disappear();
        }
    }

    private void UpdatePartAttachmentStatus(PartController pc, bool status){
        pc.UpdateAttachmentStatus(status);
        //Instantiate(blood, pc.transform.position, Quaternion.identity);
    }

    bool startedTickling = false;
    private void SetPartPosition(Vector3 pos)
    {
        //each part has a relative position to other objects
        
        if(!currentPC.detached)
        {
            float flip = currentPC.flippedXAxis? -1f : 1f;
            currentPC.transform.localPosition = new Vector3(pos.x, pos.y, currentPC.pd.absoluteWorldPositionZ);

            Vector3 absPos = new Vector3(pos.x*flip, pos.y, currentPC.pd.absoluteWorldPositionZ);

            //Debug.Log(currentPC.pd.PositionOutsideMaximum(absPos));
            currentPC.pd.SetClampedPosition(absPos);

            currentChange = Vector2.Distance(currentPC.pd.relativeToParentPosition, positionCache);
            //Debug.Log("Position change: " + currentPC.pd.relativePosition + " is the clamped pos : " + positionCache + "is the abs position: " +  currentChange);
            
            if(currentPC.mirroredPart != null && currentPC.canUpdateMirror){
                if(!currentPC.mirroredPart.detached)
                    currentPC.mirroredPart.UpdateAllTransformValues();
            }
            
            if(currentPC.pd.IsPositionOutsideMaximum(absPos))
            {
                currentPC.ShakePiece(absPos.magnitude*10f, 0.25f);

                if(!startedTickling)
                {
                    OnTickleEvent.Instance.Invoke();
                    startedTickling = true;
                }
                
                if(absPos.magnitude > 1.2f)
                {
                    UpdatePartAttachmentStatus(currentPC, true);
                    //currentPC.pd.SetWorldPositionBounds();
                    //currentPC.canUpdateMirror = false;
                    //remove this part from any parent if the magnitude is too high
                    for(int i = 0; i < bodyParts.Length; i++){
                        if(bodyParts[i].GetComponent<PartController>().childControllers.Contains(currentPC)){
                            bodyParts[i].GetComponent<PartController>().childControllers.Remove(currentPC);
                        }
                    }
                }
                    
            }
        }else{
            
            currentPC.transform.localPosition = new Vector3(pos.x, pos.y, currentPC.pd.absoluteWorldPositionZ);
            currentPC.parent = null;

            currentPC.pd.SetClampedPosition(pos);
            for(int i = 0; i < bodyParts.Length; i++)
            {
                if(bodyParts[i].GetComponent<BoxCollider2D>().OverlapPoint(currentPC.transform.position))
                {
                    if(currentPC.transform != bodyParts[i])
                    {
                        PartController parent = bodyParts[i].GetComponent<PartController>();
                        if(currentPC.pd.absoluteWorldPositionZ < parent.pd.absoluteWorldPositionZ)
                        {
                            currentPC.parent = parent;
                        }
                    }
                }
            }


        }

        currentPC.UpdateAllTransformValues();
            //if position is on  node then we can attach to it
    }

    private void SetPartScale(Vector3 pos){
        pos -= transform.localPosition;

        if(!currentPC.detached){
            Vector3 diff = currentPC.transform.InverseTransformDirection(currentPC.transform.localPosition - pos)*2f;
            diff = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), 1);

            currentPC.pd.SetClampedScale(diff);
            currentChange = Vector3.Distance(currentPC.pd.relativeToParentScale, scaleCache);
            //Debug.Log("Scale change: " + currentChange);
            currentPC.UpdateAllTransformValues();
            
            if(currentPC.mirroredPart != null && currentPC.canUpdateMirror){
                if(!currentPC.mirroredPart.detached)
                    currentPC.mirroredPart.UpdateAllTransformValues();
            }
            //currentPC.pd.SetPositionBounds();
        }else{
            Vector3 diff = currentPC.transform.InverseTransformDirection(currentPC.transform.localPosition - pos)*2f;
            diff = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), 1);
            

            if(!currentPC.flippedXAxis){
                currentPC.transform.localScale = currentPC.pd.GetClampedScale(diff);
            }else{
                currentPC.transform.localScale = currentPC.pd.GetFlippedClampedScale(diff);
            }
        }
    }

    private void SetPartRotation(Vector3 pos){

        pos -= transform.localPosition;

        float angle = Mathf.Atan2(pos.y - currentPC.transform.localPosition.y, pos.x - currentPC.transform.localPosition.x) * Mathf.Rad2Deg;
        currentChange = Mathf.Abs(angleCache - currentPC.pd.relativeToParentAngle)/180f;
        currentPC.transform.localRotation = Quaternion.Euler(0f, 0f, currentPC.pd.GetClampedAngle(angle, currentPC.flippedXAxis));

        if(currentPC.mirroredPart != null){
            if(!currentPC.mirroredPart.detached)
                currentPC.mirroredPart.UpdateAllTransformValues();
        }
    }

    public void SetExpression(){

    }

}
