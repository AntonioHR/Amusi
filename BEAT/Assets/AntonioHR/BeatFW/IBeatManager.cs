using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.BeatFW
{
    public interface IBeatManager
    {
        float BeatProgressFull { get; }
        int CompletedBeats { get; }


        float GetBeatProgress();
        float GetBeatProgress(int beat, float max);
    }
}
