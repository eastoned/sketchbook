using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartController : MonoBehaviour
{

    private int[] shaderIDs;

    public Renderer rend;
    private Material currentMat;
    [SerializeField] private Material colliderMaterial;

    public PartData pd;

    public bool translatable, rotatable, scalable;

    public BoxCollider2D colid;
    
    public bool flippedXAxis = false;

    public List<PartController> affectedParts = new List<PartController>();
    public PartController mirroredPart; 

    MaterialPropertyBlock propBlock;

    public Vector3 cachePosition;

    private void Start(){
        Initialize();
    }

    private void Initialize(){
        currentMat = rend.sharedMaterial;
        colid = GetComponent<BoxCollider2D>();
        
        if(!flippedXAxis){
            for(int i = 0; i < pd.shaderProperties.Count; i++){
                pd.shadePropertyDict.Add(pd.shaderProperties[i].propertyName, pd.shaderProperties[i]);
            }
        }

        UpdateAllTransformValues();

        UpdateDependencies();

        propBlock = new MaterialPropertyBlock();
        UpdateAllShadersValue(0f);
    }

    public void UpdateDependencies(){
        if(affectedParts.Count > 0){
            for(int j = 0; j < affectedParts.Count; j++){
                affectedParts[j].pd.SetPositionBounds(pd);
                affectedParts[j].pd.SetScaleBounds(pd);
                affectedParts[j].UpdateAllTransformValues();
            }
        }
    }

    void OnMouseEnter(){
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        if(Input.GetMouseButton(0))
            return;

        rend.sharedMaterials = new Material[2]{currentMat, colliderMaterial};
    }

    void OnMouseExit(){
        if(Input.GetMouseButton(0))
            return;
            
        rend.sharedMaterials = new Material[1]{currentMat};
    }

    void OnMouseDown(){
        OnMouseClickEvent.Instance.Invoke();
        
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        OnSelectedNewFacePartEvent.Instance.Invoke(transform);
        //colid.enabled = false;
    }


    void OnValidate(){
        //Initialize();

        if(propBlock == null)
            propBlock = new MaterialPropertyBlock();

        if(rend == null)
            rend = GetComponent<Renderer>();

        if(colid == null)
            colid = GetComponent<BoxCollider2D>();
    }

    public void UpdateAllTransformValues(){
        
        if(flippedXAxis){
            transform.localPosition = pd.GetFlippedAbsolutePosition();
            cachePosition = pd.GetFlippedAbsolutePosition();
        }else{
            transform.localPosition = pd.GetAbsolutePosition();
            cachePosition = pd.GetAbsolutePosition();
        }

        if(flippedXAxis){
            transform.localRotation = Quaternion.Euler(0, 0, -pd.currentAngle);
        }else{
            transform.localRotation = Quaternion.Euler(0, 0, pd.currentAngle);
        }

        if(flippedXAxis){
            transform.localScale = pd.GetFlippedAbsoluteScale();
        }else{
            transform.localScale = pd.GetAbsoluteScale();
        }

        UpdateDependencies();


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

    [ContextMenu("Shake Test")]
    public void ShakeTest(){
        ShakePieces(new Vector3(.1f, 0.01f, 0f), .5f);
    }

    public void ShakePieces(Vector3 strength, float time){
        StartCoroutine(ShakeRoutine(strength, time));
    }

    public IEnumerator ShakeRoutine(Vector3 strength, float length){
        float time = length;
        while(time > 0){
            time -= Time.deltaTime;
            transform.localPosition = cachePosition + Vector3.Scale(Random.insideUnitSphere, strength);
            yield return null;
        }
        transform.localPosition = cachePosition;
    }

    public void UpdateAllShadersValue(){
        rend.SetPropertyBlock(propBlock);
    }

    public void UpdateRenderPropBlock(){
        rend.SetPropertyBlock(propBlock);
    }

    public void UpdateSingleShaderValue(string param, float value){
        propBlock.SetFloat(param, value);
    }

    public void UpdateSingleShaderVector(string param, Vector3 vec){
        propBlock.SetVector(param, vec);
    }

    void UpdateSingleShaderColor(string param, Color col){
        propBlock.SetColor(param, col);
    }

}
