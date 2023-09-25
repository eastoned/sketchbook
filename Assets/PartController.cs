using System.Collections;
using System.Collections.Generic;
using OpenCvSharp;
using UnityEngine;
using UnityEngine.XR.WSA;

public class PartController : MonoBehaviour
{

    private int[] shaderIDs;

    [SerializeField] private Renderer rend;
    private Material currentMat;
    [SerializeField] private Material colliderMaterial;

    public PartData pd;

    public Collider colid;
    
    public bool flippedXAxis = false;

    public List<PartController> affectedParts = new List<PartController>();

    MaterialPropertyBlock propBlock;

    void Start(){
        currentMat = rend.sharedMaterial;
        if(!flippedXAxis){
            for(int i = 0; i < pd.shaderProperties.Count; i++){
                pd.shadePropertyDict.Add(pd.shaderProperties[i].propertyName, pd.shaderProperties[i]);
            }
        }

        UpdateAllTransformValues();

        if(affectedParts.Count > 0){
            for(int j = 0; j < affectedParts.Count; j++){
                affectedParts[j].pd.SetPositionBounds(pd);
                affectedParts[j].pd.SetScaleBounds(pd);
                affectedParts[j].UpdateAllTransformValues();
            }
        }

        propBlock = new MaterialPropertyBlock();
        UpdateAllShadersValue(0f);
       
    }

    void OnMouseEnter(){
        rend.sharedMaterials = new Material[2]{currentMat, colliderMaterial};
    }

    void OnMouseExit(){
        rend.sharedMaterials = new Material[1]{currentMat};
    }

    void OnMouseDown(){
        OnSelectedNewFacePartEvent.Instance.Invoke(transform);
    }


    void OnValidate(){
        propBlock = new MaterialPropertyBlock();
        rend = GetComponent<Renderer>();
        colid = GetComponent<Collider>();
        //Debug.Log("Does unity recognize updating a scriptable object the same as the inspector");
    }

    public void UpdateAllTransformValues(){
        
        transform.localPosition = pd.GetAbsolutePosition();
        if(flippedXAxis)
            transform.localPosition = new Vector3(-pd.absolutePosition.x, pd.absolutePosition.y, pd.absolutePosition.z);

        transform.localEulerAngles = new Vector3(0, 0, pd.currentAngle);
        if(flippedXAxis)
            transform.localEulerAngles = new Vector3(0, 0, -pd.currentAngle);

        transform.localScale = pd.GetAbsoluteScale();
        if(flippedXAxis)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        if(affectedParts.Count > 0){
            for(int j = 0; j < affectedParts.Count; j++){
                affectedParts[j].pd.SetPositionBounds(pd);
                affectedParts[j].pd.SetScaleBounds(pd);
                affectedParts[j].UpdateAllTransformValues();
            }
        }
    }


    public void UpdateAllShadersValue(float ignore){
        //MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
        for(int i = 0; i < pd.shaderProperties.Count; i++){
            UpdateSingleShaderValue(pd.shaderProperties[i].propertyName, pd.shaderProperties[i].propertyValue);
        }
        for(int j = 0; j < pd.shaderColors.Count; j++){
            UpdateSingleShaderColor(pd.shaderColors[j].colorName, pd.shaderColors[j].colorValue);
        }
        rend.SetPropertyBlock(propBlock);
    }

    public void UpdateSingleShaderValue(string param, float value){
        propBlock.SetFloat(param, value);
    }

    void UpdateSingleShaderColor(string param, Color col){
        propBlock.SetColor(param, col);
    }



}
