using System.Data;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "ScriptableObjects/Character", order = 100)]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public bool writeable = false;

    public PartData earData, eyebrowData, eyeData, hairBackData, hairFrontData, headData, mouthData, neckData, noseData;

    public PartData[] allParts;

    public FaceFeatureData featureData;

    [ContextMenu("Random A Piece")]
    void RandomPiece(PartData part, float randomFactor){
        part.relativeToParentAngle = Mathf.Lerp(part.minAngle, part.maxAngle, UnityEngine.Random.Range(0f, 1f) < 0.5f? 0.5f - randomFactor : .5f + randomFactor);

        if(part == hairBackData || part == hairFrontData){
            part.relativeToParentAngle = UnityEngine.Random.Range(0f, 1f) < 0.5f? 0 : part.maxAngle;
        }
        
        if(part == noseData){
            part.maxPosY = eyeData.relativeToParentPosition.y;
        }

        if(part == mouthData){
            part.maxPosY = noseData.relativeToParentPosition.y;
        }

        part.relativeToParentPosition = new Vector3(
            Mathf.Lerp(part.minPosX, part.maxPosX, UnityEngine.Random.Range(0f, 1f) < 0.5f? 0.5f - randomFactor : .5f + randomFactor),
            Mathf.Lerp(part.minPosY, part.maxPosY, UnityEngine.Random.Range(0f, 1f) < 0.5f? 0.5f - randomFactor : .5f + randomFactor),
            part.absoluteWorldPositionZ);
            
        part.relativeToParentScale = new Vector3(
            Mathf.Lerp(part.minScaleX, part.maxScaleX, UnityEngine.Random.Range(0f, 1f) < 0.5f? 0.5f - randomFactor : .5f + randomFactor),
            Mathf.Lerp(part.minScaleY, part.maxScaleY, UnityEngine.Random.Range(0f, 1f) < 0.5f? 0.5f - randomFactor : .5f + randomFactor),
            1f);

        if(part == eyebrowData){
            part.relativeToParentPosition = new Vector3(eyeData.relativeToParentPosition.x, eyeData.relativeToParentPosition.y + UnityEngine.Random.Range(.5f - randomFactor, .5f + randomFactor), part.absoluteWorldPositionZ);
        }
        
        part.SetClampedPosition(part.relativeToParentPosition);
        part.SetRelativeScale(part.relativeToParentScale);

        foreach(ShaderProperty sp in part.shaderProperties){
            float val = Random.Range(0f, 1f) < 0.5f? 0.5f - randomFactor : .5f + randomFactor;
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
