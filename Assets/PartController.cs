using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;


public class PartController : MonoBehaviour
{

    private int[] shaderIDs;

    [SerializeField] private Renderer rend;

    public PartData pd;

    public Collider colid;
    
    public bool flippedXAxis = false;

    MaterialPropertyBlock propBlock;

    void Start(){
        if(!flippedXAxis){
            for(int i = 0; i < pd.shaderProperties.Count; i++){
                pd.shadePropertyDict.Add(pd.shaderProperties[i].propertyName, pd.shaderProperties[i]);
            }
        }
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
       // if(other != transform)
           // colid.enabled = false;
    }

    void ToggleColliderOn(){
        //colid.enabled = true;
    }

    void Update(){
        UpdateAllTransformValues();
        UpdateAllShadersValue();
    }

    void OnMouseEnter(){
        Debug.Log("Entered object");
        OnSelectedNewFacePartEvent.Instance.Invoke(transform);
    }

    void OnMouseExit(){
        //OnDeselectedFacePartEvent.Instance.Invoke();
    }

    void OnValidate(){
        propBlock = new MaterialPropertyBlock();
        rend = GetComponent<Renderer>();
        colid = GetComponent<Collider>();
        //Debug.Log("Does unity recognize updating a scriptable object the same as the inspector");
    }

    void UpdateAllTransformValues(){
        transform.localPosition = pd.position;
        if(flippedXAxis)
            transform.localPosition = new Vector3(-pd.position.x, pd.position.y, pd.position.z);

        transform.localEulerAngles = new Vector3(0, 0, pd.currentAngle);
        if(flippedXAxis)
            transform.localEulerAngles = new Vector3(0, 0, -pd.currentAngle);

        transform.localScale = pd.scale;
        if(flippedXAxis)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
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
