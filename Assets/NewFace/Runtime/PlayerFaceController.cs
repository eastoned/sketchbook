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
    public Vector3 positionDifference;

    public PartTransformController rotationController, scaleController;

    public PartController currentHovered;
    [SerializeField] private Material colliderMaterial;

    public float currentChange = 0f;

    public override void OnEnable()
	{
        base.OnEnable();
        OnHoveredNewFacePartEvent.Instance.AddListener(SetMaterialOutline);
        OnSelectedNewFacePartEvent.Instance.AddListener(SetTransformControllers);
        OnDeselectedFacePartEvent.Instance.AddListener(ClearCurrentHover);
        OnDeselectedFacePartEvent.Instance.AddListener(DisappearControllers);
        OnTranslatePartController.Instance.AddListener(SetPartPosition);
        OnRotatePartController.Instance.AddListener(SetPartRotation);
        OnScalePartController.Instance.AddListener(SetPartScale);
        OnSetKeyFrameData.Instance.AddListener(SetDefaultBlinkAndMouthPos);
        OnConfirmTransformPart.Instance.AddListener(UpdateMoneyAmount);
    }

    public override void OnDisable(){
        base.OnDisable();
        OnHoveredNewFacePartEvent.Instance.RemoveListener(SetMaterialOutline);
        OnSelectedNewFacePartEvent.Instance.RemoveListener(SetTransformControllers);
        OnDeselectedFacePartEvent.Instance.RemoveListener(ClearCurrentHover);
        OnDeselectedFacePartEvent.Instance.RemoveListener(DisappearControllers);
        OnTranslatePartController.Instance.RemoveListener(SetPartPosition);
        OnRotatePartController.Instance.RemoveListener(SetPartRotation);
        OnScalePartController.Instance.RemoveListener(SetPartScale);
        OnSetKeyFrameData.Instance.RemoveListener(SetDefaultBlinkAndMouthPos);
        OnConfirmTransformPart.Instance.AddListener(UpdateMoneyAmount);
    }

    private void UpdateMoneyAmount(){
        //NuFaceManager.money -= currentChange;
        currentChange = 0f;
    }

    public void SetMaterialOutline(Transform hoveredTarget){
        ClearCurrentHover();
        if(hoveredTarget.GetComponent<PartController>()){
            hoveredTarget.GetComponent<Renderer>().sharedMaterials = new Material[2]{hoveredTarget.GetComponent<Renderer>().sharedMaterials[0], colliderMaterial};
            hoveredTransform = hoveredTarget;
        }
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

    private void SetTransformControllers(Transform selectedTarget){
        
        currentPC = selectedTarget.GetComponent<PartController>();
        
        if(currentTransform != selectedTarget){
            currentTransform = selectedTarget;
        
            cube.position = currentTransform.position;
        }
        
        DisappearControllers();

        if(currentPC.rotatable)
            rotationController.transform.localPosition = selectedTarget.TransformPoint(new Vector3(0.5f, 0, 0));
        
        rotationController.transform.localPosition = new Vector3(rotationController.transform.localPosition.x, rotationController.transform.localPosition.y, -1f);

        if(currentPC.scalable)
            scaleController.transform.localPosition = selectedTarget.TransformPoint(new Vector3(0.5f, 0.5f, 0));
        
        scaleController.transform.localPosition = new Vector3(scaleController.transform.localPosition.x, scaleController.transform.localPosition.y, -1f);
    
    }

    private void SetPartPosition(Vector3 pos){
        //each part has a relative position to other objects
        float flip = currentPC.flippedXAxis? -1f : 1f;
        //Debug.Log(currentPC.pd.GetAbsolutePosition() - pos);
        
        currentTransform.localPosition = new Vector3(pos.x, pos.y, currentTransform.localPosition.z);
        Vector3 absPos = new Vector3(pos.x*flip, pos.y, currentTransform.localPosition.z);

        currentChange = Vector3.Distance(currentPC.pd.ReturnClampedPosition(pos), currentPC.pd.GetAbsolutePosition());
        

        currentPC.pd.ClampedPosition(absPos);

        if(currentPC.affectedParts.Count > 0){
            for(int i = 0; i < currentPC.affectedParts.Count; i++){
                currentPC.affectedParts[i].pd.SetPositionBounds(currentPC.pd);
                currentPC.affectedParts[i].pd.SetScaleBounds(currentPC.pd);

                currentPC.affectedParts[i].UpdateAllTransformValues();
            }
        }

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

        currentChange = Vector3.Distance(diff, currentPC.pd.GetAbsoluteScale());

        currentPC.pd.ClampedScale(diff);

        if(currentPC.affectedParts.Count > 0){
            for(int i = 0; i < currentPC.affectedParts.Count; i++){
                currentPC.affectedParts[i].pd.SetPositionBounds(currentPC.pd);
                currentPC.affectedParts[i].pd.SetScaleBounds(currentPC.pd);
                currentPC.affectedParts[i].UpdateAllTransformValues();
            }
        }

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

        currentChange = Mathf.Abs(angle - currentPC.pd.currentAngle);
        //Debug.Log("The current angle diff: " + currentChange);

        currentTransform.localRotation = Quaternion.Euler(0f, 0f, currentPC.pd.ClampedAngle(angle, currentPC.flippedXAxis));
        
        if(currentPC.mirroredPart != null){
            currentPC.mirroredPart.UpdateAllTransformValues();
        }
        
        SetTransformControllers(currentTransform);
        
    }

}
