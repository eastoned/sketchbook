using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public BoxCollider2D newPiece;

    public PartController connectablePart;

    public Transform customScaleAnchor;

    public Vector3 scaleControllerPos = new Vector3(0.5f, 0.5f, 0);
    public Vector3 rotateControllerPos = new Vector3(0.5f, 0, 0);

    public Dictionary<string, ShaderProperty> shadePropertyDict = new Dictionary<string, ShaderProperty>();

    public BodyPartController limb;

    public bool debugSpringForce;

    private void Start()
    {
        transform.name = transform.name + Random.Range(0, 20);
    }

    void OnJointBreak2D(Joint2D brokenJoint)
    {
        brokenJoint.gameObject.layer = 13;
        rb2D.gravityScale = 1;
        if(!lockedYaxis)
        {
            rb2D.drag = 0f;
            rb2D.angularDrag = 0f;
        }
        
        detached = true;

        OnCurrentJointBreak.Instance.Invoke();

        if(limb != null)
        {
            limb.rend.enabled = false;
            limb.colid.enabled = false;
        }
    }

    public void UpdateDependencies()
    {  
        /*
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
        }*/
    }

    public void SetCache(PartData pd)
    {
        shaderPropertyCache = new ShaderCache[shaderProperties.Count];
        for(int i = 0; i < shaderPropertyCache.Length; i++)
        {
            shaderPropertyCache[i] = new ShaderCache(i, shaderProperties[i].propertyValue);
        }
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();

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
        }
        else
        {
            positionCache = transform.position;
        }
        
        ptc = transform.gameObject.AddComponent<PartTransformController>();
        ptc.controls = PartTransformController.TransformController.TRANSLATE;
        ptc.UpdateActivePart(this);
        
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
        if(sj2D != null)
            rb2D.bodyType = RigidbodyType2D.Dynamic;

        if(detached)
        {
            
            if(connectablePart != null)
            {
                if(connectablePart.transform.GetComponent<BoxCollider2D>().OverlapPoint(transform.position))
                {
                    if(limb != null)
                    {
                        sj2D.connectedAnchor = transform.position;
                    }
                    else
                    {
                        sj2D.connectedAnchor = connectablePart.transform.InverseTransformPoint(transform.position);
                    }
                    OnCurrentJointRepair.Instance.Invoke(this);
                    sj2D.connectedAnchor = new Vector2(Mathf.Round(sj2D.connectedAnchor.x*10f)/10f, Mathf.Round(sj2D.connectedAnchor.y*10f)/10f);
                    UpdateAttachmentStatus(false);

                    //UpdateAllTransformValues();
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

    void Update()
    {
        if(limb != null)
        {
            limb.transform.localScale = new Vector3(limb.transform.localScale.x, transform.position.y + 2f, 1f);
            limb.UpdateSingleShaderFloatUnsafe("_HeadPosX", (transform.position.x - limb.transform.position.x)/limb.transform.localScale.x);
            limb.UpdateRenderPropBlock();
            sj2D.connectedAnchor = new Vector2(limb.transform.position.x, sj2D.connectedAnchor.y);
        }
        
        //if(newPiece.OverlapPoint(transform.position))
        //{
        //    RandomizeData();
        //    transform.position = new Vector3(0, 0, transform.position.z);
        //}
        
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

    public void ShakePiece(float strength, float time)
    {
        if(shakeRotate != null)
        {
            StopCoroutine(shakeRotate);
        }
        shakeRotate = StartCoroutine(ShakeRotationRoutineTimed(strength, time));
    }

    public void ShakePieces(Vector3 strength, float time)
    {
        ShakePositionRoutineTimed(strength, time);
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
        cacheAngle = transform.localEulerAngles.z;

        while(time > 0)
        {
            time -= Time.deltaTime;

            if(flippedXAxis)
            {
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

        if(detached)
        {
            //PlayerActionData padBreak = new PlayerActionData(CharacterActionData.ActionType.BREAKCHANGE, pd);
            //OnBreakPart.Instance.Invoke(padBreak);
            OnTriggerAudioOneShot.Instance.Invoke("Detach");
            rb2D.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            //connectablePart.UpdateAllTransformValues();
            OnTriggerAudioOneShot.Instance.Invoke("Attach");

            rb2D.gravityScale = 0f;
            rb2D.drag = 15f;
            rb2D.angularDrag = 5f;
            if(limb != null)
            {
                transform.gameObject.layer = 11;
                limb.transform.position = new Vector3(transform.position.x, limb.transform.position.y, limb.transform.position.z);
                limb.rend.enabled = true;
                limb.colid.enabled = true;
            }else{
                transform.gameObject.layer = 12;
            }
            
            sj2D.enabled = true;
            
        }
        
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
