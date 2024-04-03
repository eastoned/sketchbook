using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private void RemoveMaterialOutlineFromPreviousHover()
    {
        if(currentTransform != null)
        {
            currentTransform.GetComponent<Renderer>().sharedMaterials = new Material[1]{currentTransform.GetComponent<Renderer>().sharedMaterials[0]};
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
    private void SetPartPosition(PartController translatingPC, Vector3 pos)
    {
        //each part has a relative position to other objects
        
        if(!translatingPC.detached)
        {
            float flip = translatingPC.flippedXAxis? -1f : 1f;
            translatingPC.transform.position = new Vector3(pos.x, pos.y, translatingPC.pd.absoluteWorldPositionZ);

            Vector3 absPos = new Vector3(pos.x*flip, pos.y, translatingPC.pd.absoluteWorldPositionZ);

            translatingPC.pd.SetClampedPosition(absPos);
            
            if(translatingPC.mirroredPart != null){
                if(!translatingPC.mirroredPart.detached)
                    translatingPC.mirroredPart.UpdateAllTransformValues();
            }
            
            if(translatingPC.pd.IsPositionOutsideMaximum(absPos))
            {
                translatingPC.ShakePiece(absPos.magnitude*10f, 0.25f);

                if(!startedTickling)
                {
                    OnTickleEvent.Instance.Invoke();
                    startedTickling = true;
                }
                
                if(absPos.magnitude > 1.2f)
                {
                    Debug.Log("Reached limit so break");
                    UpdatePartAttachmentStatus(translatingPC, true);
                }
                    
            }
        }else{
            
            translatingPC.transform.position = new Vector3(pos.x, pos.y, translatingPC.pd.absoluteWorldPositionZ);
            
        }

        translatingPC.UpdateAllTransformValues();
    }

    private void SetPartScale(Vector3 pos){
        pos -= transform.position;

        if(!currentPC.detached){
            Vector3 diff = currentPC.transform.InverseTransformDirection(currentPC.transform.position - pos)*2f;
            diff = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), 1);

            currentPC.pd.SetClampedScale(diff);

            currentPC.UpdateAllTransformValues();
            
            if(currentPC.mirroredPart != null){
                if(!currentPC.mirroredPart.detached)
                    currentPC.mirroredPart.UpdateAllTransformValues();
            }
        }else{
            Vector3 diff = currentPC.transform.InverseTransformDirection(currentPC.transform.position - pos)*2f;
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

}
