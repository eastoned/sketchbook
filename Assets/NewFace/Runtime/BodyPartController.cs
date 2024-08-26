using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Controls the state of each face part between physics-based or face-based.
/// </summary>
public class BodyPartController : PartController
{

    public PartData pd;

    public bool translatable, rotatable, scalable;
    public bool lockedYaxis = false;

    public BoxCollider2D colid;
    public Rigidbody2D rb2D;
    public SpringJoint2D sj2D;
    
    public bool flippedXAxis = false;
    public bool detached = false;
    public BodyPartController mirroredPart;

    public Vector3 cachePosition, cacheScale;
    public float cacheAngle;
    public ShaderCache[] shaderPropertyCache;
    PartTransformController ptc;
    PlayerActionData currentPAD;
    float timeCache;
    Vector3 positionCache, scaleCache;
    float angleCache;
    Coroutine shakeRotate;

    public BodyPartController connectablePart;

    public BodyPartController[] affectedChildren;
    public GameObject[] detailChildren;

    public Transform customScaleAnchor;

    public Vector3 scaleControllerPos = new Vector3(0.5f, 0.5f, 0);
    public Vector3 rotateControllerPos = new Vector3(0.5f, 0, 0);

    public List<ShaderProperty> shaderProperties;
    public Dictionary<string, ShaderProperty> shadePropertyDict = new Dictionary<string, ShaderProperty>();

    public override void InitializePropertyBlock()
    {
        base.InitializePropertyBlock();
        
        for(int i = 0; i < shaderProperties.Count; i++)
        {
            propBlock.SetFloat(shaderProperties[i].propertyName, shaderProperties[i].propertyValue);
        }
    }

    private void Start()
    {
        transform.name = transform.name + Random.Range(0, 20);
    }

    public void InitializePartDataDictionary()
    {
        if(!flippedXAxis)
        {
            //dummy was clearing the dictionary before and not populating it after smh
            shadePropertyDict.Clear();
            for(int i = 0; i < shaderProperties.Count; i++)
            {
                if(!shadePropertyDict.ContainsKey(shaderProperties[i].propertyName))
                {
                    shadePropertyDict.Add(shaderProperties[i].propertyName, shaderProperties[i]);
                }
            }
        }
    }

    public void UpdateDependencies()
    {  
        //Debug.Log("updating affected parts");
        foreach(BodyPartController pc in affectedChildren)
        {
            if(!pc.detached)
            {
                //pc.pd.SetPositionBounds(pd);
                //pc.pd.SetScaleBounds(pd);
                pc.UpdateAllTransformValues();
                pc.UpdateDependencies();
            }
        }

        if(mirroredPart != null)
        {
            if(!mirroredPart.detached)
                mirroredPart.UpdateAllTransformValues();
        }
    }

    public void SetCache(PartData pd)
    {
        shaderPropertyCache = new ShaderCache[shaderProperties.Count];
        for(int i = 0; i < shaderPropertyCache.Length; i++)
        {
            shaderPropertyCache[i] = new ShaderCache(i, shaderProperties[i].propertyValue);
        }
    }

    void OnMouseOver()
    {
        if(CustomUtils.IsPointerOverUIObject())
            return;

        if(Input.GetMouseButton(0))
            return;

        OnHoveredNewFacePartEvent.Instance.Invoke(this);
    }

    public override void OnMouseDown()
    {
        OnMouseClickEvent.Instance.Invoke();
        
        if(CustomUtils.IsPointerOverUIObject())
            return;
        
        //Begin transform part
        currentPAD = new PlayerActionData(PlayerActionData.ActionType.TRANSFORMCHANGE, pd);
        
        timeCache = Time.time;
        PartClicked();
    }

    public void PartClicked()
    {
        if(customScaleAnchor != null)
        {
            positionCache = customScaleAnchor.position;
        }else{
            positionCache = transform.position;
        }
        
        OnSelectedNewFacePartEvent.Instance.Invoke(this);
        ptc = transform.gameObject.AddComponent<PartTransformController>();
        ptc.controls = PartTransformController.TransformController.TRANSLATE;
        ptc.partInEdit = this;
        
        rb2D.bodyType = RigidbodyType2D.Kinematic;
        
        if(!detached && mirroredPart != null)
        {
            if(!mirroredPart.detached)
                mirroredPart.UpdateAllShadersValue(1f);
        }
            
        SetCache(pd);
    }

    void OnMouseUp()
    {

        if(CustomUtils.IsPointerOverUIObject())
            return;

        PartUnclicked();
        currentPAD.timeStamp = Time.time;
        currentPAD.timeToChange = Time.time - timeCache;
        currentPAD.positionChange = transform.position - positionCache;
        OnConfirmTransformPart.Instance.Invoke(currentPAD);
    }

    public void PartUnclicked()
    {
        if(ptc != null){
            Destroy(ptc);
        }

        ReleasePart();
    }

    public void ReleasePart()
    {
        rb2D.Sleep();
        rb2D.WakeUp();
        if(sj2D != null)
            rb2D.bodyType = RigidbodyType2D.Dynamic;
        if(detached)
        {
            rb2D.Sleep();
            rb2D.WakeUp();

            if(connectablePart != null)
            {
                if(!connectablePart.detached && connectablePart.transform.GetComponent<BoxCollider2D>().OverlapPoint(transform.position)){
                    UpdateAttachmentStatus(false);
                    UpdateAllTransformValues();
                }else{
                    if(sj2D != null)
                        rb2D.bodyType = RigidbodyType2D.Dynamic;   
                }
            }
            else
            {
                if(Vector3.Distance(transform.position, new Vector3(0, -1, transform.position.z)) < 0.1f){
                    UpdateAttachmentStatus(false);
                }else{
                    if(sj2D != null)
                        rb2D.bodyType = RigidbodyType2D.Dynamic;   
                }
            }
        }
        //ccc.DeleteCharacterHead(this.gameObject);
    }

    public void UpdateAllTransformValues()
    {
        if(!detached)
        {
            //if(translatable) UpdatePosition();

            //if(scalable) UpdateScale();

            //if(rotatable) UpdateRotation();
        }
    }

    public void UpdateScale()
    {
        if(flippedXAxis)
            {
                if(customScaleAnchor != null)
                {
                    customScaleAnchor.localScale = pd.GetFlippedAbsScale();
                    cacheScale = pd.GetFlippedAbsScale();
                }else{
                    transform.localScale = pd.GetFlippedAbsScale();
                    cacheScale = pd.GetFlippedAbsScale();
                }
                
            }else{
                if(customScaleAnchor != null)
                {
                    customScaleAnchor.localScale = pd.GetAbsScale();
                    cacheScale = pd.GetAbsScale();
                }else{
                    transform.localScale = pd.GetAbsScale();
                    cacheScale = pd.GetAbsScale();
                }
                
            }
        //pd.SetPositionBounds();
    }

    public void UpdatePosition()
    {
        if(flippedXAxis)
            {
                if(customScaleAnchor != null)
                {
                    customScaleAnchor.localPosition = pd.GetFlippedAbsPosition();
                    cachePosition = pd.GetFlippedAbsPosition();
                }else{
                    transform.localPosition = pd.GetFlippedAbsPosition();
                    cachePosition = pd.GetFlippedAbsPosition();
                }
            }else{
                if(customScaleAnchor != null)
                {
                    customScaleAnchor.localPosition = pd.GetAbsPosition();
                    cachePosition = pd.GetAbsPosition();
                }else{
                    transform.localPosition = pd.GetAbsPosition();
                    cachePosition = pd.GetAbsPosition();
                }
            }
    }

    public void UpdateRotation()
    {   
        if(flippedXAxis)
            {
                if(customScaleAnchor != null)
                {
                    customScaleAnchor.localRotation = Quaternion.Euler(0, 0, -pd.relativeToParentAngle);
                    cacheAngle = -pd.relativeToParentAngle;
                }else{
                    transform.localRotation = Quaternion.Euler(0, 0, -pd.relativeToParentAngle);
                    cacheAngle = -pd.relativeToParentAngle;
                }
            }else{
                if(customScaleAnchor != null)
                {
                    customScaleAnchor.localRotation = Quaternion.Euler(0, 0, pd.relativeToParentAngle);
                    cacheAngle = pd.relativeToParentAngle;
                }else{
                    transform.localRotation = Quaternion.Euler(0, 0, pd.relativeToParentAngle);
                    cacheAngle = pd.relativeToParentAngle;
                }
        }
    }

    public void UpdateColliderBounds()
    {
        colid.size = pd.GetColliderSize();
        if(customScaleAnchor != null){
            colid.offset = new Vector2(transform.GetChild(0).transform.localPosition.x, transform.GetChild(0).transform.localPosition.y);
        }else{
            colid.offset = pd.GetColliderOffset();
        }
        
    }

    public void AddHoveredMaterial(Material mat)
    {
        rend.sharedMaterials = new Material[2]{rend.sharedMaterials[0], mat};
    }

    public void ResetMaterial()
    {
        rend.sharedMaterials = new Material[1]{rend.sharedMaterials[0]};
    }

    public void UpdateAllShadersValue(float ignore)
    {

        for(int i = 0; i < shaderProperties.Count; i++){
            UpdateSingleShaderFloat(shaderProperties[i].propertyName, shaderProperties[i].propertyValue);
        }

        for(int j = 0; j < shaderColors.Count; j++){
            UpdateSingleShaderColor(shaderColors[j].colorName, shaderColors[j].colorValue);
        }
        rend.SetPropertyBlock(propBlock);

        if(colid != null && shadePropertyDict.Count > 0){
            UpdateColliderBounds();
            UpdateDependencies();
        }
    }

    public void ShakePiece(float strength, float time)
    {
        if(shakeRotate != null){
            StopCoroutine(shakeRotate);
        }
        shakeRotate = StartCoroutine(ShakeRotationRoutineTimed(strength, time));
    }

    public void ShakePieces(Vector3 strength, float time)
    {
        ShakePositionRoutineTimed(strength, time);
    }

    public void CopyData(BodyPartController bpcToCopyFrom)
    {
        if(propBlock != null)
        {
            for(int i = 0; i < shaderProperties.Count; i++)
            {
                shaderProperties[i].propertyValue = bpcToCopyFrom.shaderProperties[i].propertyValue;
                UpdateSingleShaderFloat(shaderProperties[i].propertyName, shaderProperties[i].propertyValue);
            }
            
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
            CopyData(bpcToCopyFrom);
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
    
    public override void RandomizeData()
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

    public IEnumerator ShakePositionRoutineTimed(Vector3 strength, float length)
    {
        float time = length;
        while(time > 0){
            time -= Time.deltaTime;
            transform.localPosition = cachePosition + Vector3.Scale(Random.insideUnitSphere, strength);
            yield return null;
        }
        transform.localPosition = cachePosition;
    }

    public IEnumerator ShakeRotationRoutineTimed(float strength, float length)
    {
        float time = length;
        while(time > 0){
            time -= Time.deltaTime;
            if(flippedXAxis){
                transform.localRotation = Quaternion.Euler(0, 0, -cacheAngle + Random.Range(-strength, strength));
            }else{
                transform.localRotation = Quaternion.Euler(0, 0, cacheAngle + Random.Range(-strength, strength));
            }
            yield return null;
        }
        transform.localRotation = Quaternion.Euler(0, 0, cacheAngle);
    }

    public IEnumerator ScalePopRoutine(float size, float length, AnimationCurve curve)
    {
        float time = length;
        while(time > 0){
            time -= Time.deltaTime;
            float perc = Mathf.Clamp01(1f-(time/length));
            float scl = size * curve.Evaluate(perc);
            transform.localScale = cacheScale + (Vector3.one*scl);//(size*curve.Evaluate(perc));
            yield return null;
        }
        transform.localScale = cacheScale;
    }

    public void UpdateAttachmentStatus(bool detach)
    {
        detached = detach;

        if(detached){
            PlayerActionData padBreak = new PlayerActionData(CharacterActionData.ActionType.BREAKCHANGE, pd);
            OnBreakPart.Instance.Invoke(padBreak);
            OnTriggerAudioOneShot.Instance.Invoke("Detach");
            transform.gameObject.layer = 11;
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            rb2D.AddForce(Random.insideUnitCircle * 2f, ForceMode2D.Impulse);
            if(detailChildren.Length > 0)
            {
                foreach(GameObject go in detailChildren)
                {
                    go.SetActive(false);
                }
            }
        }else{
            connectablePart.UpdateAllTransformValues();
            OnTriggerAudioOneShot.Instance.Invoke("Attach");
            transform.gameObject.layer = 12;
            //rb2D.bodyType = RigidbodyType2D.Kinematic;
            if(detailChildren.Length > 0)
            {
                foreach(GameObject go in detailChildren)
                {
                    go.SetActive(true);
                }
            }
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
            Debug.Log("Prop Block not initialized for: " + transform.name);
        }
    }

    public void UpdateSingleShaderFloatUnsafe(string param, float value)
    {
        propBlock.SetFloat(param, value);
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

    public void UpdateSingleShaderVector(string param, Vector3 vec)
    {
        propBlock.SetVector(param, vec);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {

        //Debug.Log(col.transform.name + " has a velocity of: " + col.rigidbody.velocity);
        //Debug.Log(transform.name + " has a velocity of: " + rb2D.velocity);
        
        if(Mathf.Abs(col.otherRigidbody.velocity.x) > .8f)
        {
            OnCharacterCollisionEvent.Instance.Invoke();
        }
    }
}
