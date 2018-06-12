using UnityEngine;

namespace AntonioHR.Amusi
{
    public abstract class MonoDancer : MonoBehaviour
    {
        [SerializeField]
        protected NoteEventBinding binding;

        private void Start()
        {
            binding.Init();
            binding.Bind(OnNoteStart, OnNoteUpdate, OnNoteEnd);
            Init();
        }
        private void OnDestroy()
        {
            binding.Cleanup();
        }

        protected abstract void Init();

        protected abstract void OnNoteStart();
        protected abstract void OnNoteUpdate(float progress);
        protected abstract void OnNoteEnd();
        protected virtual void Cleanup() { }
    }
}
