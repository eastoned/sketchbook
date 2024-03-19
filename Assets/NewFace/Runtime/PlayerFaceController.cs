using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerFaceController : FaceController
{

    public Transform hoveredTransform;
    
    public PartController currentPC;
    public Transform currentTransform;

    public Transform cube;
    public Vector3 positionCache, scaleCache;
    public float angleCache;
    public PartTransformController rotationController, scaleController;

    public PartController currentHovered;
    [SerializeField] private Material colliderMaterial;

    public float currentChange = 0f;
    public SpeechController sc;
    private MaterialPropertyBlock block;
    public Renderer tear1, tear2;
    public ParticleSystem blood;
    public PartController hoveredParent;
    public override void OnEnable()
	{
        base.OnEnable();
        OnHoveredNewFacePartEvent.Instance.AddListener(SetMaterialOutline);
        OnSelectedNewFacePartEvent.Instance.AddListener(SetTransformControllers);
        OnSetTransformCacheEvent.Instance.AddListener(SetTransformCache);
        OnDeselectedFacePartEvent.Instance.AddListener(RemoveMaterialOutlineFromPreviousHover);
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
        OnTranslatePartController.Instance.RemoveListener(SetPartPosition);
        OnRotatePartController.Instance.RemoveListener(SetPartRotation);
        OnScalePartController.Instance.RemoveListener(SetPartScale);
        //OnConfirmTransformPart.Instance.RemoveListener(Comment);
        //OnSelectedNewFacePartEvent.Instance.RemoveListener(PopInScale);
    }

    public void SetMaterialOutline(Transform hoveredTarget){
        RemoveMaterialOutlineFromPreviousHover();
        if(hoveredTarget != null){
            if(hoveredTarget.GetComponent<PartController>()){
                hoveredTarget.GetComponent<Renderer>().sharedMaterials = new Material[2]{hoveredTarget.GetComponent<Renderer>().sharedMaterials[0], colliderMaterial};
                hoveredTransform = hoveredTarget;
            }
        }
    }

    private void PopOutScale(){
        //Debug.Log("pop");
        currentPC.ScalePieces(1f, .2f, scalePopCurve);
    }

    private void PopInScale(Transform ignore){
        //Debug.Log("pop");
        ignore.GetComponent<PartController>().ScalePieces(-1f, .2f, scalePopCurve);
    }

    private void RemoveMaterialOutlineFromPreviousHover(){
        if(hoveredTransform != null){
            hoveredTransform.GetComponent<Renderer>().sharedMaterials = new Material[1]{hoveredTransform.GetComponent<Renderer>().sharedMaterials[0]};
        }
    }

    private void SetTransformCache(){
        if(currentPC != null){
            if(currentPC.translatable){
                positionCache = currentPC.pd.relativePosition;
            }
            if(currentPC.rotatable){
                angleCache = currentPC.pd.currentAngle;
            }
            if(currentPC.scalable){
                scaleCache = currentPC.pd.relativeScale;
            }
        }
    }

    private void SetTransformControllers(Transform selectedTarget){

        currentPC = selectedTarget.GetComponent<PartController>();
        Debug.Log("Set the transform controllers on: " + selectedTarget);
        if(currentTransform != selectedTarget){
            currentTransform = selectedTarget;
            cube.position = currentTransform.position;
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
        pc.UpdateAttachmentStatus(status, hoveredParent);
        Instantiate(blood, pc.transform.position, Quaternion.identity);
    }

    private void SetPartPosition(Vector3 pos){
        //each part has a relative position to other objects
        
        if(!currentPC.detached){
            float flip = currentPC.flippedXAxis? -1f : 1f;
            currentTransform.localPosition = new Vector3(pos.x, pos.y, currentTransform.localPosition.z);

            Vector3 absPos = new Vector3(pos.x*flip, pos.y, currentTransform.localPosition.z);

            //Debug.Log(currentPC.pd.PositionOutsideMaximum(absPos));
            currentPC.pd.ClampedPosition(absPos);

            currentChange = Vector2.Distance(currentPC.pd.relativePosition, positionCache);
            //Debug.Log("Position change: " + currentPC.pd.relativePosition + " is the clamped pos : " + positionCache + "is the abs position: " +  currentChange);

            currentPC.UpdateAllTransformValues();
            
            if(currentPC.mirroredPart != null){
                if(!currentPC.mirroredPart.detached)
                    currentPC.mirroredPart.UpdateAllTransformValues();
            }

            if(currentPC.pd.PositionOutsideMaximum(absPos)){
                currentPC.ShakePiece(absPos.magnitude*10f, 0.25f);
                
                if(absPos.magnitude > 1.2f){
                    UpdatePartAttachmentStatus(currentPC, true);
                }
                    
            }
        }else{
            currentTransform.localPosition = new Vector3(pos.x, pos.y, currentTransform.localPosition.z);
            currentPC.overAttachmentNode = false;
            currentPC.parent = null;
            for(int i = 0; i < bodyParts.Length; i++){
                if(currentTransform != bodyParts[i]){
                    if(bodyParts[i].GetComponent<BoxCollider2D>().OverlapPoint(currentTransform.position)){
                        hoveredParent = bodyParts[i].GetComponent<PartController>();
                        currentPC.overAttachmentNode = true;
                        currentPC.parent = hoveredParent;
                    }
                }
            }
            //if position is on  node then we can attach to it
        }
    
    }

    private void SetPartScale(Vector3 pos){
        pos -= transform.localPosition;

        if(!currentPC.detached){
            Vector3 diff = currentTransform.InverseTransformDirection(currentTransform.localPosition - pos)*2f;
            diff = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), 1);

            currentPC.pd.ClampedScale(diff);
            currentChange = Vector3.Distance(currentPC.pd.relativeScale, scaleCache);
            //Debug.Log("Scale change: " + currentChange);
            currentPC.UpdateAllTransformValues();
            
            if(currentPC.mirroredPart != null){
                if(!currentPC.mirroredPart.detached)
                    currentPC.mirroredPart.UpdateAllTransformValues();
            }
            //currentPC.pd.SetPositionBounds();
        }else{
            Vector3 diff = currentTransform.InverseTransformDirection(currentTransform.localPosition - pos)*2f;
            diff = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), 1);
            

            if(!currentPC.flippedXAxis){
                currentTransform.localScale = currentPC.pd.GetClampedScale(diff);
            }else{
                currentTransform.localScale = currentPC.pd.GetFlippedClampedScale(diff);
            }
        }
    }

    private void SetPartRotation(Vector3 pos){

        pos -= transform.localPosition;

        float angle = Mathf.Atan2(pos.y - currentTransform.localPosition.y, pos.x - currentTransform.localPosition.x) * Mathf.Rad2Deg;
        currentChange = Mathf.Abs(angleCache - currentPC.pd.currentAngle)/180f;
        currentTransform.localRotation = Quaternion.Euler(0f, 0f, currentPC.pd.ClampedAngle(angle, currentPC.flippedXAxis));

        if(currentPC.mirroredPart != null){
            if(!currentPC.mirroredPart.detached)
                currentPC.mirroredPart.UpdateAllTransformValues();
        }
    }

    public void SetExpression(){

    }

}
