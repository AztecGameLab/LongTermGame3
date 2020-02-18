using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralMapGenerator))]
[CanEditMultipleObjects]
public class LookAtPointEditor : Editor
{
    
    void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        
        DrawDefaultInspector();
        ProceduralMapGenerator gen = (ProceduralMapGenerator)target;
        if (GUILayout.Button("Generate new map"))
        {
            gen.Initialize();
        }
}
}
