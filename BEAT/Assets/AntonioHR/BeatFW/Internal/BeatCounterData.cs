using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.BeatFW.Internal
{
    static class BeatCounterData
    {
        private static readonly int[] MeasureSizes = { 3, 4 };
        public static int MeasureSize(this MeasureSignature measure)
        {
            return MeasureSizes[(int)measure];
        }
    }
    public enum MeasureSignature
    {
        THREE_FOUR = 0, FOUR_FOUR = 1
    }
    public enum BeatUnit
    {
        MEASURE, BEAT
    }
}
