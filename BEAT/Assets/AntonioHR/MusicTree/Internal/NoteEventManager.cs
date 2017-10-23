﻿using AntonioHR.BeatFW;
using AntonioHR.BeatFW.Internal;
using AntonioHR.MusicTree.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.MusicTree.Internal
{
    public class NoteEventManager
    {

        private float lastTime;
        private List<INoteEventListener>[,] eventListeners;
        private Dictionary<string, int> trackIds;
        


        public CueMusicTreeNode currentCue;


        public NoteEventManager(MusicTreeAsset asset)
        {
            int trackCount = asset.trackDefinitions.Count;
            int subTrackCount = asset.MaxSubTrack;
            eventListeners = new List<INoteEventListener>[trackCount, subTrackCount];

            for (int i = 0; i < trackCount; i++)
            {
                for (int j = 0; j < subTrackCount; j++)
                {
                    eventListeners[i, j] = new List<INoteEventListener>();
                }
            }

            trackIds = new Dictionary<string, int>();
            for (int i = 0; i < asset.trackDefinitions.Count; i++)
            {
                trackIds.Add(asset.trackDefinitions[i].name, i);
            }
        }


        public void AddListener(string track, int subTrack, INoteEventListener listener)
        {
            eventListeners[TrackIndex(track), subTrack].Add(listener);
        }
        
        public void RemoveListener(string track, int subTrack, INoteEventListener listener)
        {
            eventListeners[TrackIndex(track), subTrack].Remove(listener);
        }


        private int TrackIndex(string track)
        {
            return trackIds[track];
        }

        public void SwitchCue(CueMusicTreeNode newCue)
        {
            PerformChecks(float.PositiveInfinity);
            currentCue = newCue;
            lastTime = float.NegativeInfinity;
        }

        

        public void PerformChecks(float currentTime)
        {
            if (currentCue != null)
            {
                List<NoteEvent> events = new List<NoteEvent>();
                for (int i = 0; i < currentCue.Tracks.Count; i++)
                {
                    events.Clear();
                    currentCue.Tracks[i].CalculateTriggersBetween(lastTime, currentTime, events);
                    TriggerEvents(i, events);
                }
            }
            lastTime = currentTime;
        }

        void TriggerEvents(int track, List<NoteEvent> events)
        {
            foreach (var ev in events)
            {
                int subtrack = ev.subTrack;

                foreach (var listener in eventListeners[track, subtrack])
                {
                    TriggerEvent(listener, ev);
                }
            }
        }

        void TriggerEvent(INoteEventListener listener, NoteEvent ev)
        {
            switch (ev.type)
            {
                case NoteEvent.Type.Start:
                    listener.OnNoteStart();
                    break;
                case NoteEvent.Type.Update:
                    listener.OnNoteUpdate(ev.progress);
                    break;
                case NoteEvent.Type.End:
                    listener.OnNoteEnd();
                    break;
                default:
                    break;
            }
        }
    }
}
