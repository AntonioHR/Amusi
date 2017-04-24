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
        static float minTempo = .25f;
        BeatPattern currentObject;

        SerializedProperty beatsPerMeasure;
        SerializedProperty measureCount;
        SerializedProperty notes;

        bool locked;

        static GUIStyle toggles;

        GUISkin skin;

        void UpdatePatternObject(BeatPattern obj, bool lockIt = true)
        {
            currentObject = obj;
            var serialized = new SerializedObject(currentObject);
            beatsPerMeasure = serialized.FindProperty("beatsPerMeasure");
            measureCount = serialized.FindProperty("measureCount");
            notes = serialized.FindProperty("notes");
            if(lockIt)
                locked = true;
            this.Repaint();
        }

        public static void Show(BeatPattern BeatPatternProperty)
        {
           EditorWindow.GetWindow<BeatPatternWindow>().UpdatePatternObject(BeatPatternProperty);
        }
        void OnEnable()
        {
            if(currentObject != null)
            {
                UpdatePatternObject(currentObject, false);
            }
            skin = Resources.Load<GUISkin>("EditorSkin");
            toggles = skin.toggle;
        }
        void OnGUI()
        {
            var wasLocked = locked;
            locked = GUILayout.Toggle(locked, "Lock");
            if (wasLocked && !locked)
                CheckUpdate();
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

        void OnSelectionChange()
        {
            if(!locked)
            {
                CheckUpdate();
            }
        }
        void CheckUpdate()
        {
            var eventMaker = Selection.activeGameObject.GetComponent<BeatPatternBaseEventMaker>();
            if (eventMaker != null)
            {
                this.UpdatePatternObject(eventMaker.pattern, false);
            }
        }

        public static void DrawEditablePatern(Rect rect, BeatPattern pat)
        {
            int totalBeats = pat.GetSize(minTempo);
            //Draw Background Rect
            Color background = new Color(.35f, .35f, .35f);
            EditorGUI.DrawRect(rect, background);

            //Draw Buttons
            //var grid = rect.MinusMargin(10, 5).GetHorGridInsideWithSpaces(totalBeats, 2, Vector2.zero);
            var grid = rect.MinusMargin(10, 0).GetHorGridInsideWithSpaces(totalBeats, 2, new Vector2(0, 5));

            var active = new Color(0, 1, 1);
            var inactive = new Color(0, .25f, .25f);
            for (int i = 0; i < grid.Slots.Length; i++)
            {
                EditorGUI.DrawRect(grid.Slots[i], pat.HasNoteOn(i, minTempo) ? active : inactive);
                var hasNote = pat.HasNoteOn(i, minTempo);
                var isToggled = GUI.Toggle(grid.Slots[i], hasNote, GUIContent.none, GUIStyle.none);
                if(!hasNote && isToggled)
                {
                    pat.AddNoteOn(i);
                } else if(hasNote && !isToggled)
                {
                    pat.RemoveNoteOn(i);
                }
            }
            
            //var defaultColor = new Color(.25f, .25f, .25f);
            var defaultColor = background;
            var beatColor = new Color(.65f, .65f, .65f);
            var measureColor = new Color(1, 1, 1);
            int indxPerBeat = Mathf.RoundToInt(1/minTempo);
            int beatPoint = indxPerBeat - 1;
            int measurePoint = (indxPerBeat * pat.BeatsPerMeasure) - 1;
            for (int i = 0; i < grid.Divisions.Length; i++)
            {
                Color c = defaultColor;
                //If it is a Beat Division
                if(i % indxPerBeat == beatPoint)
                {
                    c = i % (indxPerBeat * pat.BeatsPerMeasure) ==  measurePoint ? measureColor:beatColor;
                }
                EditorGUI.DrawRect(grid.Divisions[i], c);
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
            for (float i = minTempo; i < totalBeats; i+= minTempo)
            {
                EditorGUI.DrawRect(new Rect(rect.x + beatWidth * i - 2, rect.y, 4, rect.height), 
                    i % 1.0f != 0? halfBeatColor : (i % pat.BeatsPerMeasure != 0? beatColor : measureColor));
            }
        }
    }
}
