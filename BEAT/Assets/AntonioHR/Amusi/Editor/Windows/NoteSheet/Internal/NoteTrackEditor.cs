using AntonioHR.Amusi.Data;
using AntonioHR.Amusi.Editor.Internal;
using AntonioHR.Amusi.Internal;

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.Amusi.Editor.Window.NoteSheet.Internal
{
    public class NoteTrackEditor
    {
        public event Action DataUpdated;

        private NoteTrack noteTrack;
        private NoteSheetEditor owner;

        private Vector2 cursor;
        private int eventId;

        //Editor state
        enum ActionTarget { NoteCornerLeft, NoteCornerRight, Note, NewNote, Empty}
        ActionTarget currentActionTarget = ActionTarget.Empty;
        int grabbedNoteIndex;
        Vector2 grabOffset;
        Vector2 grabStart;

        Note ghostNote;

        public int Width
        {
            get
            {
                return (int)(owner.CueLengthInBeats * NoteSheetEditorWindow.configs.BeatWidth);
            }
        }
        public int Height
        {
            get
            {
                var st = SubtracksToShow;

                return (int)LabelSize.y + (st) * SubtrackHeight + (st -1) * SubtrackSpacing;
            }
        }
        

        public NoteTrackEditor(NoteTrack track, NoteSheetEditor owner)
        {
            this.noteTrack = track;
            this.owner = owner;
        }
        
        public void Update()
        {
            eventId = GUIUtility.GetControlID(FocusType.Passive);

            AddCursorRects();

            switch (Event.current.GetTypeForControl(eventId))
            {
                case EventType.Repaint:
                    OnRepaint();
                    break;
                case EventType.MouseDown:
                    OnMouseDown();
                    break;
                case EventType.MouseUp:
                    OnMouseUp();
                    break;
                case EventType.MouseMove:
                    //Nothing, probably
                    break;
                case EventType.MouseDrag:
                    OnMouseDrag();
                    break;
                case EventType.KeyDown:
                    //If there's a note selected, delete it
                    break;
                case EventType.ContextClick:
                    //Show menu with "delete" if over note
                    break;
                default:
                    break;
            }
        }

        private void OnMouseDown()
        {
            var mousePos = Event.current.mousePosition;
            float beat;
            int subTrack;
            if (BeatAtPosition(mousePos, out beat) && SubtrackAtPosition(mousePos, out subTrack))
            {
                int noteIndex = noteTrack.notes.FindIndex(x => x.subTrack == subTrack && x.Contains(beat));
                if (noteIndex != -1)
                {
                    //Clicked on a note
                    var note = noteTrack.notes[noteIndex];
                    var bounds = BoundsForNote(note);

                    grabStart = mousePos;
                    grabOffset = mousePos - bounds.position;

                    grabbedNoteIndex = noteIndex;
                    if (Event.current.button == 0)
                    {
                        if (bounds.RightBorder(NoteResizeBorderWidth).Contains(mousePos))
                        {
                            currentActionTarget = ActionTarget.NoteCornerRight;
                        }
                        else if (bounds.LeftBorder(NoteResizeBorderWidth).Contains(mousePos))
                        {
                            currentActionTarget = ActionTarget.NoteCornerLeft;
                        }
                        else
                        {
                            currentActionTarget = ActionTarget.Note;
                        }
                    } else if(Event.current.button == 2)
                    {
                        noteTrack.notes.RemoveAt(noteIndex);
                    }
                }
                else
                {
                    if (Event.current.clickCount == 2)
                    {
                        if (TryCreateNoteAtMousePosition() && DataUpdated != null)
                        {
                            DataUpdated();
                        }
                    }
                }
                Event.current.Use();
            }
        }

        private void OnMouseDrag()
        {
            switch (currentActionTarget)
            {
                case ActionTarget.NoteCornerLeft:
                    ProcessNodeResizeToLeft();
                    break;
                case ActionTarget.NoteCornerRight:
                    ProcessNodeResizeToRight();
                    break;
                case ActionTarget.Note:
                    ProcessNoteDrag();
                    break;
                default:
                    break;
            }
        }
        private void ProcessNodeResizeToRight()
        {
            var currNote = GrabbedNote;
            Vector2 delta = Event.current.mousePosition - grabStart;
            float beatDelta = delta.x / BeatWidth;
            beatDelta = MusicTreeEditorUtilities.RoundBeat(beatDelta, NoteResizeSnap, NoteResizeSnap / 2);
            float targetDuration = currNote.duration + beatDelta;
            if (targetDuration > 0)
            {
                var newGhostNote = new Note() { start = currNote.start, subTrack = currNote.subTrack, duration = targetDuration };

                if (noteTrack.IsUpdateValid(grabbedNoteIndex, newGhostNote))
                {
                    ghostNote = newGhostNote;
                }
                Event.current.Use();
            }
        }
        private void ProcessNodeResizeToLeft()
        {
            var currNote = GrabbedNote;
            Vector2 delta = Event.current.mousePosition - grabStart;
            float beatDelta = delta.x / BeatWidth;
            beatDelta = MusicTreeEditorUtilities.RoundBeat(beatDelta, NoteResizeSnap, NoteResizeSnap / 2);
            float targetDuration = currNote.duration - beatDelta;
            float targetStart = currNote.start + beatDelta;
            if (targetDuration > 0)
            {
                var newGhostNote = new Note() { start = targetStart, subTrack = currNote.subTrack, duration = targetDuration };

                if (noteTrack.IsUpdateValid(grabbedNoteIndex, newGhostNote))
                {
                    ghostNote = newGhostNote;
                }
                Event.current.Use();
            }
        }
        private void ProcessNoteDrag()
        {
            var currNote = GrabbedNote;
            var offsetPos = Event.current.mousePosition - grabOffset;
            var realPos = Event.current.mousePosition;
            float beat;
            int subtrack;
            if (BeatAtPosition(offsetPos, out beat) && SubtrackAtPosition(realPos, out subtrack))
            {
                beat = MusicTreeEditorUtilities.RoundBeat(beat, NoteMoveSnap, NoteMoveSnap / 2);
                var newGhostNote = new Note() { duration = currNote.duration, start = beat, subTrack = subtrack };
                
                if (noteTrack.IsUpdateValid(grabbedNoteIndex, newGhostNote))
                {
                    ghostNote = newGhostNote;
                }
                Event.current.Use();
            }
        }

        private void OnMouseUp()
        {
            //Try to execute whichever action is being performed
            //Button click
            switch (currentActionTarget)
            {
                case ActionTarget.NoteCornerLeft:
                    TryApplyGhostNote();
                    break;
                case ActionTarget.NoteCornerRight:
                    TryApplyGhostNote();
                    break;
                case ActionTarget.Note:
                    TryApplyGhostNote();
                    break;
                case ActionTarget.Empty:
                    break;
                default:
                    break;
            }
        }
        private void TryApplyGhostNote()
        {
            if (noteTrack.TryUpdate(grabbedNoteIndex, ghostNote))
            {
                Event.current.Use();
                if (DataUpdated != null)
                    DataUpdated();
            }
            currentActionTarget = ActionTarget.Empty;
        }

        private void OnRepaint()
        {
            cursor = Vector2.zero;

            DrawBackground();

            DrawLabel();
            DrawSubtracks();
            
            if(currentActionTarget == ActionTarget.Note || currentActionTarget == ActionTarget.NoteCornerRight
                || currentActionTarget == ActionTarget.NoteCornerLeft)
            {
                
                GUI.DrawTexture(BoundsForNote(ghostNote), GhostNoteTexture);
            }

            DrawBars();

        }

        private void DrawBars()
        {
            for (int i = 0; i < (int)owner.CueLengthInBeats; i++)
            {
                var size = new Vector2(2, SubtrackHeight * SubtracksToShow);
                var rect = new Rect(NoteTracksStart + Vector2.right * (i * BeatWidth - size.x / 2), size);
                GUI.DrawTexture(rect, BG);
            }




            int barCount = (int)(owner.CueLengthInBeats / owner.NotesPerBar);
            for (int i = 1; i < barCount; i++)
            {
                var size = new Vector2(5, SubtrackHeight * SubtracksToShow);
                var measureSize = owner.NotesPerBar * BeatWidth;
                var rect = new Rect(NoteTracksStart +  Vector2.right * (i * measureSize - size.x/2) , size);
                GUI.DrawTexture(rect, BG);
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
            GUI.Box(SubtrackBoundsAtCursor, GUIContent.none, SubtrackStyle);
        }
        private void DrawSubtrack(List<Note> notes, int subtrackIndex)
        {
            var bounds = SubtrackBoundsAtCursor;
            GUI.Box(bounds, GUIContent.none, SubtrackStyle);

            DrawSubtrackNotes(notes);

            cursor.y += bounds.height + SubtrackSpacing;
        }
        private void DrawSubtrackNotes(List<Note> subTrackNotes)
        {
            for (int j = 0; j < subTrackNotes.Count; j++)
            {
                DrawNoteWithCursor(subTrackNotes[j], cursor);
            }
        }
        private void DrawNoteWithCursor(Note note, Vector2 cursor)
        {
            int width = (int)(note.duration * BeatWidth);
            Vector2 start = cursor + Vector2.right * (BeatWidth * note.start);
            var drawRect = new Rect(start, new Vector2(width, SubtrackHeight));
            GUI.DrawTexture(drawRect, NoteTexture);
        }
        
        
        private bool TryCreateNoteAtMousePosition()
        {
            var mousePos = Event.current.mousePosition;
            float beatPos;
            int subtrack;


            if (BeatAtPosition(mousePos, out beatPos) && SubtrackAtPosition(mousePos, out subtrack))
            {
                beatPos = MusicTreeEditorUtilities.RoundBeat(beatPos, NoteInsertionSnap, NoteInsertionSnap);
                return noteTrack.TryAddNote(subtrack, beatPos, DefaultNoteSize);
            }
            return false;
        }

        private void AddCursorRects()
        {
            foreach (var note in noteTrack.notes)
            {
                var bounds = BoundsForNote(note);
                var borders = bounds.HorizontalBorders(NoteResizeBorderWidth);
                foreach (var border in borders)
                {
                    EditorGUIUtility.AddCursorRect(border, MouseCursor.ResizeHorizontal);
                }
                EditorGUIUtility.AddCursorRect(bounds, MouseCursor.Arrow);
            }
            EditorGUIUtility.AddCursorRect(NoteTracksBounds, MouseCursor.ArrowPlus);
        }

        private bool BeatAtPosition(Vector2 mousePosition, out float beat)
        {
            beat = mousePosition.x / BeatWidth;
            return beat >= 0 && beat < owner.CueLengthInBeats;
            
        }
        private bool SubtrackAtPosition(Vector2 mousePosition, out int subtrack)
        {
            float y = mousePosition.y - LabelSize.y;
            try
            {
                subtrack = Mathf.FloorToInt(y / SubtrackHeight);
                return subtrack >= 0 && subtrack < SubtracksToShow;
            }
            catch (DivideByZeroException)
            {
                subtrack = -1;
                return false;
            }
        }


        private Rect BoundsForNote(Note n)
        {
            int width = (int)(n.duration * BeatWidth);
            Vector2 start = NoteTracksStart + 
                new Vector2(0, n.subTrack * SubtrackHeight) + 
                Vector2.right * (BeatWidth * n.start);
            return new Rect(start, new Vector2(width, SubtrackHeight));
        }





        private Note GrabbedNote { get { return noteTrack.notes[grabbedNoteIndex]; } }

        private Rect SubtrackBoundsAtCursor { get { return new Rect(cursor, new Vector2(Width, SubtrackHeight)); } }
        private int SubtracksToShow { get { return noteTrack.SubtrackCount() + 1; } }

        private Rect LabelRect { get { return new Rect(cursor, LabelSize); } }
        private Vector2 LabelSize { get { return new Vector2(50, EditorGUIUtility.singleLineHeight); } }
        private int SubtrackHeight { get { return NoteSheetEditorWindow.configs.SubTrackHeight; } }
        private int SubtrackSpacing { get { return NoteSheetEditorWindow.configs.SubTrackSpacing; } }

        private Vector2 NoteTracksStart { get { return new Vector2(0, LabelSize.y); } }
        private Rect NoteTracksBounds { get { return new Rect(NoteTracksStart, new Vector2(Width, Height) -NoteTracksStart); } }
        private float NoteResizeBorderWidth { get { return NoteSheetEditorWindow.configs.NoteResizeBorderWidth; } }

        private float BeatWidth { get { return NoteSheetEditorWindow.configs.BeatWidth; } }
        private Rect TrackBounds { get { return new Rect(0, 0, Width, Height); } }

        private float NoteInsertionSnap { get { return NoteSheetEditorWindow.configs.NoteInsertionSnap; } }
        private float NoteResizeSnap { get { return NoteSheetEditorWindow.configs.NoteResizeSnap; } }
        private float NoteMoveSnap { get { return NoteSheetEditorWindow.configs.NoteMoveSnap; } }
        private float DefaultNoteSize { get { return NoteSheetEditorWindow.configs.DefaultNoteSize; } }


        private Texture BG { get { return NoteSheetEditorWindow.configs.BgTexture; } }
        private Texture NoteTexture { get { return NoteSheetEditorWindow.configs.NoteTextureActive; } }
        private Texture GhostNoteTexture { get { return NoteSheetEditorWindow.configs.GhostNoteTexture; } }

        private GUIStyle BGStyle { get { return NoteSheetEditorWindow.configs.Skin.box; } }
        private GUIStyle SubtrackStyle { get { return NoteSheetEditorWindow.configs.Skin.box; } }
    }
}
