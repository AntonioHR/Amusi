using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SimplePropertyWindow : EditorWindow {

    public SerializedProperty CurrentValue { get; set; }

    [MenuItem("Window/MyWindow")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SimplePropertyWindow));
    }

    public static void ShowWith(SerializedProperty val)
    {
        var window = EditorWindow.GetWindow<SimplePropertyWindow>();
        window.CurrentValue = val;
    }
    void OnGUI()
    {
        if (CurrentValue == null)
        {
            EditorGUILayout.HelpBox("No Value", MessageType.Error);
            return;
        }
        CurrentValue.serializedObject.Update();
        CurrentValue.stringValue = EditorGUILayout.TextArea(CurrentValue.stringValue);
        CurrentValue.serializedObject.ApplyModifiedProperties();
    }
}
