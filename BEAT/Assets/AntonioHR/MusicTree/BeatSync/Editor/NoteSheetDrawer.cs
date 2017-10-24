using AntonioHR.MusicTree.BeatSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace AntonioHR.MusicTree.BeatSync.Editor
{
    public class NoteSheetDrawer
    {
        public NoteSheet sheet;

        public int beatsPerMeasure;

        public int subTrackCount;
        public int measureCount;
        

        public int Height { get { return NoteSheetEditorWindow.configs.SubTrackHeight * subTrackCount; } }

        public int Width { get { return BeatCount * NoteSheetEditorWindow.configs.BeatWidth; } }
        public int BeatCount { get { return beatsPerMeasure * measureCount; } }

        public Texture BG { get { return NoteSheetEditorWindow.configs.BgTexture; } }


        public NoteSheetDrawer(NoteSheet sheet, int beatsPerMeasure)
        {
            this.sheet = sheet;
            this.beatsPerMeasure = beatsPerMeasure;
            measureCount = 3;
            subTrackCount = 2;
        }

        public void Draw()
        {
            Rect rect = new Rect(Vector2.zero, new Vector2(Width, Height));
            GUI.DrawTexture(rect, BG);
        }
    }
}
