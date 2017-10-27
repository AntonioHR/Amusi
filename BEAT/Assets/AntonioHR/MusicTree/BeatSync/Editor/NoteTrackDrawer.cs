using AntonioHR.Editor;
using AntonioHR.MusicTree.BeatSync.Internal;
using AntonioHR.MusicTree.Editor.Internal;
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
        public NoteTrackDrawer(NoteTrack track, NoteSheetDrawer parent)
        {
            this.noteTrack = track;
            this.parent = parent;
        }



        public event Action DataUpdated;



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

        

        public void Draw()
        {
            noteAdded = false;
            drawCursor = Vector2.zero;

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
            drawCursor.y += LabelSize.y;
        }

        private void DrawSubtracks()
        {
            var notesBySubtrack = noteTrack.BySubtrack();
            for (int i = 0; i < notesBySubtrack.Count; i++)
            {
                SubtrackCursor = i;
                DrawSubtrack(notesBySubtrack[i]);
            }
        }

        private void DrawSubtrack(List<Note> notes)
        {
            var bounds = SubtrackBoundsAtCursor;
            GUI.Box(bounds, GUIContent.none, SubtrackStyle);
            
            DrawSubtrackNotes(notes);

            drawCursor.y += bounds.height + SubtrackSpacing;


            if (EditorGUIUtils.DoubleClickArea(bounds))
            {
                CreateNoteAtMousePosition();
            }
        }

        private void DrawSubtrackNotes(List<Note> subTrackNotes)
        {
            for (int j = 0; j < subTrackNotes.Count; j++)
            {
                NoteCursor = j;
                
                DrawNote(subTrackNotes[j]);
            }
        }

        private void DrawNote(Note note)
        {
            //int width = (int)(note.duration * BeatWidth);
            //Vector2 start = drawCursor + Vector2.right * (BeatWidth * note.start);
            //var drawRect = new Rect(start, new Vector2(width, SubtrackHeight));

            NoteDrawUtility.NoteOnCurrentTrack(note);


            //GUI.DrawTexture(drawRect, IsDrawingSelection ? NoteTextureSelected : NoteTextureIdle);

            //if (GUI.Button(drawRect, GUIContent.none, GUIStyle.none))
            //{
            //    selectionSubtrack = SubtrackCursor;
            //    selectionNote = NoteCursor;
            //}
        }


        private float BeatAtPosition(Vector2 mousePosition)
        {
            return MusicTreeEditorUtilities.RoundToBeat(mousePosition.x / BeatWidth, NoteInsertionSnap, NoteInsertionSnap);

        }

        private void CreateNoteAtMousePosition()
        {
            float beatPos = BeatAtPosition(Event.current.mousePosition);
            Note note;
            int noteIndex;
            noteAdded = noteTrack.TryAddNote(SubtrackCursor, beatPos, DefaultNoteSize, out noteIndex, out note);
            selectionSubtrack = SubtrackCursor;
            selectionNote = noteIndex;
        }
        
        public static bool IsSelection(int i, int j)
        {
            return i == selectionSubtrack && j == selectionNote;
        }

        public static void SelectCursor()
        {
            selectionSubtrack = SubtrackCursor;
            selectionNote = NoteCursor;
        }


        
        public static bool IsDrawingSelection { get { return IsSelection(SubtrackCursor, NoteCursor); } }

        #region Drawing Size params
        private Rect TrackBounds { get { return new Rect(0, 0, Width, Height); } }
        
        public static int SubtrackHeight { get { return NoteSheetEditorWindow.configs.SubTrackHeight; } }
        public static int SubtrackSpacing { get { return NoteSheetEditorWindow.configs.SubTrackSpacing; } }
        private Rect SubtrackBoundsAtCursor { get { return new Rect(drawCursor, new Vector2(Width, SubtrackHeight)); } }
        

        public static Rect LabelRect { get { return new Rect(drawCursor, LabelSize); } }
        public static Vector2 LabelSize { get { return new Vector2(50, EditorGUIUtility.singleLineHeight); } }

        public static float BeatWidth { get { return NoteSheetEditorWindow.configs.BeatWidth; } }
        #endregion

        #region Behaviour params
        private float NoteInsertionSnap { get { return NoteSheetEditorWindow.configs.NoteInsertionSnap; } }
        private float DefaultNoteSize { get { return NoteSheetEditorWindow.configs.DefaultNoteSize; } }
        #endregion

        #region Texuring and Styling params
        public static Texture BG { get { return NoteSheetEditorWindow.configs.BgTexture; } }
        public static Texture NoteTextureIdle { get { return NoteSheetEditorWindow.configs.NoteTextureInactive; } }
        public static Texture NoteTextureSelected { get { return NoteSheetEditorWindow.configs.NoteTextureActive; } }

        private GUIStyle BGStyle { get { return NoteSheetEditorWindow.configs.Skin.box; } }
        private GUIStyle SubtrackStyle { get { return NoteSheetEditorWindow.configs.Skin.box; } }
        #endregion



        private NoteTrack noteTrack;
        private NoteSheetDrawer parent;

        public static Vector2 drawCursor;
        public static int SubtrackCursor;
        public static int NoteCursor;

        private bool noteAdded;

        public static int selectionSubtrack = -1;
        public static int selectionNote = -1;
    }
}
