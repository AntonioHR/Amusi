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
        public MusicController musicController;
        [Space()]
        public BeatCounter.Settings beatCounterSettings;

        private BeatCounter counter;

        void Start()
        {
            Debug.Log("Initializing");
            musicController.OnClipCloseToEnd += controller_OnClipCloseToEnd;
            counter = new BeatCounter(beatCounterSettings, musicController);

            double initTime = musicController.Init(selector.SelectNextPatch());
            StartCoroutine(musicController.ClipCheck());
            StartCoroutine(counter.BeatCountCoroutine(initTime));
        }

        void controller_OnClipCloseToEnd(object sender, ClipEventArgs e)
        {
            musicController.EnqueuePatch(selector.SelectNextPatch());
        }
    }
}
