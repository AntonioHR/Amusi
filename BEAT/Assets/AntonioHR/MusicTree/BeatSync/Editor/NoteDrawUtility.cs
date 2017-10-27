using AntonioHR.MusicTree.BeatSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR.MusicTree.BeatSync.Editor
{
    public static class NoteDrawUtility
    {
        public static void NoteOnCurrentTrack(Note note)
        {
            var bounds = note.Bounds();

            GUI.DrawTexture(bounds, CurrentNoteTexture);


            if (GUI.Button(bounds, GUIContent.none, GUIStyle.none))
            {
                NoteTrackDrawer.SelectCursor();
            }
        }

        public static Texture CurrentNoteTexture { get { return NoteTrackDrawer.IsDrawingSelection ? NoteTrackDrawer.NoteTextureSelected : NoteTrackDrawer.NoteTextureIdle; } }

        private static Rect Bounds(this Note note)
        {
            int width = (int)(note.duration * NoteTrackDrawer.BeatWidth);
            Vector2 start = NoteTrackDrawer.drawCursor + Vector2.right * (NoteTrackDrawer.BeatWidth * note.start);
            return new Rect(start, new Vector2(width, NoteTrackDrawer.SubtrackHeight));
        }
    }
}
