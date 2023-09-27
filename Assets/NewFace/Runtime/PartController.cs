using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public PartController mirroredPart; 

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
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        rend.sharedMaterials = new Material[2]{currentMat, colliderMaterial};
    }

    void OnMouseExit(){
        rend.sharedMaterials = new Material[1]{currentMat};
    }

    void OnMouseDown(){
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        OnSelectedNewFacePartEvent.Instance.Invoke(transform);
    }


    void OnValidate(){
        if(propBlock == null)
            propBlock = new MaterialPropertyBlock();

        if(rend == null)
            rend = GetComponent<Renderer>();

        if(colid == null)
            colid = GetComponent<Collider>();
    }

    public void UpdateAllTransformValues(){
        
        if(flippedXAxis){
            transform.localPosition = pd.GetFlippedAbsolutePosition();
        }else{
            transform.localPosition = pd.GetAbsolutePosition();
        }


        if(flippedXAxis){
            transform.localEulerAngles = new Vector3(0, 0, -pd.currentAngle);
        }else{
            transform.localEulerAngles = new Vector3(0, 0, pd.currentAngle);
        }

        if(flippedXAxis){
            transform.localScale = pd.GetFlippedAbsoluteScale();
        }else{
            transform.localScale = pd.GetAbsoluteScale();
        }

        if(affectedParts.Count > 0){
            for(int j = 0; j < affectedParts.Count; j++){
                affectedParts[j].pd.SetPositionBounds(pd);
                affectedParts[j].pd.SetScaleBounds(pd);
                affectedParts[j].UpdateAllTransformValues();
            }
        }

    }


    public void UpdateAllShadersValue(float ignore){

        for(int i = 0; i < pd.shaderProperties.Count; i++){
            UpdateSingleShaderValue(pd.shaderProperties[i].propertyName, pd.shaderProperties[i].propertyValue);
        }

        for(int j = 0; j < pd.shaderColors.Count; j++){
            UpdateSingleShaderColor(pd.shaderColors[j].colorName, pd.shaderColors[j].colorValue);
        }

        rend.SetPropertyBlock(propBlock);
    }

    public void UpdateAllShadersValue(){
        rend.SetPropertyBlock(propBlock);
    }


    public void UpdateSingleShaderValue(string param, float value){
        propBlock.SetFloat(param, value);
    }

    void UpdateSingleShaderColor(string param, Color col){
        propBlock.SetColor(param, col);
    }



}
