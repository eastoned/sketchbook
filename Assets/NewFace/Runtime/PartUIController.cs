using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class PartUIController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI titleText;

    [SerializeField] private List<TextMeshProUGUI> sliderNames;
    [SerializeField] private List<Slider> sliders;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private List<Slider> colorSliders;

    [SerializeField] private GameObject colorSliderContainer;

    public PartController partData;

    void OnEnable()
	{
        OnSelectedNewFacePartEvent.Instance.AddListener(UpdateTitleText);
        OnDeselectedFacePartEvent.Instance.AddListener(TurnOffUI);
    }

    void OnDisable()
    {
        OnSelectedNewFacePartEvent.Instance.RemoveListener(UpdateTitleText);
        OnDeselectedFacePartEvent.Instance.RemoveListener(TurnOffUI);
    }

    private void UpdateTitleText(Transform partTransform)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        titleText.text = partTransform.name;
        partData = partTransform.GetComponent<PartController>();
        colorSliderContainer.SetActive(false); 

        for(int i = 0; i < sliders.Count; i++){
            if(i < partData.pd.shaderProperties.Count){
                sliders[i].gameObject.SetActive(true);
                sliders[i].onValueChanged.RemoveAllListeners();
                //if(i <= partData.pd.shaderProperties.Count){
                //sliders[i].gameObject.SetActive(true);
                sliders[i].value = partData.pd.shaderProperties[i].propertyValue;
                //sliderNames[i].text = partData.pd.shaderProperties[i].propertyName;
                //can know a part was changed but we need to send more information?
                // do we send a key that the speech controller reads from?
                //at minimum we need to send the part that was changed/what shader property it was, and what direction it went
                ///if(partData.pd.shaderProperties[i].propertyChange != null){
                 //   sliders[i].onValueChanged.AddListener(partData.pd.shaderProperties[i].propertyChange);
                //}
                
                sliders[i].onValueChanged.AddListener(partData.pd.shaderProperties[i].SetValue);
                sliders[i].onValueChanged.AddListener(partData.UpdateAllShadersValue);
                
                if(partData.mirroredPart != null){
                    sliders[i].onValueChanged.AddListener(partData.mirroredPart.UpdateAllShadersValue);
                }
            }else{
                sliders[i].gameObject.SetActive(false);
            }
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

    void ToggleColorSliders()
    {
        colorSliderContainer.SetActive(!colorSliderContainer.activeInHierarchy);
    }

    void TurnOffUI(){
        transform.GetChild(0).gameObject.SetActive(false);
        colorSliderContainer.SetActive(false);
    }

    void SetSlidersForCurrentColor(int currentColor)
    {
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

        if(partData.mirroredPart != null){
            colorSliders[0].onValueChanged.AddListener(partData.mirroredPart.UpdateAllShadersValue);
            colorSliders[1].onValueChanged.AddListener(partData.mirroredPart.UpdateAllShadersValue);
            colorSliders[2].onValueChanged.AddListener(partData.mirroredPart.UpdateAllShadersValue);
        }

    }

}
