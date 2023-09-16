using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FaceController))]
public class FaceEditor : Editor
{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();

        EditorGUILayout.HelpBox("This is a help box.", MessageType.Info);

        FaceController faceControl = (FaceController)target;
        if(GUILayout.Button("Randomize All")){
            faceControl.AllRandom();
            faceControl.SetTransformValues();
            faceControl.SetShaderValues();
        }

        if(GUILayout.Button("Randomize Head")){
            faceControl.RandomHead();
            faceControl.SetTransformValues();
            faceControl.SetShaderValues();
        }

        if(GUILayout.Button("Randomize Neck")){
            faceControl.RandomNeck();
            faceControl.SetTransformValues();
            faceControl.SetShaderValues();
        }

        if(GUILayout.Button("Randomize Eyes")){
            faceControl.RandomEye();
            faceControl.SetTransformValues();
            faceControl.SetShaderValues();
        }

        if(GUILayout.Button("Randomize Eyebrows")){
            faceControl.RandomEyebrow();
            faceControl.SetTransformValues();
            faceControl.SetShaderValues();
        }

        if(GUILayout.Button("Randomize Nose")){
            faceControl.RandomNose();
            faceControl.SetTransformValues();
            faceControl.SetShaderValues();
        }

        if(GUILayout.Button("Randomize Mouth")){
            faceControl.RandomMouth();
            faceControl.SetTransformValues();
            faceControl.SetShaderValues();
        }

        if(GUILayout.Button("Randomize Ear")){
            faceControl.RandomEar();
            faceControl.SetTransformValues();
            faceControl.SetShaderValues();
        }

        if(GUILayout.Button("Load Character")){
            faceControl.LoadCharacterData();
        }

        if(GUILayout.Button("Save Character")){
            faceControl.SaveCharacterData();
        }
    }
}
