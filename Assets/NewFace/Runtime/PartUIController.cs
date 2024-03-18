using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class PartUIController : MonoBehaviour
{
    [SerializeField] private GameObject editButton;

    [SerializeField] private TextMeshProUGUI titleText;

    [SerializeField] private List<TextMeshProUGUI> sliderNames;
    [SerializeField] private List<Slider> sliders;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private List<Slider> colorSliders;

    [SerializeField] private GameObject colorSliderContainer;

    public PartController partData;

    void OnEnable()
	{
        OnSelectedNewFacePartEvent.Instance.AddListener(EnableEditButton);
        OnDeselectedFacePartEvent.Instance.AddListener(TurnOffUI);
    }

    void OnDisable()
    {
        OnSelectedNewFacePartEvent.Instance.RemoveListener(EnableEditButton);
        OnDeselectedFacePartEvent.Instance.RemoveListener(TurnOffUI);
    }

    private void EnableEditButton(Transform partTransform){
        TurnOffUI();
        editButton.SetActive(true);
        titleText.text = partTransform.name;
        partData = partTransform.GetComponent<PartController>();
    }

    public void UpdateTitleText()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        colorSliderContainer.SetActive(false); 

        for(int i = 0; i < sliders.Count; i++){
            if(i < partData.pd.shaderProperties.Count){
                sliders[i].gameObject.SetActive(true);
                sliders[i].onValueChanged.RemoveAllListeners();
                sliders[i].value = partData.pd.shaderProperties[i].propertyValue;
                sliders[i].name = partData.pd.shaderProperties[i].propertyName;
                //if(partData.pd.shaderProperties[i].IncreaseValueRemarks.Length > 0 || partData.pd.shaderProperties[i].DecreaseValueRemarks.Length > 0){
                    //sliders[i].onValueChanged.AddListener(partData.pd.shaderProperties[i].ReadRandomRemark);
                //}
                sliders[i].onValueChanged.AddListener(partData.pd.shaderProperties[i].SetValue);
                sliders[i].onValueChanged.AddListener(partData.UpdateAllShadersValue);
                if(partData.pd.shaderProperties[i].wholeNumberInterval){
                    float intervalValue = partData.pd.shaderProperties[i].valueInterval;
                    sliders[i].onValueChanged.AddListener(delegate{SetCurrentShaderInterval.Instance.Invoke(intervalValue);});
                    sliders[i].onValueChanged.AddListener(OnChangedShaderProperty.Instance.Invoke);
                }else{
                    sliders[i].onValueChanged.AddListener(OnSlideShaderProperty.Instance.Invoke);
                }

                if(partData.pd.shaderProperties[i].affectedFeature != ShaderProperty.AffectedFeature.NOTHING){
                    //sliders[i].onValueChanged.AddListener(delegate{OnAffectFeatureWithShaderProperty.Instance.Invoke(partData.pd.shaderProperties[i].affectedFeature);});
                }
                
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
                cb.pressedColor = partData.pd.shaderColors[j].colorValue;
                cb.highlightedColor = Color.white - partData.pd.shaderColors[j].colorValue;
                buttons[j].colors = cb;
                int l = j;
                buttons[j].onClick.AddListener(ToggleColorSliders);
                buttons[j].onClick.AddListener(() => SetSlidersForCurrentColor(l));
            }else{
                buttons[j].gameObject.SetActive(false);
            }
        }

    }

    void UpdateEyeMouth(float ignore){
        OnSetKeyFrameData.Instance.Invoke();
    }

    void UpdateButtonColor(float ignore){
        for(int j = 0; j < partData.pd.shaderColors.Count; j++){
            ColorBlock cb = ColorBlock.defaultColorBlock;
            cb.normalColor = partData.pd.shaderColors[j].colorValue;
            cb.pressedColor = partData.pd.shaderColors[j].colorValue;
            cb.highlightedColor = Color.white - partData.pd.shaderColors[j].colorValue;
            buttons[j].colors = cb;
        }
    }

    void ToggleColorSliders()
    {
        colorSliderContainer.SetActive(true);
    }

    void TurnOffUI(){
        editButton.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        colorSliderContainer.SetActive(false);
    }

    void SetCurrentInterval(float intervalValue){
        SetCurrentShaderInterval.Instance.Invoke(intervalValue);
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
        colorSliders[0].onValueChanged.AddListener(UpdateButtonColor);

        colorSliders[1].onValueChanged.AddListener(partData.pd.shaderColors[currentColor].SetSaturation);
        colorSliders[1].onValueChanged.AddListener(partData.UpdateAllShadersValue);
        colorSliders[1].onValueChanged.AddListener(UpdateButtonColor);

        colorSliders[2].onValueChanged.AddListener(partData.pd.shaderColors[currentColor].SetValue);
        colorSliders[2].onValueChanged.AddListener(partData.UpdateAllShadersValue);
        colorSliders[2].onValueChanged.AddListener(UpdateButtonColor);

        if(partData.mirroredPart != null){
            colorSliders[0].onValueChanged.AddListener(partData.mirroredPart.UpdateAllShadersValue);
            colorSliders[1].onValueChanged.AddListener(partData.mirroredPart.UpdateAllShadersValue);
            colorSliders[2].onValueChanged.AddListener(partData.mirroredPart.UpdateAllShadersValue);
        }

    }

}
