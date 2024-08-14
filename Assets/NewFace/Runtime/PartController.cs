using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartController : MonoBehaviour
{

    public Renderer rend;
    public MaterialPropertyBlock propBlock;
    public List<ShaderColor> shaderColors;

    public virtual void OnMouseDown(){
        OnMouseClickEvent.Instance.Invoke();
        
        if(IsPointerOverUIObject())
            return;
            
        OnDeselectedFacePartEvent.Instance.Invoke();
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
     }

    void OnMouseEnter()
    {
        OnHoveredNewFacePartEvent.Instance.Invoke(null);
    }

    public virtual void InitializePropertyBlock()
    {
        propBlock = new MaterialPropertyBlock();

        for(int j = 0;  j < shaderColors.Count; j++)
        {
            propBlock.SetColor(shaderColors[j].colorName, shaderColors[j].colorValue);
        }
    }

    public void UpdateRenderPropBlock()
    {
        rend.SetPropertyBlock(propBlock);
    }

    public virtual void RandomizeData()
    {
        if(propBlock != null)
        {
            for(int j = 0; j < shaderColors.Count; j++)
            {
                shaderColors[j].colorValue = Random.ColorHSV();
                UpdateSingleShaderColor(shaderColors[j].colorName, shaderColors[j].colorValue);
            }
            UpdateRenderPropBlock();
        }
        else
        {
            InitializePropertyBlock();
            RandomizeData();
        }
    }

    protected void UpdateSingleShaderColor(string param, Color col)
    {
        if(propBlock != null)
        {
            if(propBlock.HasColor(param))
            {
            propBlock.SetColor(param, col); 
            }else{
                Debug.Log(param + " is not an available color.");
            }
        }
        else
        {
            Debug.Log("Prop Block not initialized");
        }
        
    }
}
