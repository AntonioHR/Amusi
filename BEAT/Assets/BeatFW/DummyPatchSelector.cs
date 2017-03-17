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
        public AudioClip clip;

        public AudioClip SelectNextPatch()
        {
            Debug.Assert(clip != null);
            return clip;
        }
    }
}
