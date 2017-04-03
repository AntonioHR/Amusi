using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MyPlayer))]
[CanEditMultipleObjects]
public class MyPlayerEditor : Editor {
    SerializedProperty damageProp;
    SerializedProperty armorProp;
    SerializedProperty gunProp;



    void OnEnable()
    {
        damageProp = serializedObject.FindProperty("damage");
        armorProp = serializedObject.FindProperty("armor");
        gunProp = serializedObject.FindProperty("gun");
    }

    public override void OnInspectorGUI()
    {
        //Update the SerializedProperty - always do this in the beginning of OnInspectorGUI
        serializedObject.Update();

        PropertySlider(damageProp, 0, 100, "Damage");
        PropertySlider(armorProp, 0, 100, "Armor");

        EditorGUILayout.PropertyField(gunProp, new GUIContent("Gun Object"));

        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();

    }

    static void PropertySlider(SerializedProperty property, int min, int max, string label)
    {
        //Show the Custom GUI Controls
        EditorGUILayout.IntSlider(property, min, max, new GUIContent(label));

        //Only show the damage progress bar if all the objects have the same damage value:
        if (!property.hasMultipleDifferentValues)
            ProgressBar(property.intValue / (float)max, label);
    }

    //Custom GUILayout progress bar
    static void ProgressBar(float value, string label)
    {
        //Get a rect for the progress bar using the same margins as a textField
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }
}
