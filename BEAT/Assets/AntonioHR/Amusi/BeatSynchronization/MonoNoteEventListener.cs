﻿using AntonioHR.Amusi.BeatSynchronization.Internal;
using UnityEngine;

namespace AntonioHR.Amusi.BeatSynchronization
{
    public abstract class MonoNoteEventListener : MonoBehaviour
    {
        [SerializeField]
        private NoteEventBinding binding;

        private void Start()
        {
            binding.Init();
            binding.Bind(OnNoteStart, OnNoteUpdate, OnNoteEnd);
            Init();
        }

        protected abstract void Init();

        protected abstract void OnNoteStart();
        protected abstract void OnNoteUpdate(float progress);
        protected abstract void OnNoteEnd();
    }
}
