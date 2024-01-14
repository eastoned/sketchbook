using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


public class SaveCharacterProfile : MonoBehaviour
{
    public string characterName;
    public CharacterData charData;
    public PartController ear, eyebrow, eye, hairBack, hairFront, head, mouth, neck, nose;
    
    #if UNITY_EDITOR
    [ContextMenu("SaveCharacter")]
    public void SaveChara(){
        CharacterData cd = (CharacterData)AssetDatabase.LoadAssetAtPath("Assets/NewFace/PartData/Characters/" + characterName + "/" + characterName + ".asset", typeof(CharacterData));
        
        if(cd != null){
            Debug.Log("Character exists already with this name. Overriding previous save data");
            
            cd.earData.CopyData(ear.pd);
            cd.eyebrowData.CopyData(eyebrow.pd);
            cd.eyeData.CopyData(eye.pd);
            cd.hairBackData.CopyData(hairBack.pd);
            cd.hairFrontData.CopyData(hairFront.pd);
            cd.headData.CopyData(head.pd);
            cd.mouthData.CopyData(mouth.pd);
            cd.neckData.CopyData(neck.pd);
            cd.noseData.CopyData(nose.pd);

        }else{
            Debug.Log("Creating new character named: " + characterName);

            string guid = AssetDatabase.CreateFolder("Assets/NewFace/PartData/Characters", characterName);
            
            string newPath = AssetDatabase.GUIDToAssetPath(guid);
            CharacterData newScriptableObject = ScriptableObject.CreateInstance<CharacterData>();
            newScriptableObject.name = characterName;
            newScriptableObject.allParts = new PartData[9];

            PartData earData = ScriptableObject.CreateInstance<PartData>();
            earData.CopyData(ear.pd);
            UnityEditor.AssetDatabase.CreateAsset(earData, newPath + "/ear.asset");
            newScriptableObject.earData = earData;
            newScriptableObject.allParts[0] = earData;

            PartData eyebrowData = ScriptableObject.CreateInstance<PartData>();
            eyebrowData.CopyData(eyebrow.pd);
            UnityEditor.AssetDatabase.CreateAsset(eyebrowData, newPath + "/eyebrow.asset");
            newScriptableObject.eyebrowData = eyebrowData;
            newScriptableObject.allParts[2] = eyebrowData;

            PartData eyeData = ScriptableObject.CreateInstance<PartData>();
            eyeData.CopyData(eye.pd);
            UnityEditor.AssetDatabase.CreateAsset(eyeData, newPath + "/eye.asset");
            newScriptableObject.eyeData = eyeData;
            newScriptableObject.allParts[1] = eyeData;

            PartData hairBackData = ScriptableObject.CreateInstance<PartData>();
            hairBackData.CopyData(hairBack.pd);
            UnityEditor.AssetDatabase.CreateAsset(hairBackData, newPath + "/hairBack.asset");
            newScriptableObject.hairBackData = hairBackData;
            newScriptableObject.allParts[3] = hairBackData;

            PartData hairFrontData = ScriptableObject.CreateInstance<PartData>();
            hairFrontData.CopyData(hairFront.pd);
            UnityEditor.AssetDatabase.CreateAsset(hairFrontData, newPath + "/hairFront.asset");
            newScriptableObject.hairFrontData = hairFrontData;
            newScriptableObject.allParts[4] = hairFrontData;

            PartData headData = ScriptableObject.CreateInstance<PartData>();
            headData.CopyData(head.pd);
            UnityEditor.AssetDatabase.CreateAsset(headData, newPath + "/head.asset");
            newScriptableObject.headData = headData;
            newScriptableObject.allParts[5] = headData;

            PartData mouthData = ScriptableObject.CreateInstance<PartData>();
            mouthData.CopyData(mouth.pd);
            UnityEditor.AssetDatabase.CreateAsset(mouthData, newPath + "/mouth.asset");
            newScriptableObject.mouthData = mouthData;
            newScriptableObject.allParts[6] = mouthData;

            PartData neckData = ScriptableObject.CreateInstance<PartData>();
            neckData.CopyData(neck.pd);
            UnityEditor.AssetDatabase.CreateAsset(neckData, newPath + "/neck.asset");
            newScriptableObject.neckData = neckData;
            newScriptableObject.allParts[7] = neckData;

            PartData noseData = ScriptableObject.CreateInstance<PartData>();
            noseData.CopyData(nose.pd);
            UnityEditor.AssetDatabase.CreateAsset(noseData, newPath + "/nose.asset");
            newScriptableObject.noseData = noseData;
            newScriptableObject.allParts[8] = noseData;

            UnityEditor.AssetDatabase.CreateAsset(newScriptableObject, newPath + "/" + characterName + ".asset");
        }
    }
    #endif

    [ContextMenu("LoadCharacter")]
    public void LoadChara(){
        if(charData != null){
            Morph(charData);
        }else{
            //cannot load asset outside of editor
            #if UNITY_EDITOR
                CharacterData cd = (CharacterData)AssetDatabase.LoadAssetAtPath("Assets/NewFace/PartData/Characters/" + characterName + "/" + characterName + ".asset", typeof(CharacterData));
                Morph(cd);
            #endif
        }
    }
    

    public void Morph(CharacterData cd){
        ear.pd.CopyData(cd.earData);
        eyebrow.pd.CopyData(cd.eyebrowData);
        eye.pd.CopyData(cd.eyeData);
        hairBack.pd.CopyData(cd.hairBackData);
        hairFront.pd.CopyData(cd.hairFrontData);
        head.pd.CopyData(cd.headData);
        mouth.pd.CopyData(cd.mouthData);
        neck.pd.CopyData(cd.neckData);
        nose.pd.CopyData(cd.noseData);
        UpdateAllControllers();
    }

    public void UpdateAllControllers(){
        head.UpdateAllTransformValues();
        head.UpdateAllShadersValue(0f);
        eye.UpdateAllTransformValues();
        eye.UpdateAllShadersValue(0f);
        eye.mirroredPart.UpdateAllShadersValue(0f);
        eyebrow.UpdateAllTransformValues();
        eyebrow.UpdateAllShadersValue(0f);
        eyebrow.mirroredPart.UpdateAllShadersValue(0f);
        ear.UpdateAllTransformValues();
        ear.UpdateAllShadersValue(0f);
        ear.mirroredPart.UpdateAllShadersValue(0f);
        hairBack.UpdateAllTransformValues();
        hairBack.UpdateAllShadersValue(0f);
        hairFront.UpdateAllTransformValues();
        hairFront.UpdateAllShadersValue(0f);
        mouth.UpdateAllTransformValues();
        mouth.UpdateAllShadersValue(0f);
        neck.UpdateAllTransformValues();
        neck.UpdateAllShadersValue(0f);
        nose.UpdateAllTransformValues();
        nose.UpdateAllShadersValue(0f);
    }

}