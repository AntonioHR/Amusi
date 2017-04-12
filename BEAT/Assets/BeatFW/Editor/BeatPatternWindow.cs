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
        static float divisions = .25f;
        BeatPattern currentObject;

        SerializedProperty beatsPerMeasure;
        SerializedProperty measureCount;
        SerializedProperty notes;

        void UpdatePatternObject(BeatPattern obj)
        {
            currentObject = obj;
            var serialized = new SerializedObject(currentObject);
            beatsPerMeasure = serialized.FindProperty("beatsPerMeasure");
            measureCount = serialized.FindProperty("measureCount");
            notes = serialized.FindProperty("notes");
        }

        public static void Show(BeatPattern BeatPatternProperty)
        {
           EditorWindow.GetWindow<BeatPatternWindow>().UpdatePatternObject(BeatPatternProperty);
        }
        void OnGUI()
        {
            beatsPerMeasure.serializedObject.Update();
            EditorGUILayout.PropertyField(beatsPerMeasure);
            EditorGUILayout.PropertyField(measureCount);
            EditorGUILayout.PropertyField(notes, true);
            beatsPerMeasure.serializedObject.ApplyModifiedProperties();
            var rect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight * 2);
            rect.x += rect.width *.25f;
            rect.width *= .5f;
            DrawPattern(rect, currentObject);
        }

        public static void DrawPattern(Rect rect, BeatPattern pat)
        {
            float totalBeats = (pat.MeasureCount * pat.BeatsPerMeasure);
            var beatWidth = rect.width / totalBeats;
            var noteWidth = beatWidth * .25f;
            var halfNoteWidth = noteWidth * .5f;
            EditorGUI.DrawRect(rect, Color.black);
            var noteColor = new Color(0, 1, 1, .7f);
            var noteHighlightColor = new Color(.5f, 1, 1, .7f);
            foreach (var note in pat.Notes)
            {
                EditorGUI.DrawRect(new Rect(rect.x + beatWidth * note, rect.y, noteWidth, rect.height), noteColor);
                EditorGUI.DrawRect(new Rect(rect.x + beatWidth * note, rect.y, noteWidth *.9f, rect.height), noteColor);
            }
            var measureColor = new Color(1, 1f, 1f, 1f);
            var beatColor = new Color(1, .7f, .7f, .7f);
            var halfBeatColor = new Color(1, .7f, .7f, .3f);
            for (float i = divisions; i < totalBeats; i+= divisions)
            {
                EditorGUI.DrawRect(new Rect(rect.x + beatWidth * i - 2, rect.y, 4, rect.height), 
                    i % 1.0f != 0? halfBeatColor : (i % pat.BeatsPerMeasure != 0? beatColor : measureColor));
            }
        }
    }
}
