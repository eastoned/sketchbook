using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class PartUIController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI titleText;

    [SerializeField] private List<TextMeshProUGUI> sliderNames;
    [SerializeField] private List<Slider> sliders;

    public PartController partData;

    delegate void MyDelegate(string propName, float val);
    MyDelegate meDel;

    protected virtual void OnEnable()
	{
        OnSelectedNewFacePartEvent.Instance.AddListener(UpdateTitleText);
    }

    protected virtual void OnDisable(){
        OnSelectedNewFacePartEvent.Instance.RemoveListener(UpdateTitleText);
    }

    private void UpdateUISliders(){
        //UpdateTitleText();
    }

    private void UpdateTitleText(Transform transform){
        titleText.text = transform.name;
        partData = transform.GetComponent<PartController>(); 

        for(int i = 0; i < sliders.Count; i++){
            sliders[i].onValueChanged.RemoveAllListeners();
            //if(i <= partData.pd.shaderProperties.Count){
            //sliders[i].gameObject.SetActive(true);
            sliders[i].value = partData.pd.shaderProperties[i].propertyValue;
            sliderNames[i].text = partData.pd.shaderProperties[i].propertyName;
            
            if(partData.pd.shaderProperties[i].significant){
                sliders[i].onValueChanged.AddListener(partData.pd.shaderProperties[i].SignificantPiece);
            }
            
            sliders[i].onValueChanged.AddListener(partData.pd.shaderProperties[i].SetValue);
            
            //}else{
            //    sliders[i].gameObject.SetActive(false);
            //}
        }
        //transform.GetComponent<PartController>()
    }

    void UpdateShaderParams(float value){
        
    }

    void SendSliderValue(float value){
        //if(partData){
        //Debug.Log("Trying to set data for slider: " + id);
        //partData.UpdateSingleShaderValue("_Radius", partData.pd.shaderProperties[0].propertyValue);
        //}
    }


}
