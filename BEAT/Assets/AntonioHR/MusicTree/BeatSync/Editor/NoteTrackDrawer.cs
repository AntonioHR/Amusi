using AntonioHR.MusicTree.BeatSync.Internal;
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

        private NoteTrack noteTrack;
        private NoteSheetDrawer parent;

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

                return LabelSize + (st + 1) * SubtrackHeight + (st) * SubtrackSpacing;
            }
        }

        public int LabelSize
        {
            get
            {
                return (int)EditorGUIUtility.singleLineHeight;
            }
        }
        public int SubtrackHeight
        {
            get
            {
                return NoteSheetEditorWindow.configs.SubTrackHeight;
            }
        }
        public int SubtrackSpacing
        {
            get
            {
                return NoteSheetEditorWindow.configs.SubTrackSpacing;
            }
        }
        public float BeatWidth { get { return NoteSheetEditorWindow.configs.BeatWidth; } }


        public Texture BG { get { return NoteSheetEditorWindow.configs.BgTexture; } }
        public Texture NoteTexture { get { return NoteSheetEditorWindow.configs.NoteTextureActive; } }

        public GUIStyle BGStyle { get { return NoteSheetEditorWindow.configs.Skin.box; } }
        public GUIStyle SubtrackStyle { get { return NoteSheetEditorWindow.configs.Skin.box; } }

        public NoteTrackDrawer(NoteTrack track, NoteSheetDrawer parent)
        {
            this.noteTrack = track;
            this.parent = parent;
        }

        public void Draw()
        {
            GUI.Box(new Rect(0, 0, Width, Height), GUIContent.none, BGStyle);

            Vector2 cursor = Vector2.zero;

            Vector2 labelSize = new Vector2(50, LabelSize);

            GUI.Label(new Rect(cursor, labelSize), noteTrack.name);
            cursor.y += labelSize.y;

            var notesBySubtrack = noteTrack.BySubtrack();
            for (int i = 0; i < notesBySubtrack.Count; i++)
            {
                var subtrackRect = new Rect(cursor, new Vector2(Width, SubtrackHeight));
                GUI.Box(subtrackRect, GUIContent.none, SubtrackStyle);

                for (int j = 0; j < notesBySubtrack[i].Count; j++)
                {
                    DrawNote(notesBySubtrack[i][j], cursor);
                }


                cursor.y += subtrackRect.height + SubtrackSpacing;
            }


            
        }

        private void DrawNote(Note note, Vector2 cursor)
        {
            int width = (int)(note.duration * BeatWidth);
            Vector2 start = cursor + Vector2.right * (BeatWidth * note.start);
            var drawRect = new Rect(start, new Vector2(width, SubtrackHeight));
            GUI.DrawTexture(drawRect, NoteTexture);
        }
    }
}
