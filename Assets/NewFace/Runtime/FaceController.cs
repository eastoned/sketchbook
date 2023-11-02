using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using OpenCvSharp;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR;

public class FaceController : MonoBehaviour
{

    public PartController leftEye, rightEye;

    public float leftPupilX;
    public float leftPupilY;
    public float rightPupilX;
    public float rightPupilY;

    public Vector2 mousePos;
    public float clampVal;
    public PartController currentPC;
    public Transform currentTransform;

    [Range(0f, 1f)]
    public float blendCharacterValue;

    public string blendTo;

    public SaveCharacterProfile scp;

    public PartTransformController widthLeft, widthRight, heightTop, heightBottom;

     void OnEnable()
	{
        OnSelectedNewFacePartEvent.Instance.AddListener(SetTransformControllers);
        OnDeselectedFacePartEvent.Instance.AddListener(DisappearControllers);
        OnTranslatePartController.Instance.AddListener(SetPartPosition);
        OnRotatePartController.Instance.AddListener(SetPartRotation);
        OnScalePartController.Instance.AddListener(SetPartScale);
    }

    void OnDisable(){
        OnSelectedNewFacePartEvent.Instance.RemoveListener(SetTransformControllers);
        OnDeselectedFacePartEvent.Instance.RemoveListener(DisappearControllers);
        OnTranslatePartController.Instance.RemoveListener(SetPartPosition);
        OnRotatePartController.Instance.RemoveListener(SetPartRotation);
        OnScalePartController.Instance.RemoveListener(SetPartScale);
    }

    private void DisappearControllers(){
        widthLeft.transform.localPosition = new Vector3(100, 100, 100);
        widthRight.transform.localPosition = new Vector3(100, 100, 100);
        heightTop.transform.localPosition = new Vector3(100, 100, 100);
    }

    private void SetTransformControllers(Transform selectedTarget){
        
        currentPC = selectedTarget.GetComponent<PartController>();
        currentTransform = selectedTarget;
        DisappearControllers();

        if(currentPC.translatable)
            widthLeft.transform.localPosition = selectedTarget.TransformPoint(new Vector3(0, 0, 0));

        widthLeft.transform.localPosition = new Vector3(widthLeft.transform.localPosition.x, widthLeft.transform.localPosition.y, -1f);

        if(currentPC.rotatable)
            widthRight.transform.localPosition = selectedTarget.TransformPoint(new Vector3(-0.5f, 0, 0));

        if(currentPC.scalable)
            heightTop.transform.localPosition = selectedTarget.TransformPoint(new Vector3(0.5f, 0.5f, 0));
        
        heightTop.transform.localPosition = new Vector3(heightTop.transform.localPosition.x, heightTop.transform.localPosition.y, -1f);
    
    }

    private void SetPartPosition(Vector3 pos){
        //each part has a relative position to other objects
        float flip = 1;

        if(currentPC.flippedXAxis)
            flip = -1;

        currentTransform.localPosition = new Vector3(pos.x, pos.y, currentTransform.localPosition.z);
        Vector3 absPos = new Vector3(pos.x*flip, pos.y, currentTransform.localPosition.z);

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

    private void OnValidate(){
        Interpolate(blendCharacterValue, blendTo);
    }

    private void SetPartScale(Vector3 pos){

        float flip = 1;

        if(currentPC.flippedXAxis)
            flip = -1;
        
        Vector3 diff = currentTransform.InverseTransformDirection(currentTransform.localPosition - pos)*2f;
        diff = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), 1);

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

        float angle = Mathf.Atan2(currentTransform.localPosition.y - pos.y, currentTransform.localPosition.x - pos.x) * Mathf.Rad2Deg;
        if(!currentPC.flippedXAxis){
            currentTransform.localRotation = Quaternion.Euler(0f, 0f, ClampPartRotation(currentPC, angle));
        }else{
            currentTransform.localRotation = Quaternion.Euler(0f, 0f, ClampPartRotation(currentPC, angle));
        }
        currentPC.pd.currentAngle = currentTransform.localEulerAngles.z;
        
        if(currentPC.mirroredPart != null){
            currentPC.mirroredPart.UpdateAllTransformValues();
        }
        SetTransformControllers(currentTransform);
        
    }

    public float ClampPartRotation(PartController pc, float angle){
        return pc.pd.ClampedAngle(angle);
    }
    
    public Vector2 Rotate2D(Vector2 v, float delta) {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    public void Interpolate(float val, string blend){
        
    }

    public IEnumerator BlendIntoNewCharacter(CharacterData cd){
        yield return null;
    }

    void Update(){

        mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
       
        //position of mouse relative to right eye position
        Vector2 rightX = new Vector2(rightEye.pd.absolutePosition.x - mousePos.x, 0);
        Vector2 rightY = new Vector2(0, rightEye.pd.absolutePosition.y - mousePos.y);
        Vector2 rotatedRight = Rotate2D(rightX, -rightEye.pd.currentAngle * Mathf.Deg2Rad);
        Vector2 rotatedRight2 = Rotate2D(rightY, -rightEye.pd.currentAngle * Mathf.Deg2Rad);

        Vector2 leftX = new Vector2(-rightEye.pd.absolutePosition.x - mousePos.x, 0);
        Vector2 leftY = new Vector2(0, rightEye.pd.absolutePosition.y - mousePos.y);
        Vector2 rotatedLeft = Rotate2D(leftX, rightEye.pd.currentAngle * Mathf.Deg2Rad);
        Vector2 rotatedLeft2 = Rotate2D(leftY, rightEye.pd.currentAngle * Mathf.Deg2Rad);

        //clampVal = rightEye.pd.shadePropertyDict["_EyeRadius"].propertyValue/6f;
        //rightPupilX = rotatedRight.x - rotatedRight2.x;
        //rightPupilY = rotatedRight2.y - rotatedRight.y;

        Vector2 rightPupilTarget = new Vector2(rotatedRight.x - rotatedRight2.x, rotatedRight2.y - rotatedRight.y);
        Vector2 leftPupilTarget = new Vector2(rotatedLeft.x - rotatedLeft2.x, rotatedLeft2.y - rotatedLeft.y);

        rightPupilX = Mathf.Lerp(rightPupilX, rightPupilTarget.x*clampVal, 4f* Time.deltaTime);
        rightPupilY = Mathf.Lerp(rightPupilY, rightPupilTarget.y*clampVal, 4f* Time.deltaTime);
        leftPupilX = Mathf.Lerp(leftPupilX, leftPupilTarget.x*clampVal, 4f* Time.deltaTime);
        leftPupilY = Mathf.Lerp(leftPupilY, leftPupilTarget.y*clampVal, 4f* Time.deltaTime);
        
//
        //rightPupilX = Mathf.Lerp(rightPupilX, Mathf.Clamp(rightPupilTarget.x, -clampVal, clampVal), Time.deltaTime * 3f);
        //rightPupilY = Mathf.Lerp(rightPupilY, Mathf.Clamp(rightPupilTarget.y, -clampVal, clampVal), Time.deltaTime * 3f);
        //leftPupilX = Mathf.Lerp(leftPupilX, Mathf.Clamp(leftPupilTarget.x, -clampVal, clampVal), Time.deltaTime * 3f);
        //leftPupilY = Mathf.Lerp(leftPupilY, Mathf.Clamp(leftPupilTarget.y, -clampVal, clampVal), Time.deltaTime * 3f);
       // leftEye.UpdateSingleShaderValue("_PupilOffsetX", -leftPupilX);
       // leftEye.UpdateSingleShaderValue("_PupilOffsetY", leftPupilY);
       // leftEye.UpdateAllShadersValue(0f);
        //LeftEyeProp.SetFloat("_PupilOffsetX", -leftPupilX);
        //LeftEyeProp.SetFloat("_PupilOffsetY", leftPupilY);
       // rightEye.UpdateSingleShaderValue("_PupilOffsetX", rightPupilX);
       // rightEye.UpdateSingleShaderValue("_PupilOffsetY", rightPupilY);
        //rightEye.UpdateAllShadersValue(0f);
        //RightEyeProp.SetFloat("_PupilOffsetX", rightPupilX);
       // RightEyeProp.SetFloat("_PupilOffsetY", rightPupilY);
    }
}
