using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AntonioHR.MusicTree;
using AntonioHR.MusicTree.BeatSync.Internal;
using AntonioHR.MusicTree.BeatSync;
using AntonioHR.MusicTree.Internal;
using AntonioHR.MusicTree.Nodes;

namespace AntonioHR.MusicTree
{
    public class MusicTreePlayer : MonoBehaviour
    {
        public MusicTreeAsset musicTree;
        [Space()]
        public MusicController.Settings musicControllerSettings;

        BeatCounter counter;
        MusicController musicController;
        NoteEventManager checker;

        PlayableRuntimeMusicTree musicTreeRuntime;
        private CueMusicTreeNode nextCueNode;


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

        public void AddListener(string track, int subTrack, INoteEventListener listener)
        {
            checker.AddListener(track, subTrack, listener);
        }

        public void RemoveListener(string track, int subTrack, INoteEventListener listener)
        {
            checker.RemoveListener(track, subTrack, listener);
        }



        void Start()
        {
            Debug.Log("Initializing");
            musicController = new MusicController(GetComponents<AudioSource>(), musicControllerSettings);
            musicController.OnClipCloseToEnd += Controller_OnClipCloseToEnd;
            musicController.OnNewClipStart += MusicController_OnNewClipStarted;
            counter = new BeatCounter();
            musicTreeRuntime = PlayableRuntimeMusicTree.CreateFrom(musicTree);
            checker = new NoteEventManager(musicTreeRuntime);

            nextCueNode = musicTreeRuntime.SelectNextPatch();
            double initTime = musicController.Init(nextCueNode.clip);
            Debug.LogFormat("init: {0}", initTime);
            counter.UpdateClipVariables(initTime, musicTree.defaultBPM, musicController.Frequency);
        }


        void Update()
        {
            musicController.Step();
            if (musicController.HasStarted)
            {
                counter.Step();
                checker.PerformChecks((float)counter.Progress);
            }
        }


        private void MusicController_OnNewClipStarted()
        {
            float bpm = MusicTreeNodeUtilities.BPMFor(nextCueNode, musicTree);
            counter.UpdateClipVariables(musicController.CurrentClipStartDSPTme, bpm, musicController.Frequency);
            checker.SwitchCue(nextCueNode);
        }

        private void Controller_OnClipCloseToEnd()
        {
            try
            {
                nextCueNode = musicTreeRuntime.SelectNextPatch();
                musicController.EnqueueClip(nextCueNode.clip);
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (NoValidPatchToPlayException e)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                Debug.Log("No valid Patch to play");
                throw;
            }
        }

    }
}
