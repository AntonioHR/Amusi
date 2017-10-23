using AntonioHR.MusicTree.BeatSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace AntonioHR.MusicTree.BeatSync.Editor
{
    public class NoteTrackDrawer
    {
        public NoteTrack track;

        public int beatsPerMeasure;

        public int subTrackCount;
        public int measureCount;

        public NoteTrackEditorConfigs configs;
        

        public int Height { get { return configs.subTrackHeight * subTrackCount; } }

        public int Width { get { return BeatCount * configs.beatWidth; } }
        public int BeatCount { get { return beatsPerMeasure * measureCount; } }


        public NoteTrackDrawer(NoteTrack track, int beatsPerMeasure)
        {

        }

        public void Draw()
        {
            Rect rect = new Rect(Vector2.zero, new Vector2(Width, Height));
            GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture);
        }
    }
}
