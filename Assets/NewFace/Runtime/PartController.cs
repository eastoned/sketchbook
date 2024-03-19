using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Controls the state of each face part between physics-based or face-based.
/// </summary>
public class PartController : MonoBehaviour
{

    public Renderer rend;
    private Material currentMat;

    public PartData pd;

    public bool translatable, rotatable, scalable;

    public BoxCollider2D colid;
    public Rigidbody2D rb2D;
    
    public bool flippedXAxis = false;
    public bool detached = false;
    public bool overAttachmentNode = false;

    public List<PartController> childControllers = new List<PartController>();
    public PartController parentController;
    public PartController mirroredPart; 

    MaterialPropertyBlock propBlock;

    public Vector3 cachePosition, cacheScale;
    public float cacheAngle;
    public ShaderCache[] shaderPropertyCache;
    PartTransformController ptc;
    PlayerActionData currentPAD;
    float timeCache;
    Vector3 positionCache, scaleCache;
    float angleCache;
    Coroutine shakeRotate;
    public PartController parent;
    void Awake()
    {
        propBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        currentMat = rend.sharedMaterial;

    }

    public void InitializePartDataDictionary()
    {
        pd.shadePropertyDict.Clear();
        if(!flippedXAxis){
            for(int i = 0; i < pd.shaderProperties.Count; i++){
                if(!pd.shadePropertyDict.ContainsKey(pd.shaderProperties[i].propertyName)){
                    pd.shadePropertyDict.Add(pd.shaderProperties[i].propertyName, pd.shaderProperties[i]);
                }
            }
        }
    }

    public void UpdateDependencies()
    {
        if(childControllers.Count > 0){
            for(int j = 0; j < childControllers.Count; j++){
                if(!childControllers[j].detached){
                    childControllers[j].pd.SetPositionBounds(pd);
                    childControllers[j].pd.SetScaleBounds(pd);
                    childControllers[j].UpdateAllTransformValues();
                }
            }
        }
    }

    public void SetCache(PartData pd)
    {
        shaderPropertyCache = new ShaderCache[pd.shaderProperties.Count];
        for(int i = 0; i < shaderPropertyCache.Length; i++){
            shaderPropertyCache[i] = new ShaderCache(i, pd.shaderProperties[i].propertyValue);
        }
    }

    void OnMouseEnter()
    {
        if(CustomUtils.IsPointerOverUIObject())
            return;

        if(Input.GetMouseButton(0))
            return;

        OnHoveredNewFacePartEvent.Instance.Invoke(transform);
    }

    void OnMouseDown()
    {
        OnMouseClickEvent.Instance.Invoke();
        
        if(CustomUtils.IsPointerOverUIObject())
            return;

        
        //Begin transform part
        currentPAD = new PlayerActionData(pd, PlayerActionData.ActionType.TRANSFORMCHANGE);
        
        timeCache = Time.time;
        positionCache = transform.position;
        //scaleCache = transform.localScale;
        //angleCache = transform.localEulerAngles.z;
        
        
        OnSelectedNewFacePartEvent.Instance.Invoke(transform);
        ptc = transform.gameObject.AddComponent<PartTransformController>();
        ptc.controls = PartTransformController.TransformController.TRANSLATE;
        
        rb2D.bodyType = RigidbodyType2D.Kinematic;
        
        SetCache(pd);
    }
    void OnValidate(){
    }

    void OnMouseUp()
    {

        if(CustomUtils.IsPointerOverUIObject())
            return;

        if(ptc != null){
            Destroy(ptc);
        }
        if(detached){
            rb2D.Sleep();
            rb2D.bodyType = RigidbodyType2D.Dynamic;

            if(overAttachmentNode){
                //transform.position = new Vector3(attachPosition.x, attachPosition.y, transform.position.z);
                UpdateAttachmentStatus(false, parent);
                
            }
        }
        currentPAD.timeToChange = Time.time - timeCache;
        currentPAD.brokePart = detached;
        currentPAD.positionChange = transform.position - positionCache;
        OnConfirmTransformPart.Instance.Invoke(currentPAD);
    }

    public void UpdateAllTransformValues()
    {

        if(flippedXAxis){
            transform.localScale = pd.GetFlippedAbsoluteScale();
            cacheScale = pd.GetFlippedAbsoluteScale();
        }else{
            transform.localScale = pd.GetAbsoluteScale();
            cacheScale = pd.GetAbsoluteScale();
        }

        if(flippedXAxis){
            transform.localPosition = pd.GetFlippedAbsolutePosition();
            cachePosition = pd.GetFlippedAbsolutePosition();
        }else{
            transform.localPosition = pd.GetAbsolutePosition();
            cachePosition = pd.GetAbsolutePosition();
        }

        if(flippedXAxis){
            transform.localRotation = Quaternion.Euler(0, 0, -pd.currentAngle);
            cacheAngle = -pd.currentAngle;
        }else{
            transform.localRotation = Quaternion.Euler(0, 0, pd.currentAngle);
            cacheAngle = pd.currentAngle;
        }

        pd.SetPositionBounds();
        pd.SetScaleBounds();
        UpdateDependencies();
    }

    public void UpdateColliderBounds()
    {
        colid.size = pd.GetColliderSize();
        colid.offset = pd.GetColliderOffset();
    }

    public void UpdateAllShadersValue(float ignore)
    {

        //Debug.Log("updating shaders");

        for(int i = 0; i < pd.shaderProperties.Count; i++){
            UpdateSingleShaderFloat(pd.shaderProperties[i].propertyName, pd.shaderProperties[i].propertyValue);
        }

        for(int j = 0; j < pd.shaderColors.Count; j++){
            UpdateSingleShaderColor(pd.shaderColors[j].colorName, pd.shaderColors[j].colorValue);
        }

        rend.SetPropertyBlock(propBlock);

        if(colid != null && pd.shadePropertyDict.Count > 0){
            UpdateColliderBounds();
            UpdateDependencies();
        }
        
    }

    [ContextMenu("Shake Test")]
    public void ShakeTest()
    {
        ShakePieces(new Vector3(.1f, 0.01f, 0f), .5f);
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

    public void ScalePieces(float size, float time, AnimationCurve curve)
    {
        StartCoroutine(ScalePopRoutine(size, time, curve));
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

    public void UpdateAttachmentStatus(bool detach, PartController parent)
    {
        detached = detach;
        parentController = parent; 
        if(parentController != null){
            if(!parentController.childControllers.Contains(this))
                parentController.childControllers.Add(this);
        }

        if(detached){
            if(parentController != null){
                if(!parentController.childControllers.Contains(this))
                    parentController.childControllers.Remove(this);
            }
            PlayerActionData padBreak = new PlayerActionData(pd, CharacterActionData.ActionType.BREAKCHANGE);
            OnBreakPart.Instance.Invoke(padBreak);
            OnTriggerAudioOneShot.Instance.Invoke("Detach");
            transform.gameObject.layer = 11;
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            rb2D.AddForce(Random.insideUnitCircle * 2f, ForceMode2D.Impulse);
        }else{
            OnTriggerAudioOneShot.Instance.Invoke("Attach");
            transform.gameObject.layer = 12;
            rb2D.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public void UpdateAllShadersValue()
    {
        rend.SetPropertyBlock(propBlock);
    }

    public void UpdateRenderPropBlock()
    {
        rend.SetPropertyBlock(propBlock);
    }

    public void UpdateSingleShaderFloat(string param, float value)
    {
        propBlock.SetFloat(param, value);
    }

    public float GetSingleShaderFloat(string param)
    {
        return propBlock.GetFloat(param);
    }

    public void UpdateSingleShaderVector(string param, Vector3 vec)
    {
        propBlock.SetVector(param, vec);
    }

    void UpdateSingleShaderColor(string param, Color col)
    {
        propBlock.SetColor(param, col);
    }

}
