using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "ScriptableObjects/Character", order = 100)]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public bool writeable = false;

    public PartData earData, eyebrowData, eyeData, hairBackData, hairFrontData, headData, mouthData, neckData, noseData;

    public PartData[] allParts;

    public enum Expression
    {
        NEUTRAL,
        HAPPY,
        SAD,
        ANGRY,
        SURPRISE,
        SCARED
    }

    public enum CharacterState
    {
        SLEEPING,
        TIRED,
        CRYING
    }
    
    public Expression currentExpression;

    public bool canSee = true;
    public bool canSpeak = true;
    public bool canSmell = true;
    public bool canHear = true;

    public void UpdateVisionStatus()
    {
        float eyeRadius = eyeData.shadePropertyDict["_PupilRadius"].propertyValue;
        float eyeOpen = eyeData.shadePropertyDict["_EyelidBottomOpen"].propertyValue + eyeData.shadePropertyDict["_EyelidTopOpen"].propertyValue;
        canSee = eyeRadius > 0.05f && eyeOpen > 0.05f;
    }

    public bool CanSee()
    {
        if(eyeData.activeInScene){
           float eyeRadius = eyeData.shadePropertyDict["_PupilRadius"].propertyValue;
            float eyeOpen = eyeData.shadePropertyDict["_EyelidBottomOpen"].propertyValue + eyeData.shadePropertyDict["_EyelidTopOpen"].propertyValue;

            canSee =  eyeRadius > 0.05f && eyeOpen > 0.05f;
            return canSee; 
        }else{
            return false;
        }
        
    }

    public void UpdateHearingStatus(PartController ear)
    {
        canHear = true;
    }

    [ContextMenu("Random A Piece")]
    void RandomPiece(PartData part, float randomFactor){
        part.relativeToParentAngle = Mathf.Lerp(part.minAngle, part.maxAngle, UnityEngine.Random.Range(0.5f - randomFactor, 0.5f + randomFactor));

        if(part == hairBackData || part == hairFrontData){
            part.relativeToParentAngle = Mathf.Lerp(0, part.maxAngle, UnityEngine.Random.Range(0f, 1f));
        }
        
        if(part == noseData){
            part.maxPosY = eyeData.relativeToParentPosition.y;
        }

        if(part == mouthData){
            part.maxPosY = noseData.relativeToParentPosition.y;
        }

        

        part.relativeToParentPosition = new Vector3(
            Mathf.Lerp(part.minPosX, part.maxPosX, UnityEngine.Random.Range(0.5f - randomFactor, 0.5f + randomFactor)),
            Mathf.Lerp(part.minPosY, part.maxPosY, UnityEngine.Random.Range(0.5f - randomFactor, 0.5f + randomFactor)),
            part.absoluteWorldPositionZ);
            
        if(part == headData){
            Debug.Log("Randomizing head");
            Debug.Log(part.minPosY);
        }
        part.relativeToParentScale = new Vector3(
            Mathf.Lerp(part.minScaleX, part.maxScaleX, UnityEngine.Random.Range(0.5f - randomFactor, 0.5f + randomFactor)),
            Mathf.Lerp(part.minScaleY, part.maxScaleY, UnityEngine.Random.Range(0.5f - randomFactor, 0.5f + randomFactor)),
            1f);

        if(part == eyebrowData){
            part.minPosY = eyeData.relativeToParentPosition.y;
        }
        
        part.SetClampedPosition(part.relativeToParentPosition);
        part.SetRelativeScale(part.relativeToParentScale);

        foreach(ShaderProperty sp in part.shaderProperties){
            float val = UnityEngine.Random.Range(0.5f - randomFactor, 0.5f + randomFactor);
            sp.SetValue(val);
        }
        
        foreach(ShaderColor sc in part.shaderColors){
            sc.SetValue(UnityEngine.Random.Range(0f, 1f));
            sc.SetHue(UnityEngine.Random.Range(0f, 1f));
            sc.SetSaturation(UnityEngine.Random.Range(0f, 1f));
        }
    }
    public void RandomizeRandomPart(){
        RandomPiece(allParts[UnityEngine.Random.Range(0, allParts.Length)], .1f);
    }

    public void RandomizeData(float randomFactor){
        RandomPiece(headData, randomFactor);
        RandomPiece(earData, randomFactor);
        RandomPiece(eyeData, randomFactor);
        RandomPiece(eyebrowData, randomFactor);
        RandomPiece(hairBackData, randomFactor);
        RandomPiece(hairFrontData, randomFactor);
        RandomPiece(mouthData, randomFactor);
        RandomPiece(neckData, randomFactor);
        RandomPiece(noseData, randomFactor);
    }

    public void CopyData(CharacterData cd){
        earData.CopyData(cd.earData);
        eyebrowData.CopyData(cd.eyebrowData);
        eyeData.CopyData(cd.eyeData);
        hairBackData.CopyData(cd.hairBackData);
        hairFrontData.CopyData(cd.hairFrontData);
        headData.CopyData(cd.headData);
        mouthData.CopyData(cd.mouthData);
        neckData.CopyData(cd.neckData);
        noseData.CopyData(cd.noseData);
    }
    
}
