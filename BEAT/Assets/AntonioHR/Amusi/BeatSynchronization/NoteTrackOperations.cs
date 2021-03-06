﻿using AntonioHR.Amusi.BeatSynchronization.Internal;
using AntonioHR.Amusi.Data;
using System.Collections.Generic;

namespace AntonioHR.Amusi.BeatSynchronization
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

    }
}
