﻿using System;
using UnityEngine;

namespace AntonioHR.Amusi.BeatSynchronization.Internal
{
    [Serializable]
    public class NoteEventBinding : INoteEventListener
    {
        public string track;
        public int subtrack;

        private Action started;
        private Action<float> updated;
        private Action ended;


        public void Init()
        {
            MusicTreePlayer player = GameObject.FindObjectOfType<MusicTreePlayer>();
            player.AddListener(track, subtrack, this);
        }

        public void Bind(Action started, Action<float> updated, Action ended)
        {
            this.started = started;
            this.updated = updated;
            this.ended = ended;
        }

        public void OnNoteStart()
        {
            started();
        }
        public void OnNoteUpdate(float i)
        {
            updated(i);
        }
        public void OnNoteEnd()
        {
            ended();
        }
    }
}