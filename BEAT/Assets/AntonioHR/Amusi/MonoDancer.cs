using UnityEngine;

namespace AntonioHR.Amusi
{
    public abstract class MonoDancer : MonoBehaviour
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
