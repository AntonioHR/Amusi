using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.BeatFW
{
    public class NoteEvent
    {
        public enum Type
        {
            Start, Update, End
        }

        public Type type;
        public int subTrack;
        public float progress;

        public NoteEvent(Type type, int subTrack, float progress)
        {
            this.type = type;
            this.subTrack = subTrack;
            this.progress = progress;
        }
    }
}
