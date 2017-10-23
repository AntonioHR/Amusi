using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.MusicTree.BeatSync
{
    public interface INoteEventListener
    {
        void OnNoteStart();
        void OnNoteUpdate(float i);
        void OnNoteEnd();
    }
}
