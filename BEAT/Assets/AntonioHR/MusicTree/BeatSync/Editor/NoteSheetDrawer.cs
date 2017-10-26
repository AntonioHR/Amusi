using AntonioHR.MusicTree.BeatSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using AntonioHR.MusicTree.Editor;

namespace AntonioHR.MusicTree.BeatSync.Editor
{
    public class NoteSheetDrawer
    {
        public NoteSheet sheet;
        

        public int Height { get { return NoteSheetEditorWindow.configs.SubTrackHeight * SubtrackCount; } }

        public int Width { get { return BeatCount * NoteSheetEditorWindow.configs.BeatWidth; } }
        public int BeatCount { get { return BeatsPerMeasure * MeasureCount; } }
        public int BeatsPerMeasure { get { return MusicTreeEditorManager.Instance.BeatsPerMeasure; } }

        public int MeasureCount { get { return MusicTreeEditorManager.Instance.MeasureCount; } }
        public int SubtrackCount { get { return MusicTreeEditorManager.Instance.SubtrackCount; } }

        public Texture BG { get { return NoteSheetEditorWindow.configs.BgTexture; } }


        public NoteSheetDrawer(NoteSheet sheet)
        {
            this.sheet = sheet;
        }

        public void Draw()
        {
            Rect rect = new Rect(Vector2.zero, new Vector2(Width, Height));
            GUI.DrawTexture(rect, BG);
        }
    }
}
