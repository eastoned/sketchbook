using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{

    public PartController background;
    public BodyPartController leftEye, rightEye, mouth, nose, head, leftEyebrow, rightEyebrow, bangs, hair, neck, leftEar, rightEar, leftHand, leftArm, rightHand, rightArm;
    public BodyPartController[] partControllers;
    public Transform[] bodyParts;
    public PartData[] bodyData;

    public enum State
    {

    }

    public bool canRandomHeadPos, canRandomHeadScale, canRandomShaders = false;
    public float clampVal;
    public CharacterData currentChar;
    public AnimationCurve propertyCurve;
    public AnimationCurve blendCurve; 
    public AnimationCurve blinkCurve;
    public AnimationCurve scalePopCurve;

    public bool currentlyBlending = false;

    private Coroutine blending;


    [Range(0f, 1f)]
    public float mouthOverride = 1f;
    public float mouthOpen;
    public AnimationCurve headScale;
    public bool animating = false;

    private Coroutine movingRoutine;

    public virtual void OnEnable()
	{
        OnCharacterCollisionEvent.Instance.AddListener(UpdateAttentionTarget);
    }

    public virtual void OnDisable(){
        OnCharacterCollisionEvent.Instance.RemoveListener(UpdateAttentionTarget);
    }

    public void Start()
    {
        //InitializeControllers();
        RandomizePieces();
        RandomizeShaders();
    }

    public void RandomizePieces()
    {

        if(canRandomHeadScale)
            head.transform.localScale = new Vector2(headScale.Evaluate(Random.Range(0f, 1f)), headScale.Evaluate(Random.Range(0f, 1f)));
        
        if(canRandomHeadPos)
            head.sj2D.connectedAnchor = new Vector2(transform.position.x, Random.Range(-1f, 1.5f));

        leftHand.sj2D.connectedAnchor = new Vector2(transform.position.x + .75f, Random.Range(-1f, 1.5f));
        rightHand.sj2D.connectedAnchor = new Vector2(transform.position.x - .75f, leftHand.sj2D.connectedAnchor.y);
        
        leftEye.transform.localScale = new Vector3(Random.Range(0.25f, head.transform.localScale.x/2f), Random.Range(0.25f, head.transform.localScale.y/2f));

        rightEye.transform.localScale = leftEye.transform.localScale;
        nose.transform.localScale = new Vector2(Random.Range(0.1f, head.transform.localScale.x), Random.Range(0.1f, head.transform.localScale.y));

        RandomizeShaders();
        
        mouthOpen = mouth.GetSingleShaderFloat("_MouthOpen");
    }

    public void RandomizeShaders()
    {
        background.RandomizeData();
        bangs.RandomizeData();
        mouth.RandomizeData();
        neck.RandomizeData();
        head.RandomizeData();
        leftEye.RandomizeData();
        rightEye.CopyAllData(leftEye);
        leftHand.RandomizeData();
        rightHand.CopyAllData(leftHand);
        leftArm.RandomizeData();
        leftArm.CopyColors(neck);
        rightArm.CopyAllData(leftArm);
        leftEyebrow.RandomizeData();
        rightEyebrow.CopyAllData(leftEyebrow);
        nose.RandomizeData();
        hair.CopyAllData(bangs);
        leftEar.RandomizeData();
        rightEar.CopyAllData(leftEar);
    }

    private void InitializeControllers()
    {
        //InitializeDictionaries();
        UpdateAllControllers();
    }

    private void UpdateAttentionTarget()
    {
        /*if(bpc != head && bpc2 == head)
        {
            attentionTarget = bpc.gameObject.transform;
            eyeTarget = EyeTarget.PART;
            head.CopyData(bpc);
        }*/
        mouth.UpdateSingleShaderFloatUnsafe("_MouthBend", 1f);
        mouth.UpdateRenderPropBlock();
    }

    [ContextMenu("Refresh Connected Data")]
    public void RefreshDataConnection(){
        bodyData = new PartData[9];
        leftEye.pd = currentChar.eyeData;
        rightEye.pd = currentChar.eyeData;
        bodyData[5] = currentChar.eyeData;
        leftEyebrow.pd = currentChar.eyebrowData;
        rightEyebrow.pd = currentChar.eyebrowData;
        bodyData[6] = currentChar.eyebrowData;
        leftEar.pd = currentChar.earData;
        rightEar.pd = currentChar.earData;
        bodyData[4] = currentChar.earData;
        head.pd = currentChar.headData;
        bodyData[1] = currentChar.headData;
        neck.pd = currentChar.neckData;
        bodyData[0] = currentChar.neckData;
        nose.pd = currentChar.noseData;
        bodyData[7] = currentChar.noseData;
        mouth.pd = currentChar.mouthData;
        bodyData[8] = currentChar.mouthData;
        hair.pd = currentChar.hairBackData;
        bodyData[2] = currentChar.hairBackData;
        bangs.pd = currentChar.hairFrontData;
        bodyData[3] = currentChar.hairFrontData;
    }

    public void Interpolate(float val, CharacterData gameData, CharacterData blendFrom, CharacterData blendTo)
    {
        for(int i = 0; i < gameData.allParts.Length; i++)
        {
            if(gameData.allParts[i].activeInScene)
                BlendProfile(val, gameData.allParts[i], blendFrom.allParts[i], blendTo.allParts[i]);
        }
    }

    public void UpdateAllControllers()
    {
        foreach(BodyPartController pc in partControllers){
            if(pc.pd.activeInScene){
                pc.UpdateAllShadersValue(0f);
                if(!pc.detached){
                   pc.UpdateAllTransformValues(); 
                }
            }
        }
    }

    public void BlendProfile(float val, PartData partData, PartData blendFrom, PartData blendTo){
        
        //partData.absoluteWorldPosition = Vector3.Lerp(blendFrom.absoluteWorldPosition, blendTo.absoluteWorldPosition, val);
        partData.relativeToParentPosition = Vector3.Lerp(blendFrom.relativeToParentPosition, blendTo.relativeToParentPosition, val);
        partData.relativeToParentAngle = Mathf.Lerp(blendFrom.relativeToParentAngle, blendTo.relativeToParentAngle, val);
        //partData.absoluteWorldScale = Vector3.Lerp(blendFrom.absoluteWorldScale, blendTo.absoluteWorldScale, val);
        partData.relativeToParentScale = Vector3.Lerp(blendFrom.relativeToParentScale, blendTo.relativeToParentScale, val);

        for(int i = 0; i < partData.shaderProperties.Count; i++){
            partData.shaderProperties[i].SetValue(Mathf.Lerp(blendFrom.shaderProperties[i].propertyValue, blendTo.shaderProperties[i].propertyValue, val));
        }

        for(int j = 0; j < partData.shaderColors.Count; j++){
            partData.shaderColors[j].colorValue = Color.Lerp(blendFrom.shaderColors[j].colorValue, blendTo.shaderColors[j].colorValue, val);
        }
    }

    public void BlendBPC(float val, BodyPartController bpcFrom, BodyPartController bpcTo)
    {
        float[] shaderPropertyArray = new float[bpcFrom.shaderProperties.Count];
        Color[] shaderColorArray = new Color[bpcFrom.shaderColors.Count];

        for(int i = 0; i < bpcFrom.shaderProperties.Count; i++)
        {
            shaderPropertyArray[i] = Mathf.Lerp(bpcFrom.shaderProperties[i].propertyValue, bpcTo.shaderProperties[i].propertyValue, val);
        }

    }

    public void BlendCharacter(CharacterData char1, CharacterData char2, float animLength)
    {
        if(blending != null){
            StopCoroutine(blending);
        }

        blending = StartCoroutine(Blend(char1, char2, animLength));
    }

    public void SetCharacter(CharacterData characterToBe){
        currentChar.CopyData(characterToBe);
        UpdateAllControllers();
    }

    public Coroutine BlendCharacterSequence(CharacterData char1, CharacterData char2, float animLength){
        return StartCoroutine(Blend(char1, char2, animLength));
    }

    public Coroutine BlendPartSequence(CharacterData char1, CharacterData char2, int partID, float animLength){
        return StartCoroutine(BlendPart(char1, char2, partID, animLength));
    }

    public IEnumerator Blend(CharacterData cd1, CharacterData cd2, float value){
        float journey = 0;
        currentlyBlending = true;
        while(journey < value){
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey/value);
            float blendPercent = blendCurve.Evaluate(percent);
            Interpolate(blendPercent, currentChar, cd1, cd2);
            UpdateAllControllers();
            yield return null;
        }

        currentChar.CopyData(cd2);

        if(cd1.writeable){
            cd1.CopyData(cd2);
        }

        currentlyBlending = false;
    }

    public IEnumerator BlendPart(CharacterData char1, CharacterData char2, int partID, float animLength){
        float journey = 0;
        currentlyBlending = true;
        while(journey < animLength){
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey/animLength);
            float blendPercent = blendCurve.Evaluate(percent);
            BlendProfile(blendPercent, currentChar.allParts[partID], char1.allParts[partID], char2.allParts[partID]);
            UpdateAllControllers();
            yield return null;
        }
        
        if(char1.writeable){
            char1.allParts[partID].CopyData(char2.allParts[partID]);
        }
        currentlyBlending = false;
        Debug.Log("copied " + currentChar.allParts[partID].name + " data from blend target to writeable character");
    }

    private IEnumerator AnimatePartShaderProperty(BodyPartController pc, string shaderParam, float animationLength)
    {
        float animationTime = 0;
        float initialPropertyValue = pc.GetSingleShaderFloat(shaderParam);

        while(animationTime < animationLength)
        {
            float interval = Mathf.Clamp01(animationTime/animationLength);
            interval = propertyCurve.Evaluate(interval);
            pc.UpdateSingleShaderFloat(shaderParam, initialPropertyValue * interval);
            pc.UpdateRenderPropBlock();
            animationTime += Time.deltaTime;
            yield return null;
        }
    }

    public void DoRandomMovement()
    {
        if(movingRoutine != null)
        {
            StopCoroutine(movingRoutine);
        }
        int targetPart = Random.Range(0, partControllers.Length);
        Debug.Log("attempt to move hand to: " + partControllers[targetPart].name);
        Vector3 partTargetPos = partControllers[targetPart].transform.position;

        movingRoutine = StartCoroutine(MovePartToPosition(new Vector3(partTargetPos.x, partTargetPos.y, rightHand.transform.position.z)));
    }

    private IEnumerator MovePartToPosition(Vector3 handTargetPos)
    {
        Vector3 handPos = rightHand.transform.localPosition;
        Vector3 startPos = handPos;
        float counter = 0f;
        float animationTime = 1.4f;
        OnSelectedNewFacePartEvent.Instance.Invoke(rightHand);
        while(counter <= animationTime)
        {
            counter += Time.deltaTime;
            handPos = Vector3.Lerp(startPos, handTargetPos, Mathf.Clamp01(counter/animationTime));
            OnTranslatePartController.Instance.Invoke(rightHand, handPos, false);
            rightHand.UpdateSingleShaderFloatUnsafe("_Finger1",  Mathf.Sin((counter/animationTime) * Mathf.PI));
            rightHand.UpdateSingleShaderFloatUnsafe("_Finger2",  Mathf.Sin((counter/animationTime) * Mathf.PI));
            rightHand.UpdateSingleShaderFloatUnsafe("_Finger3",  Mathf.Sin((counter/animationTime) * Mathf.PI));
            rightHand.UpdateSingleShaderFloatUnsafe("_Finger4",  Mathf.Sin((counter/animationTime) * Mathf.PI));
            rightHand.UpdateRenderPropBlock();
            yield return null;
        }

        Debug.Log("Hand reached target");
        rightHand.ReleasePart();
        
        for(int i = 0; i < partControllers.Length; i++)
        {
            if(partControllers[i].transform.GetComponent<BoxCollider2D>().OverlapPoint(handTargetPos))
            {
                Debug.Log("successful click on: " + partControllers[i].name);
                Vector3 randomPos = new Vector3(
                    Random.Range(partControllers[i].transform.position.x-1f, partControllers[i].transform.position.x+1f),
                    Random.Range(partControllers[i].transform.position.y-2f, partControllers[i].transform.position.y+2f),
                    partControllers[i].transform.position.z);
                StartCoroutine(MovePiece(partControllers[i], handTargetPos, randomPos));
                StartCoroutine(MovePiece(rightHand, handTargetPos, new Vector3(randomPos.x, randomPos.y, rightHand.pd.absoluteWorldPositionZ)));
                break;
            }
        }
        
        yield return null;
    }

    private IEnumerator MovePiece(BodyPartController pc, Vector3 startPos, Vector3 endPos)
    {
        pc.PartClicked();
        Vector3 partPos = startPos;
        PartTransformController ptc = pc.GetComponent<PartTransformController>();
        ptc.OnHandDown(startPos);
        float counter = 0f;
        float animationTime = 1.4f;
        while(counter <= animationTime)
        {
            counter += Time.deltaTime;
            partPos = Vector3.Lerp(startPos, endPos, counter/animationTime);
            ptc.OnHandDrag(partPos);
            yield return null;
        }
        ptc.OnHandUp();
        pc.PartUnclicked();
        yield return null;
    }

}