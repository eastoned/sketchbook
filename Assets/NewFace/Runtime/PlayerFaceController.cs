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
    public GameObject node;

    public PartTransformController rotationController, scaleController;

    public PartController currentHovered;
    [SerializeField] private Material colliderMaterial;

    public float currentChange = 0f;
    public bool notInteracting = false;

    public SpeechController sc;
    private MaterialPropertyBlock block;
    public Renderer tear1, tear2;
    public List<GameObject> availableNodes;
    public ParticleSystem blood;
    public Rigidbody2D hoveredParent;
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

    public void SetMaterialOutline(Transform hoveredTarget){
        RemoveMaterialOutlineFromPreviousHover();
        if(hoveredTarget != null){
            if(hoveredTarget.GetComponent<PartController>()){
                hoveredTarget.GetComponent<Renderer>().sharedMaterials = new Material[2]{hoveredTarget.GetComponent<Renderer>().sharedMaterials[0], colliderMaterial};
                hoveredTransform = hoveredTarget;
            }
        }
    }

    public void StartCrying(){
        mouth.UpdateSingleShaderFloat("_MouthBend", 1f);
        block.SetFloat("_Radius", 1f);
        tear1.SetPropertyBlock(block);
        tear2.SetPropertyBlock(block);
        mouth.UpdateRenderPropBlock();
        ShakeAll();
    }

    public void StopCrying(){
        mouth.UpdateSingleShaderFloat("_MouthBend", 0f);
        block.SetFloat("_Radius", .3f);
        tear1.SetPropertyBlock(block);
        tear2.SetPropertyBlock(block);
        mouth.UpdateRenderPropBlock();
    }

    public void ShakeAll(){
        leftEye.ShakeTest();
        rightEye.ShakeTest();
        mouth.ShakeTest();
        nose.ShakeTest();
        head.ShakeTest();
        leftEyebrow.ShakeTest();
        rightEyebrow.ShakeTest();
        bangs.ShakeTest();
        hair.ShakeTest();
        neck.ShakeTest();
        leftEar.ShakeTest();
        rightEar.ShakeTest();
    }

    private void Comment(){
        Debug.Log("Beeing comment");
        if(NuFaceManager.canShareFeedback){

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

    private void DisappearControllers(){
        rotationController.transform.localPosition = new Vector3(100, 100, 100);
        scaleController.transform.localPosition = new Vector3(100, 100, 100);
        //scaleController.transform.SetParent(null);
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
        //Debug.Log("Set the transform controllers on object");
        if(currentTransform != selectedTarget){
            currentTransform = selectedTarget;
        
            cube.position = currentTransform.position;
        }
        
        DisappearControllers(); 

        if(currentPC.rotatable){
            rotationController.transform.localPosition = selectedTarget.TransformPoint(new Vector3(0.5f, 0, 0));
            rotationController.transform.localPosition = new Vector3(rotationController.transform.localPosition.x, rotationController.transform.localPosition.y, -1f);
            rotationController.transform.localScale = Vector3.one * currentTransform.localScale.y * 0.25f;
        }
            
        if(currentPC.scalable){
            scaleController.transform.localPosition = selectedTarget.TransformPoint(new Vector3(0.5f, 0.5f, 0));
            scaleController.transform.localPosition = new Vector3(scaleController.transform.localPosition.x, scaleController.transform.localPosition.y, -1f);
            scaleController.transform.localScale = Vector3.one * currentTransform.localScale.y * 0.25f;
            //if(currentPC.detached){
                //scaleController.transform.SetParent(selectedTarget);
            //}
        }

        //if(NuFaceManager.couldBeShared){
        //    sc.SpeakEvent("Did you already share that photo of me?");
        //}
    }

    private void UpdatePartAttachmentStatus(PartController pc, bool status){
        pc.UpdateAttachmentStatus(status, hoveredParent);
        if(pc.flippedXAxis){
            Instantiate(blood, pc.pd.GetFlippedAbsolutePosition(), Quaternion.identity);
            bool didSpawn = false;
            foreach(GameObject nod in availableNodes){
                if(!nod.activeInHierarchy){
                    nod.transform.position = pc.pd.GetFlippedAbsolutePosition();
                    nod.SetActive(true);
                    didSpawn = true;
                    break;
                }
            }

            if(!didSpawn){
                Debug.Log("add node if didn't have enough avilaable");
                availableNodes.Add(Instantiate(node, pc.pd.GetFlippedAbsolutePosition(), pc.pd.GetAbsoluteRotation()));
            }//add node to list
        }else{
            Instantiate(blood, pc.pd.GetAbsolutePosition(), Quaternion.identity);
            bool didSpawn = false;
            foreach(GameObject nod in availableNodes){
                if(!nod.activeInHierarchy){
                    nod.transform.position = pc.pd.GetAbsolutePosition();
                    nod.SetActive(true);
                    didSpawn = true;
                    break;
                }
            }
            if(!didSpawn){
                Debug.Log("add node if didn't have enough avilaable");
                availableNodes.Add(Instantiate(node, pc.pd.GetAbsolutePosition(), pc.pd.GetAbsoluteRotation()));
            }
        }
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
                    if(currentPC.affectedParts.Count > 0){
                        foreach(PartController pc in currentPC.affectedParts){
                            UpdatePartAttachmentStatus(pc, true);
                        }
                    }
                    //currentPC.UpdateAttachmentStatus(true);
                    //set empty node here
                }
                    
            }
        }else{
            currentTransform.localPosition = new Vector3(pos.x, pos.y, currentTransform.localPosition.z);
            currentPC.overAttachmentNode = false;
            currentPC.parent = null;
            for(int i = 0; i < bodyParts.Length; i++){
                if(currentTransform != bodyParts[i]){
                    if(bodyParts[i].GetComponent<BoxCollider2D>().OverlapPoint(currentTransform.position)){
                        hoveredParent = bodyParts[i].GetComponent<Rigidbody2D>();
                        currentPC.overAttachmentNode = true;
                        currentPC.parent = hoveredParent;
                   //currentPC.attachPosition = availableNodes[i].transform.position;
                        //currentPC.nodeToDelete = availableNodes[i];
                    }
                }
            }
            //if position is on  node then we can attach to it
        }
        
        //Debug.Log(currentPC.pd.GetAbsolutePosition() - pos);

        SetTransformControllers(currentTransform);
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
        SetTransformControllers(currentTransform);
    }

    private void SetPartRotation(Vector3 pos){

        pos -= transform.localPosition;

        float angle = Mathf.Atan2(pos.y - currentTransform.localPosition.y, pos.x - currentTransform.localPosition.x) * Mathf.Rad2Deg;

        if(angle < currentPC.pd.currentAngle){
            //Debug.Log("The new angle is less than the current angle");
        }else if (angle > currentPC.pd.currentAngle){
          //  Debug.Log("The new angle is greater than the current angle");
        }

        currentChange = Mathf.Abs(angleCache - currentPC.pd.currentAngle)/180f;
        //Debug.Log("The current angle diff: " + currentChange);
        //Debug.Log("Angle change: " + currentChange);

        currentTransform.localRotation = Quaternion.Euler(0f, 0f, currentPC.pd.ClampedAngle(angle, currentPC.flippedXAxis));
        
        if(currentPC.mirroredPart != null){
            if(!currentPC.mirroredPart.detached)
                currentPC.mirroredPart.UpdateAllTransformValues();
        }
        
        SetTransformControllers(currentTransform);
        
    }

}
