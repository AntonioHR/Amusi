using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace BeatFW.Editor
{
    public class BeatPatternWindow : EditorWindow
    {
        public SerializedProperty Prop { 
            set
            {
                beatsPerMeasure = value.FindPropertyRelative("beatsPerMeasure");
                measureCount = value.FindPropertyRelative("measureCount");
                notes = value.FindPropertyRelative("notes");
            }
        }


        SerializedProperty beatsPerMeasure;
        SerializedProperty measureCount;
        SerializedProperty notes;
        public static void Show(SerializedProperty BeatPatternProperty)
        {
            EditorWindow.GetWindow<BeatPatternWindow>().Prop = BeatPatternProperty;
        }
        void OnGUI()
        {
            beatsPerMeasure.serializedObject.Update();
            EditorGUILayout.PropertyField(beatsPerMeasure);
            EditorGUILayout.PropertyField(measureCount);
            EditorGUILayout.PropertyField(notes, true);
            beatsPerMeasure.serializedObject.ApplyModifiedProperties();
        }
    }
}
