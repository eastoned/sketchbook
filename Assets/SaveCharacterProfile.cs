using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SaveCharacterProfile : MonoBehaviour
{
    public string characterName;
    public PartController ear, eyebrow, eye, hairBack, hairFront, head, mouth, neck, nose;

    public CharacterData currentChar;
    
    [ContextMenu("SaveCharacter")]
    public void SaveChara(){
        string guid = AssetDatabase.CreateFolder("Assets/NewFace/PartData/Characters", characterName);
        
        string newPath = AssetDatabase.GUIDToAssetPath(guid);
        CharacterData newScriptableObject = ScriptableObject.CreateInstance<CharacterData>();
        newScriptableObject.name = characterName;
        
        PartData earData = ScriptableObject.CreateInstance<PartData>();
        earData.CopyData(ear.pd);
        UnityEditor.AssetDatabase.CreateAsset(earData, newPath + "/ear.asset");
        newScriptableObject.earData = earData;

        PartData eyebrowData = ScriptableObject.CreateInstance<PartData>();
        eyebrowData.CopyData(eyebrow.pd);
        UnityEditor.AssetDatabase.CreateAsset(eyebrowData, newPath + "/eyebrow.asset");
        newScriptableObject.eyebrowData = eyebrowData;

        PartData eyeData = ScriptableObject.CreateInstance<PartData>();
        eyeData.CopyData(eye.pd);
        UnityEditor.AssetDatabase.CreateAsset(eyeData, newPath + "/eye.asset");
        newScriptableObject.eyeData = eyeData;

        PartData hairBackData = ScriptableObject.CreateInstance<PartData>();
        hairBackData.CopyData(hairBack.pd);
        UnityEditor.AssetDatabase.CreateAsset(hairBackData, newPath + "/hairBack.asset");
        newScriptableObject.hairBackData = hairBackData;

        PartData hairFrontData = ScriptableObject.CreateInstance<PartData>();
        hairFrontData.CopyData(hairFront.pd);
        UnityEditor.AssetDatabase.CreateAsset(hairFrontData, newPath + "/hairFront.asset");
        newScriptableObject.hairFrontData = hairFrontData;

        PartData headData = ScriptableObject.CreateInstance<PartData>();
        headData.CopyData(head.pd);
        UnityEditor.AssetDatabase.CreateAsset(headData, newPath + "/head.asset");
        newScriptableObject.headData = headData;

        PartData mouthData = ScriptableObject.CreateInstance<PartData>();
        mouthData.CopyData(mouth.pd);
        UnityEditor.AssetDatabase.CreateAsset(mouthData, newPath + "/mouth.asset");
        newScriptableObject.mouthData = mouthData;

        PartData neckData = ScriptableObject.CreateInstance<PartData>();
        neckData.CopyData(neck.pd);
        UnityEditor.AssetDatabase.CreateAsset(neckData, newPath + "/neck.asset");
        newScriptableObject.neckData = neckData;

        PartData noseData = ScriptableObject.CreateInstance<PartData>();
        noseData.CopyData(nose.pd);
        UnityEditor.AssetDatabase.CreateAsset(noseData, newPath + "/nose.asset");
        newScriptableObject.noseData = noseData;

        currentChar = newScriptableObject;
        UnityEditor.AssetDatabase.CreateAsset(newScriptableObject, newPath + "/" + characterName + ".asset");
    }

    [ContextMenu("LoadCharacter")]
    public void LoadChara(){
        CharacterData cd = (CharacterData)AssetDatabase.LoadAssetAtPath("Assets/NewFace/PartData/Characters/" + characterName + "/" + characterName + ".asset", typeof(CharacterData));
        Morph(cd);
    }

    public void Morph(CharacterData cd){
        ear.pd.CopyData(cd.earData);
        ear.UpdateAllTransformValues();
        ear.UpdateAllShadersValue(0f);
        eyebrow.pd.CopyData(cd.eyebrowData);
        eyebrow.UpdateAllTransformValues();
        eyebrow.UpdateAllShadersValue(0f);
        eye.pd.CopyData(cd.eyeData);
        eye.UpdateAllTransformValues();
        eye.UpdateAllShadersValue(0f);
        hairBack.pd.CopyData(cd.hairBackData);
        hairBack.UpdateAllTransformValues();
        hairBack.UpdateAllShadersValue(0f);
        hairFront.pd.CopyData(cd.hairFrontData);
        hairFront.UpdateAllTransformValues();
        hairFront.UpdateAllShadersValue(0f);
        head.pd.CopyData(cd.headData);
        head.UpdateAllTransformValues();
        head.UpdateAllShadersValue(0f);
        mouth.pd.CopyData(cd.mouthData);
        mouth.UpdateAllTransformValues();
        mouth.UpdateAllShadersValue(0f);
        neck.pd.CopyData(cd.neckData);
        neck.UpdateAllTransformValues();
        neck.UpdateAllShadersValue(0f);
        nose.pd.CopyData(cd.noseData);
        nose.UpdateAllTransformValues();
        nose.UpdateAllShadersValue(0f);
    }

}