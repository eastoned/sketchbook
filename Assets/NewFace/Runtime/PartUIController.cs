using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using AmplifyShaderEditor;

public class PartUIController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI titleText;

    [SerializeField] private List<TextMeshProUGUI> sliderNames;
    [SerializeField] private List<Slider> sliders;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private List<Slider> colorSliders;

    [SerializeField] private GameObject colorSliderContainer;

    public PartController partData;


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

    private void UpdateTitleText(Transform partTransform){
        
        titleText.text = partTransform.name;
        partData = partTransform.GetComponent<PartController>(); 
        //transform.position = Camera.main.WorldToScreenPoint(partTransform.position) + Camera.main.WorldToScreenPoint(new Vector3(partData.pd.absoluteScale.x, 0, 0))/4f;

        for(int i = 0; i < sliders.Count; i++){
            if(i < partData.pd.shaderProperties.Count){
                sliders[i].gameObject.SetActive(true);
                sliders[i].onValueChanged.RemoveAllListeners();
                //if(i <= partData.pd.shaderProperties.Count){
                //sliders[i].gameObject.SetActive(true);
                sliders[i].value = partData.pd.shaderProperties[i].propertyValue;
                //sliderNames[i].text = partData.pd.shaderProperties[i].propertyName;
                
                if(partData.pd.shaderProperties[i].significant){
                    sliders[i].onValueChanged.AddListener(partData.pd.shaderProperties[i].SignificantPiece);
                }
                
                sliders[i].onValueChanged.AddListener(partData.pd.shaderProperties[i].SetValue);
                sliders[i].onValueChanged.AddListener(partData.UpdateAllShadersValue);
            }else{
                sliders[i].gameObject.SetActive(false);
            }
            //}else{
            //    
            //}
        }

        for(int j = 0; j < buttons.Count; j++){
            if(j < partData.pd.shaderColors.Count){
                buttons[j].gameObject.SetActive(true);
                buttons[j].onClick.RemoveAllListeners();
                ColorBlock cb = ColorBlock.defaultColorBlock;
                cb.normalColor = partData.pd.shaderColors[j].colorValue;
                buttons[j].colors = cb;
                int l = j;
                buttons[j].onClick.AddListener(ToggleColorSliders);
                buttons[j].onClick.AddListener(() => SetSlidersForCurrentColor(l));
            }else{
                buttons[j].gameObject.SetActive(false);
            }
        }

    }

    void ToggleColorSliders(){
        colorSliderContainer.SetActive(!colorSliderContainer.activeInHierarchy);
    }

    void SetSlidersForCurrentColor(int currentColor){
        Debug.Log("Sliders set from buttons");
        for(int i = 0; i < colorSliders.Count; i++){
            colorSliders[i].onValueChanged.RemoveAllListeners();
        }
        colorSliders[0].value = partData.pd.shaderColors[currentColor].GetHue();
        colorSliders[1].value = partData.pd.shaderColors[currentColor].GetSaturation();
        colorSliders[2].value = partData.pd.shaderColors[currentColor].GetValue();

        colorSliders[0].onValueChanged.AddListener(partData.pd.shaderColors[currentColor].SetHue);
        colorSliders[0].onValueChanged.AddListener(partData.UpdateAllShadersValue);
        colorSliders[1].onValueChanged.AddListener(partData.pd.shaderColors[currentColor].SetSaturation);
        colorSliders[1].onValueChanged.AddListener(partData.UpdateAllShadersValue);
        colorSliders[2].onValueChanged.AddListener(partData.pd.shaderColors[currentColor].SetValue);
        colorSliders[2].onValueChanged.AddListener(partData.UpdateAllShadersValue);

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
