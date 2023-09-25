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
            //faceControl.AllRandom();
            //faceControl.SetTransformValues();
            
        }

        
    }
}
