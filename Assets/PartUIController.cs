using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Packages.Rider.Editor.UnitTesting;

public class PartUIController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI titleText;

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
    }
}
