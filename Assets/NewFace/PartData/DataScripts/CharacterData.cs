using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "ScriptableObjects/Character", order = 100)]
public class CharacterData : ScriptableObject
{
    public string name;
    public bool writeable = false;

    public PartData earData, eyebrowData, eyeData, hairBackData, hairFrontData, headData, mouthData, neckData, noseData;

    [ContextMenu("Random A Piece")]
    void RandomPiece(PartData part){
        part.currentAngle = Random.Range(part.minAngle, part.maxAngle);
        if(part == hairBackData || part == hairFrontData){
            part.currentAngle = Random.Range(0f, 1f) < 0.5f? 0 : part.maxAngle;
        }
        part.absolutePosition = new Vector3(
            Random.Range(part.minPosX, part.maxPosX),
            Random.Range(part.minPosY, part.maxPosY),
            part.absolutePosition.z);
        
        part.SetRelativePos(part.absolutePosition);

        part.absoluteScale = new Vector3(
            Random.Range(part.minScaleX, part.maxScaleX),
            Random.Range(part.minScaleY, part.maxScaleY),
            part.absoluteScale.z);

        part.SetRelativeScale(part.absoluteScale);


        foreach(ShaderProperty sp in part.shaderProperties){
            sp.SetValue(Random.Range(0f, 1f));
        }
        foreach(ShaderColor sc in part.shaderColors){
            sc.SetValue(Random.Range(0f, 1f));
            sc.SetHue(Random.Range(0f, 1f));
            sc.SetSaturation(Random.Range(0f, 1f));
        }
    }

    public void RandomizeData(){
        RandomPiece(earData);
        RandomPiece(eyebrowData);
        RandomPiece(eyeData);
        RandomPiece(hairBackData);
        RandomPiece(hairFrontData);
        RandomPiece(headData);
        RandomPiece(mouthData);
        RandomPiece(neckData);
        RandomPiece(noseData);
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
