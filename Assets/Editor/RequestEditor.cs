using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShaderRequest))]
public class ShaderRequestEditor : Editor
{
   public override void OnInspectorGUI(){
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("This is a help box.", MessageType.Info);

        ShaderRequest fs = (ShaderRequest)target;
        if(GUILayout.Button("Randomize All")){
        }
   }
}
