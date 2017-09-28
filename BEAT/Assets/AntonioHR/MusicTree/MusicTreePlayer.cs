using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AntonioHR.MusicTree;
using AntonioHR.BeatFW.Internal;
using AntonioHR.BeatFW;

namespace AntonioHR.MusicTree
{
    public class MusicTreePlayer : MonoBehaviour, IBeatManager
    {
        public MusicTreeAsset musicTree;
        [Space()]
        public BeatCounter.Settings beatCounterSettings;
        public MusicController.Settings musicControllerSettings;

        BeatCounter counter;
        MusicController musicController;
        MusicTreeRuntime musicTreeRuntime;



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
            musicTreeRuntime = new MusicTreeRuntime(musicTree);

            double initTime = musicController.Init(musicTreeRuntime.SelectNextPatch());
            StartCoroutine(musicController.ClipCheck());
            StartCoroutine(counter.BeatCountCoroutine(initTime));
        }

        void controller_OnClipCloseToEnd()
        {
            try
            {
                var newPatch = musicTreeRuntime.SelectNextPatch();
                musicController.EnqueuePatch(newPatch);
            }
            catch (NoValidPatchToPlayException e)
            {
                Debug.Log("No valid Patch to play");
                throw;
            }
        }

    }
}
