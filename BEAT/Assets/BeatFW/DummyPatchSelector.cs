using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeatFW
{
    [CreateAssetMenu(menuName = "Beat/Tests/PatchSelector")]
    public class DummyPatchSelector:ScriptableObject
    {
        public AudioClip clip1;
        public AudioClip clip2;
        
        [System.NonSerialized]
        bool is1 = true;

        
        public AudioClip SelectNextPatch()
        {
            Debug.Assert(clip1 != null && clip2 != null);
            var result = (is1 = !is1) ? clip1 : clip2;
            Debug.LogFormat("Selecting {0} ", result);
            return result;
        }
    }
}
