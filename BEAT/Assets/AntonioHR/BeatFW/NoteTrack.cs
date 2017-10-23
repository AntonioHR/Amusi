using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.BeatFW
{
    [Serializable]
    public class NoteTrack
    {
        public List<Note> notes;
        public string name;
    }
    [Serializable]
    public class Note
    {
        public float start;
        public float duration;
        public int subTrack;
    }
}
