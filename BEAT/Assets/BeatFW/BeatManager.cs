using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeatFW
{
    public class BeatManager:MonoBehaviour
    {
        public DummyPatchSelector selector;
        public BeatMusicController controller;

        void Start()
        {
            Debug.Log("Initializing");
            controller.Init(selector.SelectNextPatch());
            controller.OnClipCloseToEnd += controller_OnClipCloseToEnd;
        }

        void controller_OnClipCloseToEnd(object sender, ClipEventArgs e)
        {
            controller.EnqueuePatch(selector.SelectNextPatch());
        }
    }
}
