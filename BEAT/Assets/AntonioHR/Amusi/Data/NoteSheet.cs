using System;
using System.Collections.Generic;

namespace AntonioHR.Amusi.Data
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
        public bool Contains(float beat)
        {
            return beat >= start && beat < End;
        }

        public bool TimeOverlaps(Note other)
        {
            bool isEqualMatch = other.start == this.start && other.End == this.End;
            return !(this.End <= other.start || this.start >= other.End) || isEqualMatch;
        }
    }
}
