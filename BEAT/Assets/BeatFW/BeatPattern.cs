using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeatFW
{
    /// <summary>
    /// Stores the data of a Beat Pattern
    /// </summary>
    [System.Serializable]
    public class BeatPattern
    {
        public int BeatsPerMeasure = 4;

        public int Measures = 1;

        public List<Note> notes;

        public class Note
        {
            public float timeInBeats;
        }
    }
}
