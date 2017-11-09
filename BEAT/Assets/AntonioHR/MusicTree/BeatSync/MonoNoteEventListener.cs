using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AntonioHR.MusicTree.BeatSync.Internal;

namespace AntonioHR.MusicTree.BeatSync
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
