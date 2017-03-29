using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeatFW.Engine;

namespace BeatFW
{
    public class BeatManager:MonoBehaviour
    {
        public DummyPatchSelector selector;
        [Space()]
        public BeatCounter.Settings beatCounterSettings;
        public MusicController.Settings musicControllerSettings;

        BeatCounter counter;
        MusicController musicController;



        public float BeatProgressFull { get { return counter.GetFullProgress(); } }
        public int CompletedBeats { get { return counter.CompletedBeats; } }


        public float GetBeatProgress()
        {
            return counter.GetBeatProgress();
        }
        public float GetBeatProgress(int beat, float max = float.PositiveInfinity)
        {
            return counter.GetBeatProgress(beat, max);
        }



        void Start()
        {
            Debug.Log("Initializing");
            musicController = new MusicController(GetComponents<AudioSource>(), musicControllerSettings);
            musicController.OnClipCloseToEnd += controller_OnClipCloseToEnd;
            counter = new BeatCounter(beatCounterSettings, musicController);

            double initTime = musicController.Init(selector.SelectNextPatch());
            StartCoroutine(musicController.ClipCheck());
            StartCoroutine(counter.BeatCountCoroutine(initTime));
        }

        void controller_OnClipCloseToEnd()
        {
            musicController.EnqueuePatch(selector.SelectNextPatch());
        }

    }
}
