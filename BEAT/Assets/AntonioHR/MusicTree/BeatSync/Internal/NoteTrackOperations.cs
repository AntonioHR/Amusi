using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.MusicTree.BeatSync.Internal
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
                

                if(endsBefore)
                    return;
                if(startsAfter)
                    continue;

                if(startsBefore)
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
            catch (InvalidOperationException e)
            {
                return 0;
            }
            
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
