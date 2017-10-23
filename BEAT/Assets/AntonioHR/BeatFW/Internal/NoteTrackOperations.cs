using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.BeatFW.Internal
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

                //UnityEngine.Debug.LogFormat("Start: {6} {0}, {1}, {2}\n End: {7} {3}, {4}, {5}", startsBefore, startsInside, startsAfter, endsBefore, endsInside, endsAfter, eventStart, eventEnd);

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
                        float noteProgressBeats = eventEnd - note.start;
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
