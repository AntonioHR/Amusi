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

        public int Width { get { return 800; } }
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

        public Texture BG { get { return NoteSheetEditorWindow.configs.BgTexture; } }

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

            var notesBySbutrack = noteTrack.BySubtrack();
            for (int i = 0; i < notesBySbutrack.Count; i++)
            {
                var subtrackRect = new Rect(cursor, new Vector2(Width, SubtrackHeight));
                GUI.Box(subtrackRect, GUIContent.none, SubtrackStyle);
                cursor.y += subtrackRect.height + SubtrackSpacing;
            }


            
        }
    }
}
