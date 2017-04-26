using BeatFW.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeatFW
{
    /// <summary>
    /// Stores the data of a Beat Pattern
    /// </summary>
    [CreateAssetMenu()]
    public class BeatPattern : ScriptableObject
    {

        [System.Serializable]
        public class Note : IComparable<Note>
        {
            [SerializeField]
            private float timeInBeats;

            public float TimeInBeats { get { return timeInBeats; } set { timeInBeats = value; } }



            public int CompareTo(Note other)
            {
                return timeInBeats.CompareTo(other.timeInBeats);
            }
        }



        [SerializeField]
        private int beatsPerMeasure = 4;
        [SerializeField]
        private int measureCount = 1;
        [SerializeField]
        private List<Note> notes;

        public float[] Notes
        {
            get
            {
                return notes.Select(x => { return x.TimeInBeats; }).ToArray();
            }
        }

        private bool sorted = false;
        



        public int BeatsPerMeasure { get { return beatsPerMeasure; } set { beatsPerMeasure = value; } }
        public int MeasureCount { get { return measureCount; } set { measureCount = value; } }

        public Note this[int index] 
        { 
            get 
            {
                return GetNoteAtIndex(index);
            } 
        }




        public int NotesBetween(float start, float end)
        {
            return NotesUntil(end) - NotesUntil(start);
        }

        public Note GetNoteAtIndex(int index)
        {
            CheckSort();
            return notes[index]; 
        }
        
        public void AddNoteOn(int count, float unit = .25f)
        {
            var note =new Note();
            note.TimeInBeats = count * unit;
            notes.Add(note);
        }
        public void RemoveNoteOn(int count, float unit = .25f)
        {
            notes.RemoveAll(x => x.TimeInBeats == count * unit);
        }
        public bool HasNoteOn(int count, float unit = .25f)
        {
            return notes.Any(x => x.TimeInBeats == count * unit);
        }
        
        public int GetSize(float unit = .25f)
        {
            return Mathf.RoundToInt(measureCount * beatsPerMeasure / unit );
        }


        private void CheckSort()
        {
            if (!sorted)
            {
                notes.Sort();
            }
        }
        
        private int NotesUntil(float progress)
        {
            CheckSort();
            int beatsPerPattern = beatsPerMeasure * measureCount;
            int notesPerPattern = notes.Count;
            float progressInCurrentPattern = progress % beatsPerPattern;
            int completePatterns = Mathf.RoundToInt(progress - progressInCurrentPattern) / beatsPerPattern;
            var v1 = completePatterns * notesPerPattern;
            int idx =  notes.FindIndex((x) => x.TimeInBeats >= progressInCurrentPattern);
            var v2 = idx == -1 ? notesPerPattern : idx;
            //Debug.LogFormat("({0:00.00}, {2} + {1:00.00}) ({3} + {4} ({5}))", progress, progressInCurrentPattern, completePatterns, v1, v2, idx);
            return v1 + v2;
        }
    }
}
