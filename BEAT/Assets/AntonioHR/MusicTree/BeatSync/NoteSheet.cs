using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.MusicTree.BeatSync
{
    [Serializable]
    public class NoteSheet
    {
        public List<NoteTrack> tracks;
    }

    [Serializable]
    public class NoteTrack
    {
        public List<Note> notes;
        public string name;

    }
    [Serializable]
    public struct Note
    {
        public float start;
        public float duration;
        public int subTrack;

        public float End { get { return start + duration; } }
    }
}
