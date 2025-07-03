using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Editor button that allows cloning of an object via the EditorCreateObjects script.
/// </summary>
[CustomEditor(typeof(EditorCreateObjects))]
public class EditorGUIButtonCreateObjects : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorCreateObjects objectCreator = (EditorCreateObjects)target;
        if (GUILayout.Button("Create new object(s)"))
        {
            objectCreator.CreateObjects();
        }

        if (GUILayout.Button("Destroy object(s)"))
        {
            objectCreator.DestroyObjects();
        }
    }
}

#endif
