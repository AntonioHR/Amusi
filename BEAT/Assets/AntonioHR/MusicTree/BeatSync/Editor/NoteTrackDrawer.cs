using AntonioHR.Editor;
using AntonioHR.MusicTree.BeatSync.Internal;
using Assets.AntonioHR.MusicTree.Editor.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.MusicTree.BeatSync.Editor
{
    public class NoteTrackDrawer
    {

        public event Action DataUpdated;

        private NoteTrack noteTrack;
        private NoteSheetDrawer parent;

        private Vector2 cursor;
        private bool noteAdded;


        public int Width
        {
            get
            {
                return (int)(parent.CueLengthInBeats * NoteSheetEditorWindow.configs.BeatWidth);
            }
        }
        public int Height
        {
            get
            {
                var st = noteTrack.SubtrackCount();

                return (int)LabelSize.y + (st + 1) * SubtrackHeight + (st) * SubtrackSpacing;
            }
        }

        public NoteTrackDrawer(NoteTrack track, NoteSheetDrawer parent)
        {
            this.noteTrack = track;
            this.parent = parent;
        }

        public void Draw()
        {
            noteAdded = false;
            cursor = Vector2.zero;

            DrawBackground();

            DrawLabel();
            DrawSubtracks();

            if (noteAdded)
            {
                if (DataUpdated != null)
                    DataUpdated();
            }
        }

        private void DrawBackground()
        {
            GUI.Box(TrackBounds, GUIContent.none, BGStyle);
        }

        private void DrawLabel()
        {
            GUI.Label(LabelRect, noteTrack.name);
            cursor.y += LabelSize.y;
        }

        private void DrawSubtracks()
        {
            var notesBySubtrack = noteTrack.BySubtrack();
            for (int i = 0; i < notesBySubtrack.Count; i++)
            {
                DrawSubtrack(notesBySubtrack[i], i);
            }
        }

        private void DrawSubtrack(List<Note> notes, int subtrackIndex)
        {
            var bounds = SubtrackBoundsAtCursor;
            GUI.Box(bounds, GUIContent.none, SubtrackStyle);

            DrawSubtrackNotes(notes);

            cursor.y += bounds.height + SubtrackSpacing;


            if (EditorGUIUtils.DoubleClickArea(bounds))
            {
                float pos = BeatAtPosition(Event.current.mousePosition);
                Debug.Assert(!noteAdded, "Should have triggers from two double click areas in the same frame");
                noteAdded = noteTrack.TryAddNote(subtrackIndex, pos, DefaultNoteSize);
            }
        }
        
        private void DrawSubtrackNotes(List<Note> subTrackNotes)
        {
            for (int j = 0; j < subTrackNotes.Count; j++)
            {
                DrawNote(subTrackNotes[j], cursor);
            }
        }

        private void DrawNote(Note note, Vector2 cursor)
        {
            int width = (int)(note.duration * BeatWidth);
            Vector2 start = cursor + Vector2.right * (BeatWidth * note.start);
            var drawRect = new Rect(start, new Vector2(width, SubtrackHeight));
            GUI.DrawTexture(drawRect, NoteTexture);

            if (GUI.Button(drawRect, GUIContent.none, GUIStyle.none))
            {

            }
        }

        private float BeatAtPosition(Vector2 mousePosition)
        {
            return MusicTreeEditorUtilities.RoundToBeat(mousePosition.x / BeatWidth, NoteInsertionSnap);
            
        }

        
        private Rect SubtrackBoundsAtCursor { get { return new Rect(cursor, new Vector2(Width, SubtrackHeight)); } }

        private Rect LabelRect { get { return new Rect(cursor, LabelSize); } }
        private Vector2 LabelSize { get { return new Vector2(50, EditorGUIUtility.singleLineHeight); } }
        private int SubtrackHeight { get { return NoteSheetEditorWindow.configs.SubTrackHeight; } }
        private int SubtrackSpacing { get { return NoteSheetEditorWindow.configs.SubTrackSpacing; } }

        private float BeatWidth { get { return NoteSheetEditorWindow.configs.BeatWidth; } }
        private Rect TrackBounds { get { return new Rect(0, 0, Width, Height); } }

        private float NoteInsertionSnap { get { return NoteSheetEditorWindow.configs.NoteInsertionSnap; } }
        private float DefaultNoteSize { get { return NoteSheetEditorWindow.configs.DefaultNoteSize; } }


        private Texture BG { get { return NoteSheetEditorWindow.configs.BgTexture; } }
        private Texture NoteTexture { get { return NoteSheetEditorWindow.configs.NoteTextureActive; } }

        private GUIStyle BGStyle { get { return NoteSheetEditorWindow.configs.Skin.box; } }
        private GUIStyle SubtrackStyle { get { return NoteSheetEditorWindow.configs.Skin.box; } }
    }
}
