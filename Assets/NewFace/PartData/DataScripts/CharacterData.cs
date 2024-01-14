using System;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "ScriptableObjects/Character", order = 100)]
public class CharacterData : ScriptableObject
{
    public string name;
    public bool writeable = false;

    public PartData earData, eyebrowData, eyeData, hairBackData, hairFrontData, headData, mouthData, neckData, noseData;

    
    public PartData[] allParts;

    [ContextMenu("Random A Piece")]
    void RandomPiece(PartData part){
        part.currentAngle = UnityEngine.Random.Range(part.minAngle, part.maxAngle);
        if(part == hairBackData || part == hairFrontData){
            part.currentAngle = UnityEngine.Random.Range(0f, 1f) < 0.5f? 0 : part.maxAngle;
        }
        

        part.absolutePosition = new Vector3(
            UnityEngine.Random.Range(part.minPosX, part.maxPosX),
            UnityEngine.Random.Range(part.minPosY, part.maxPosY),
            part.absolutePosition.z);

        if(part == eyebrowData){
            part.absolutePosition = eyeData.absolutePosition + new Vector3(0, UnityEngine.Random.Range(.1f, 1f), 0);
        }

        part.SetRelativePos(part.absolutePosition);

        part.absoluteScale = new Vector3(
            UnityEngine.Random.Range(part.minScaleX, part.maxScaleX),
            UnityEngine.Random.Range(part.minScaleY, part.maxScaleY),
            part.absoluteScale.z);

        part.SetRelativeScale(part.absoluteScale);


        foreach(ShaderProperty sp in part.shaderProperties){
            sp.SetValue(UnityEngine.Random.Range(0f, 1f));
        }
        foreach(ShaderColor sc in part.shaderColors){
            sc.SetValue(UnityEngine.Random.Range(0f, 1f));
            sc.SetHue(UnityEngine.Random.Range(0f, 1f));
            sc.SetSaturation(UnityEngine.Random.Range(0f, 1f));
        }

        if(part.affectedPartData.Count > 0){
            foreach(PartData pd in part.affectedPartData){
                pd.SetPositionBounds(part);
                pd.SetScaleBounds(part);
            }
        }
    }
    public void RandomizeRandomPart(){
        RandomPiece(allParts[UnityEngine.Random.Range(0, allParts.Length)]);
    }

    public void RandomizeData(){
        RandomPiece(headData);
        RandomPiece(earData);
        RandomPiece(eyeData);
        RandomPiece(eyebrowData);
        RandomPiece(hairBackData);
        RandomPiece(hairFrontData);
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
