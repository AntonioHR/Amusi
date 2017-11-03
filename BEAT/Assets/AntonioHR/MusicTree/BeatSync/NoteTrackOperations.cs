﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.MusicTree.BeatSync
{
    public static class NoteTrackOperations
    {
        public static void CalculateTriggersBetween(this NoteTrack track, float eventStart, float eventEnd, List<NoteEvent> result)
        {
            foreach (var note in track.notes)
            {
                float noteEnd = note.End;

                bool startsBefore = eventStart < note.start;
                bool startsAfter = eventStart > noteEnd;
                bool startsInside = !startsBefore && !startsAfter;

                bool endsBefore = eventEnd < note.start;
                bool endsAfter = eventEnd > noteEnd;
                bool endsInside = !endsBefore && !endsAfter;

                
                if (startsAfter)
                    continue;

                if (startsBefore)
                {
                    if(endsAfter)
                    {
                        result.Add(new NoteEvent(NoteEvent.Type.Start, note.subTrack, 0.0f));
                        result.Add(new NoteEvent(NoteEvent.Type.End, note.subTrack, 1.0f));
                    } else if(endsInside)
                    {
                        result.Add(new NoteEvent(NoteEvent.Type.Start, note.subTrack, 0.0f));
                    }
                } else if (startsInside)
                {
                    if(endsAfter)
                    {
                        result.Add(new NoteEvent(NoteEvent.Type.End, note.subTrack, 1.0f));
                    } else if(endsInside)
                    {
                        float noteProgressBeats = (eventEnd - note.start)/note.duration;
                        result.Add(new NoteEvent(NoteEvent.Type.Update, note.subTrack, noteProgressBeats));
                    }
                }
            }
        }

        public static int SubtrackCount(this NoteTrack track)
        {
            try
            {
                return track.notes.Max(x => x.subTrack) + 1;
            }
            catch (InvalidOperationException)
            {
                return 0;
            }
            
        }

        public static bool TryAddNote(this NoteTrack track, int subtrackIndex, float pos, float duration)
        {
            var newNote = new Note() { subTrack = subtrackIndex, duration = duration, start = pos };
            var subtrack = track.NotesOnSubtrack(subtrackIndex);
            var posInSubtrack = subtrack.FindIndex(x => x.End > pos);
            posInSubtrack = posInSubtrack == -1 ? subtrack.Count : posInSubtrack;

            if(subtrack.Skip(posInSubtrack).All(x => x.start >= newNote.End))
            {
                AddNote(track, newNote);
                return true;
            }
            return false;
        }
        public static void AddNote(this NoteTrack track, Note note)
        {
            var pos = track.notes.FindIndex(x => x.End > note.End);
            if (pos == -1)
                track.notes.Add(note);
            else
                track.notes.Insert(pos, note);
        }

        public static bool IsUpdateValid(this NoteTrack track, int noteIndex, Note note)
        {
            return !track.notes.SkipIndex(noteIndex).Any(x => x.subTrack == note.subTrack && x.TimeOverlaps(note));
        }

        public static bool TryUpdate(this NoteTrack track, int noteIndex, Note note)
        {
            if(IsUpdateValid(track, noteIndex, note))
            {
                Update(track, noteIndex, note);
                return true;
            }
            return false;
        }

        private static void Update(this NoteTrack track, int noteIndex, Note note)
        {
            track.notes.RemoveAt(noteIndex);
            track.AddNote(note);
        }

        public static List<List<Note>> BySubtrack(this NoteTrack track)
        {
            int count = track.SubtrackCount();

            var result = new List<List<Note>>();

            for (int i = 0; i < count; i++)
            {
                result.Add(NotesOnSubtrack(track, i));
            }
            return result;
        }

        private static List<Note> NotesOnSubtrack(this NoteTrack track, int i)
        {
            return new List<Note>(track.notes.Where(x => x.subTrack == i));
        }
    }
}