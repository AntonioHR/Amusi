using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR.Amusi
{
    public abstract class MultiMonoDancer : MonoBehaviour
    {
        [SerializeField]
        protected NoteEventBinding[] bindings;

        private void Start()
        {
            for (int i = 0; i < bindings.Length; i++)
            {
                int indx = i;
                bindings[i].Init();
                bindings[i].Bind(()=>OnNoteStart(indx), (f)=>OnNoteUpdate(indx,f) , ()=>OnNoteEnd(indx));
                Init();
            }
        }
        private void OnDestroy()
        {
            foreach (var binding in bindings)
            {
                binding.Cleanup();
            }
        }

        protected abstract void Init();

        protected abstract void OnNoteStart(int i);
        protected abstract void OnNoteUpdate(int i, float progress);
        protected abstract void OnNoteEnd(int i);
        protected virtual void Cleanup() { }
    }
}
