namespace AntonioHR.Amusi
{
    public interface INoteEventListener
    {
        void OnNoteStart();
        void OnNoteUpdate(float i);
        void OnNoteEnd();
    }
}
