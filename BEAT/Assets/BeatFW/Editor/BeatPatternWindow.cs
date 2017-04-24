using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using BeatFW.Util;

namespace BeatFW.Editor
{
    public class BeatPatternWindow : EditorWindow
    {
        static float divisions = .25f;
        BeatPattern currentObject;

        SerializedProperty beatsPerMeasure;
        SerializedProperty measureCount;
        SerializedProperty notes;

        static GUIStyle toggles;

        GUISkin skin;

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
        void OnEnable()
        {
            if(currentObject != null)
            {
                UpdatePatternObject(currentObject);
            }
            skin = Resources.Load<GUISkin>("EditorSkin");
            toggles = skin.toggle;
        }
        void OnGUI()
        {
            if (currentObject == null)
            {
                EditorGUILayout.LabelField("No Pattern Object Selected");
                return;
            }
            beatsPerMeasure.serializedObject.Update();
            EditorGUILayout.PropertyField(beatsPerMeasure);
            EditorGUILayout.PropertyField(measureCount);
            EditorGUILayout.PropertyField(notes, true);
            beatsPerMeasure.serializedObject.ApplyModifiedProperties();


            var rect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight * 2);
            rect.x += rect.width *.125f;
            rect.width *= .75f;
            //DrawPatternView(rect, currentObject);
            DrawEditablePatern(rect, currentObject);
        }


        public static void DrawEditablePatern(Rect rect, BeatPattern pat)
        {
            int totalBeats = pat.GetSize(divisions);
            //Draw Background Rect
            Color background = new Color(.35f, .35f, .35f);
            EditorGUI.DrawRect(rect, background);

            var insideRect = rect.MinusMargin(10, 5);
            //Draw Buttons
            var buttonRects = insideRect.GetHorGridInside(totalBeats, 5);

            var inactive = new Color(0, 1, 1);
            var active = new Color(0, .25f, .25f);
            for (int i = 0; i < totalBeats; i++)
            {
                EditorGUI.DrawRect(buttonRects[i], pat.HasNoteOn(i, divisions) ? active : inactive);
                var hasNote = pat.HasNoteOn(i, divisions);
                var isToggled = GUI.Toggle(buttonRects[i], hasNote, GUIContent.none, GUIStyle.none);
                if(!hasNote && isToggled)
                {
                    pat.AddNoteOn(i);
                } else if(hasNote && !isToggled)
                {
                    pat.RemoveNoteOn(i);
                }
            }
        }

        public static void DrawPatternView(Rect rect, BeatPattern pat)
        {
            float totalBeats = (pat.MeasureCount * pat.BeatsPerMeasure);
            var beatWidth = rect.width / totalBeats;
            var noteWidth = beatWidth * .25f;
            var halfNoteWidth = noteWidth * .5f;


            //Draw Background Rect
            EditorGUI.DrawRect(rect, Color.black);


            
            var noteColor = new Color(0, 1, 1, .7f);
            var noteHighlightColor = new Color(.5f, 1, 1, .7f);
            //Draw Notes
            foreach (var note in pat.Notes)
            {
                EditorGUI.DrawRect(new Rect(rect.x + beatWidth * note, rect.y, noteWidth, rect.height), noteColor);
                EditorGUI.DrawRect(new Rect(rect.x + beatWidth * note, rect.y, noteWidth *.9f, rect.height), noteColor);
            }

            var measureColor = new Color(1, 1f, 1f, 1f);
            var beatColor = new Color(1, .7f, .7f, .7f);
            var halfBeatColor = new Color(1, .7f, .7f, .3f);
            //Draw divisions
            for (float i = divisions; i < totalBeats; i+= divisions)
            {
                EditorGUI.DrawRect(new Rect(rect.x + beatWidth * i - 2, rect.y, 4, rect.height), 
                    i % 1.0f != 0? halfBeatColor : (i % pat.BeatsPerMeasure != 0? beatColor : measureColor));
            }
        }
    }
}
