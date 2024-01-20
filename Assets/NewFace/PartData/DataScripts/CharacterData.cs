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
        //part.currentAngle = UnityEngine.Random.Range(part.minAngle, part.maxAngle);
        part.currentAngle = 0f;
        if(part == hairBackData || part == hairFrontData){
            part.currentAngle = UnityEngine.Random.Range(0f, 1f) < 0.5f? 0 : part.maxAngle;
        }
        
        if(part == noseData){
            part.maxPosY = eyeData.absolutePosition.y;
        }
        if(part == mouthData){
            part.maxPosY = noseData.absolutePosition.y;
        }

        part.absolutePosition = new Vector3(Mathf.Lerp(part.minPosX, part.maxPosX, UnityEngine.Random.Range(0f, 1f) < 0.5f? 0.25f : .75f),
        Mathf.Lerp(part.minPosY, part.maxPosY, UnityEngine.Random.Range(0f, 1f) < 0.5f? 0.25f : .75f),
        part.absolutePosition.z);

        if(part == eyebrowData){
            part.absolutePosition = new Vector3(eyeData.absolutePosition.x, eyeData.absolutePosition.y + UnityEngine.Random.Range(.1f, 1f), part.absolutePosition.z);
        }
        
        part.SetRelativePos(part.absolutePosition);

        part.absoluteScale = new Vector3(Mathf.Lerp(part.minScaleX, part.maxScaleX, UnityEngine.Random.Range(0f, 1f) < 0.5f? 0.25f : .75f),
        Mathf.Lerp(part.minScaleY, part.maxScaleY, UnityEngine.Random.Range(0f, 1f) < 0.5f? 0.25f : .75f),
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
