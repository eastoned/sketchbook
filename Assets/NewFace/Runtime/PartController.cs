using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartController : MonoBehaviour
{
    public Renderer rend;
    public MaterialPropertyBlock propBlock;
    public List<ShaderProperty> shaderProperties;
    public List<ShaderColor> shaderColors;

    public Material highlightedMaterial;

    public enum MousedState
    {
        NONE,
        HOVERED,
        SELECTED
    }

    public MousedState currentMousedState;

    public void OnMouseEnter()
    {
        if(CustomUtils.IsPointerOverUIObject())
            return;

        if(Input.GetMouseButton(0))
            return;

        OnHoveredNewPartEvent.Instance.Invoke(this);
    }
    
    public virtual void OnMouseDown()
    {
        OnMouseClickEvent.Instance.Invoke();
        
        if(CustomUtils.IsPointerOverUIObject())
            return;
            
        OnSelectedNewPartEvent.Instance.Invoke(this);
    }

    public void InitializePropertyBlock()
    {
        propBlock = new MaterialPropertyBlock();

        if(shaderProperties.Count > 0)
        {
            for(int i = 0; i < shaderProperties.Count; i++)
            {
                propBlock.SetFloat(shaderProperties[i].propertyName, shaderProperties[i].propertyValue);
            }
        }
        
        if(shaderColors.Count > 0)
        {
            for(int j = 0;  j < shaderColors.Count; j++)
            {
                propBlock.SetColor(shaderColors[j].colorName, shaderColors[j].colorValue);
            }
        }
    }

    public void UpdateRenderPropBlock()
    {
        if(rend == null)
            return;

        rend.SetPropertyBlock(propBlock);
        SetColliderSize();
    }

    public void RandomizeData()
    {
        if(propBlock != null)
        {
            for(int i = 0; i < shaderProperties.Count; i++)
            {
                shaderProperties[i].propertyValue = Random.Range(0f, 1f);
                UpdateSingleShaderFloat(shaderProperties[i].propertyName, shaderProperties[i].propertyValue);
            }
            
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

    public void CopyAllData(PartController pcToCopyFrom)
    {
        if(propBlock != null)
        {
            for(int i = 0; i < shaderProperties.Count; i++)
            {
                shaderProperties[i].propertyValue = pcToCopyFrom.shaderProperties[i].propertyValue;
                UpdateSingleShaderFloat(shaderProperties[i].propertyName, shaderProperties[i].propertyValue);
            }
            
            for(int j = 0; j < shaderColors.Count; j++)
            {
                shaderColors[j].colorValue = pcToCopyFrom.shaderColors[j].colorValue;
                UpdateSingleShaderColor(shaderColors[j].colorName, shaderColors[j].colorValue);
            }
            UpdateRenderPropBlock();
        }
        else
        {
            //no prop block so the part needs to have it initialized before attempting to copy the data again
            InitializePropertyBlock();
            CopyAllData(pcToCopyFrom);
        }
    }

    public void CopyColors(BodyPartController bpcToCopyFrom)
    {
        if(propBlock != null)
        {
            for(int j = 0; j < shaderColors.Count; j++)
            {
                shaderColors[j].colorValue = bpcToCopyFrom.shaderColors[j].colorValue;
                UpdateSingleShaderColor(shaderColors[j].colorName, shaderColors[j].colorValue);
            }
            UpdateRenderPropBlock();
        }
        else
        {
            InitializePropertyBlock();
            CopyColors(bpcToCopyFrom);
        }
    }
    
    public void UpdateSingleShaderFloat(string param, float value)
    {
        if(propBlock != null)
        {
            if(propBlock.HasFloat(param))
            {
                propBlock.SetFloat(param, value);
            }
            else
            {
                Debug.Log(param + " is not an available float.");
            } 
        }
        else
        {
            InitializePropertyBlock();
            UpdateSingleShaderFloat(param, value);
            Debug.Log("Prop Block not initialized for: " + transform.name);
        }
    }
    public void UpdateAllShadersValue(float ignore)
    {
        UpdateAllShadersValue();

        //if(colid != null && shadePropertyDict.Count > 0){
        //    UpdateColliderBounds();
        //    UpdateDependencies();
        //}
    }

    public void UpdateAllShadersValue()
    {
        if(shaderProperties.Count > 0)
        {
            for(int i = 0; i < shaderProperties.Count; i++)
            {
                UpdateSingleShaderFloat(shaderProperties[i].propertyName, shaderProperties[i].propertyValue);
            }
        }

        if(shaderColors.Count > 0)
        {
            for(int j = 0; j < shaderColors.Count; j++)
            {
                UpdateSingleShaderColor(shaderColors[j].colorName, shaderColors[j].colorValue);
            }
        }

        UpdateRenderPropBlock();
    }

    public void UpdateSingleShaderFloatUnsafe(string param, float value)
    {
        propBlock.SetFloat(param, value);
    }

    public void UpdateSingleShaderVector(string param, Vector3 vec)
    {
        propBlock.SetVector(param, vec);
    }

    public float GetSingleShaderFloat(string param)
    {
        float defaultValue = 0f;
        if(propBlock != null)
        {
            if(propBlock.HasFloat(param))
            {
                defaultValue = propBlock.GetFloat(param);
            }
            else
            {
                Debug.Log(param + " is not an available float. Returning default: 0");
            } 
        }
        else
        {
            Debug.Log("Prop Block not initialized. Returning default: 0");
        }
        
        return defaultValue;
    }

    public void UpdatePartOutline(MousedState mouseState)
    {
        switch(mouseState)
        {
            case MousedState.NONE:
            propBlock.SetFloat("_Dashed", 0f);
            UpdateRenderPropBlock();
            if(rend == null)
                return;
            rend.sharedMaterials = new Material[1]{rend.sharedMaterials[0]};
            break;
            case MousedState.HOVERED:
            propBlock.SetFloat("_Dashed", 0.5f);
            UpdateRenderPropBlock();
            if(rend == null)
                return;
            rend.sharedMaterials = new Material[2]{rend.sharedMaterials[0], highlightedMaterial};
            break;
            case MousedState.SELECTED:
            propBlock.SetFloat("_Dashed", 0f);
            UpdateRenderPropBlock();
            if(rend == null)
                return;
            rend.sharedMaterials = new Material[2]{rend.sharedMaterials[0], highlightedMaterial};
            break;
        }
        
        currentMousedState = mouseState;
    }

    public virtual void SetColliderSize()
    {

    }
}