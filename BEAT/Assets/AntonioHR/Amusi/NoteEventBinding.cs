using System;
using UnityEngine;

namespace AntonioHR.Amusi
{
    [Serializable]
    public class NoteEventBinding : INoteEventListener
    {
        public string track;
        public int subtrack;

        private Action started;
        private Action<float> updated;
        private Action ended;

        MusicTreePlayer player;

        public void Init()
        {
            player = GameObject.FindObjectOfType<MusicTreePlayer>();
            player.AddListener(track, subtrack, this);
        }

        internal void Cleanup()
        {
            player.RemoveListener(track, subtrack, this);
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