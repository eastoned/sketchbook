using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
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
    public bool notInteracting = false;

    public override void OnEnable()
	{
        base.OnEnable();
        OnHoveredNewFacePartEvent.Instance.AddListener(SetMaterialOutline);
        OnSelectedNewFacePartEvent.Instance.AddListener(SetTransformControllers);
        OnSetTransformCacheEvent.Instance.AddListener(SetTransformCache);
        OnDeselectedFacePartEvent.Instance.AddListener(ClearCurrentHover);
        OnDeselectedFacePartEvent.Instance.AddListener(DisappearControllers);
        OnTranslatePartController.Instance.AddListener(SetPartPosition);
        OnRotatePartController.Instance.AddListener(SetPartRotation);
        OnScalePartController.Instance.AddListener(SetPartScale);
        OnConfirmTransformPart.Instance.AddListener(UpdateMoneyAmount);
    }

    public override void OnDisable(){
        base.OnDisable();
        OnHoveredNewFacePartEvent.Instance.RemoveListener(SetMaterialOutline);
        OnSelectedNewFacePartEvent.Instance.RemoveListener(SetTransformControllers);
        OnSetTransformCacheEvent.Instance.RemoveListener(SetTransformCache);
        OnDeselectedFacePartEvent.Instance.RemoveListener(ClearCurrentHover);
        OnDeselectedFacePartEvent.Instance.RemoveListener(DisappearControllers);
        OnTranslatePartController.Instance.RemoveListener(SetPartPosition);
        OnRotatePartController.Instance.RemoveListener(SetPartRotation);
        OnScalePartController.Instance.RemoveListener(SetPartScale);
        OnConfirmTransformPart.Instance.RemoveListener(UpdateMoneyAmount);
    }

    private void UpdateMoneyAmount(){
        NuFaceManager.money -= currentChange;
        currentChange = 0f;
    }

    public void SetMaterialOutline(Transform hoveredTarget){
        ClearCurrentHover();
        if(hoveredTarget){
            if(hoveredTarget.GetComponent<PartController>()){
                hoveredTarget.GetComponent<Renderer>().sharedMaterials = new Material[2]{hoveredTarget.GetComponent<Renderer>().sharedMaterials[0], colliderMaterial};
                hoveredTransform = hoveredTarget;
            }
        }
    }

    private void StartCrying(){
        mouth.UpdateSingleShaderValue("_MouthBend", 0f);
        mouth.UpdateRenderPropBlock();
    }

    private void StopCrying(){
        mouth.UpdateSingleShaderValue("_MouthBend", 1f);
        mouth.UpdateRenderPropBlock();
    }

    private void ClearCurrentHover(){
        if(hoveredTransform != null){
            //Debug.Log(hoveredTransform.name);
            hoveredTransform.GetComponent<Renderer>().sharedMaterials = new Material[1]{hoveredTransform.GetComponent<Renderer>().sharedMaterials[0]};
        }
    }

    private void DisappearControllers(){
        rotationController.transform.localPosition = new Vector3(100, 100, 100);
        scaleController.transform.localPosition = new Vector3(100, 100, 100);
    }

    private void SetTransformCache(){
        if(currentPC != null){
            ///Debug.Log("Setting caches");
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
        
        if(currentTransform != selectedTarget){
            currentTransform = selectedTarget;
        
            cube.position = currentTransform.position;
        }
        
        DisappearControllers(); 

        if(currentPC.rotatable){
            rotationController.transform.localPosition = selectedTarget.TransformPoint(new Vector3(0.5f, 0, 0));
            rotationController.transform.localPosition = new Vector3(rotationController.transform.localPosition.x, rotationController.transform.localPosition.y, -1f);
        }
            
        if(currentPC.scalable){
            scaleController.transform.localPosition = selectedTarget.TransformPoint(new Vector3(0.5f, 0.5f, 0));
            scaleController.transform.localPosition = new Vector3(scaleController.transform.localPosition.x, scaleController.transform.localPosition.y, -1f);
        }
    }

    private void SetPartPosition(Vector3 pos){
        //each part has a relative position to other objects
        float flip = currentPC.flippedXAxis? -1f : 1f;
        //Debug.Log(currentPC.pd.GetAbsolutePosition() - pos);
        
        currentTransform.localPosition = new Vector3(pos.x, pos.y, currentTransform.localPosition.z);
        Vector3 absPos = new Vector3(pos.x*flip, pos.y, currentTransform.localPosition.z);

        
        currentPC.pd.ClampedPosition(absPos);

        currentChange = Vector2.Distance(currentPC.pd.relativePosition, positionCache);
        //Debug.Log("Position change: " + currentPC.pd.relativePosition + " is the clamped pos : " + positionCache + "is the abs position: " +  currentChange);

        currentPC.UpdateAllTransformValues();
        
        if(currentPC.mirroredPart != null){
            currentPC.mirroredPart.UpdateAllTransformValues();
        }
        
        SetTransformControllers(currentTransform);
    }

    private void SetPartScale(Vector3 pos){
        pos -= transform.localPosition;

        float flip = 1;

        if(currentPC.flippedXAxis){
            flip = -1;
        }
        
        Vector3 diff = currentTransform.InverseTransformDirection(currentTransform.localPosition - pos)*2f;
        diff = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), 1);

        
        
        currentPC.pd.ClampedScale(diff);
        currentChange = Vector3.Distance(currentPC.pd.relativeScale, scaleCache);
        //Debug.Log("Scale change: " + currentChange);
        currentPC.UpdateAllTransformValues();
        
        if(currentPC.mirroredPart != null){
            currentPC.mirroredPart.UpdateAllTransformValues();
        }
        //currentPC.pd.SetPositionBounds();

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
            currentPC.mirroredPart.UpdateAllTransformValues();
        }
        
        SetTransformControllers(currentTransform);
        
    }

}
