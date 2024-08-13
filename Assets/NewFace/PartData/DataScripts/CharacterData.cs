using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "ScriptableObjects/Character", order = 100)]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public bool writeable = false;

    public PartData earData, eyebrowData, eyeData, hairBackData, hairFrontData, headData, mouthData, neckData, noseData, handData;

    public PartData[] allParts;

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
