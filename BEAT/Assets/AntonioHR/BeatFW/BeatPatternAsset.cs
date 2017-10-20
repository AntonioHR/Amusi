using AntonioHR.BeatFW.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR.BeatFW
{
    /// <summary>
    /// Stores the data of a Beat Pattern
    /// </summary>
    [CreateAssetMenu()]
    public class BeatPatternAsset : ScriptableObject
    {
        public BeatPattern pattern;
    }
}
