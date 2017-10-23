using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AntonioHR.MusicTree;
using AntonioHR.BeatFW.Internal;
using AntonioHR.BeatFW;
using AntonioHR.MusicTree.Internal;

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
        NoteEventHandler checker;

        PlayableRuntimeMusicTree musicTreeRuntime;



        #region Beat Acessors
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
        #endregion

        #region Tree Envionment Accessors
        public float GetFloatValue(string name)
        {
            return musicTreeRuntime.GetFloatValue(name);
        }
        public void SetFloatValue(string name, float newVal)
        {
            musicTreeRuntime.SetFloatValue(name, newVal);
        }

        public bool GetBoolValue(string name)
        {
            return musicTreeRuntime.GetBoolValue(name);
        }
        public void SetBoolValue(string name, bool newVal)
        {
            musicTreeRuntime.SetBoolValue(name, newVal);
        }

        public int GetIntValue(string name)
        {
            return musicTreeRuntime.GetIntValue(name);
        }
        public void SetIntValue(string name, int newVal)
        {
            musicTreeRuntime.SetIntValue(name, newVal);
        }
        #endregion

        void Start()
        {
            Debug.Log("Initializing");
            musicController = new MusicController(GetComponents<AudioSource>(), musicControllerSettings);
            musicController.OnClipCloseToEnd += controller_OnClipCloseToEnd;
            counter = new BeatCounter(beatCounterSettings, musicController);
            musicTreeRuntime = PlayableRuntimeMusicTree.CreateTreeFrom<PlayableRuntimeMusicTree>(musicTree);
            checker = new NoteEventHandler();

            double initTime = musicController.Init(musicTreeRuntime.SelectNextPatch());
            StartCoroutine(musicController.ClipCheck());
            StartCoroutine(counter.BeatCountCoroutine(initTime));
        }

        void Update()
        {
            checker.Update(counter.Progress);
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
