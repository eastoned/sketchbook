using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "ScriptableObjects/Character", order = 100)]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public bool writeable = false;

    public PartData earData, eyebrowData, eyeData, hairBackData, hairFrontData, headData, mouthData, neckData, noseData, handData;

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

    void RandomPiece(PartData part, float randomFactor){
        part.Randomize(randomFactor);
    }
    
    [ContextMenu("Random A Piece")]
    public void RandomizeRandomPart(){
        RandomPiece(allParts[UnityEngine.Random.Range(0, allParts.Length)], .1f);
    }

    public void RandomizeData(float randomFactor){
        RandomPiece(neckData, randomFactor);
        RandomPiece(headData, randomFactor);
        RandomPiece(earData, randomFactor);
        RandomPiece(eyeData, randomFactor);
        RandomPiece(eyebrowData, randomFactor);
        RandomPiece(hairBackData, randomFactor);
        RandomPiece(hairFrontData, randomFactor);
        RandomPiece(mouthData, randomFactor);
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
