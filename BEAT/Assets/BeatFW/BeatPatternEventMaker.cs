using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace BeatFW
{
    class BeatPatternEventMaker : BeatPatternBaseEventMaker
    {

        public UnityEvent OnNoteEvent;

        public override void OnNote()
        {
            OnNoteEvent.Invoke();
        }
    }
}
