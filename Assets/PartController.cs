using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PartController : MonoBehaviour
{

    private int[] shaderIDs;

    [SerializeField] private Renderer rend;

    public PartData pd;

    public Collider colid;

    MaterialPropertyBlock propBlock;

    void Start(){
        propBlock = new MaterialPropertyBlock();
        //shaderIDs = new int[shaderParams.Count];
        //for(int i = 0; i < shaderParams.Count; i++){
        //    shaderIDs[i] = Shader.PropertyToID(shaderParams[i]);
       // }
    }

    void OnEnable()
	{
        OnDeselectedFacePartEvent.Instance.AddListener(ToggleColliderOn);
        OnSelectedNewFacePartEvent.Instance.AddListener(ToggleColliderOff);
    }

    void OnDisable(){
        OnDeselectedFacePartEvent.Instance.RemoveListener(ToggleColliderOn);
        OnSelectedNewFacePartEvent.Instance.AddListener(ToggleColliderOff);
    }

    void ToggleColliderOff(Transform other){
        if(other != transform)
            colid.enabled = false;
    }

    void ToggleColliderOn(){
        colid.enabled = true;
    }

    void Update(){
        UpdateAllShadersValue();
    }

    void OnMouseOver(){
        OnSelectedNewFacePartEvent.Instance.Invoke(transform);
    }

    void OnMouseExit(){
        OnDeselectedFacePartEvent.Instance.Invoke();
    }

    void OnValidate(){
        propBlock = new MaterialPropertyBlock();
        rend = GetComponent<Renderer>();
        colid = GetComponent<Collider>();
        Debug.Log("Does unity recognize updating a scriptable object the same as the inspector");
    }


    void UpdateAllShadersValue(){
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
