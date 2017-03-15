using BeatFW;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(BeatPatchData))]
public class BeatPatchDataEditor : Editor
{
    private ReorderableList list;



    private void OnEnable()
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("patches"), true, true, true, true);
        list.drawElementCallback = DrawCallback;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    void DrawCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;
        float w1 = Mathf.Min(150, rect.width * .35f);
        float w2 = Mathf.Min(300, rect.width * .60f);
        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y, w1, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("name"), GUIContent.none);
        EditorGUI.PropertyField(
            new Rect(rect.x + rect.width - w2, rect.y, w2, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("clip"), GUIContent.none);
    }
}
