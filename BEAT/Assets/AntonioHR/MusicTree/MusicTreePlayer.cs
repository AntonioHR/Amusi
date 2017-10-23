using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AntonioHR.MusicTree;
using AntonioHR.BeatFW.Internal;
using AntonioHR.BeatFW;
using AntonioHR.MusicTree.Internal;
using AntonioHR.MusicTree.Nodes;

namespace AntonioHR.MusicTree
{
    public class MusicTreePlayer : MonoBehaviour
    {
        public MusicTreeAsset musicTree;
        [Space()]
        public BeatCounter.Settings beatCounterSettings;
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

        void Start()
        {
            Debug.Log("Initializing");
            musicController = new MusicController(GetComponents<AudioSource>(), musicControllerSettings);
            musicController.OnClipCloseToEnd += Controller_OnClipCloseToEnd;
            musicController.OnNewClipStart += MusicController_OnNewClipStarted;
            counter = new BeatCounter(beatCounterSettings);
            musicTreeRuntime = PlayableRuntimeMusicTree.CreateTreeFrom<PlayableRuntimeMusicTree>(musicTree);
            checker = new NoteEventManager(musicTree);

            nextCueNode = musicTreeRuntime.SelectNextPatch();
            double initTime = musicController.Init(nextCueNode.clip);
            counter.UpdateClipVariables(musicController.CurrentClipStartDSPTme, musicController.BPM, musicController.Frequency);
        }


        void Update()
        {
            musicController.Step();
            counter.Step();
            checker.PerformChecks(counter.Progress);
        }


        private void MusicController_OnNewClipStarted()
        {
            counter.UpdateClipVariables(musicController.CurrentClipStartDSPTme, musicController.BPM, musicController.Frequency);
            checker.SwitchCue(nextCueNode);
        }

        private void Controller_OnClipCloseToEnd()
        {
            try
            {
                nextCueNode = musicTreeRuntime.SelectNextPatch();
                musicController.EnqueueClip(nextCueNode.clip);
            }
            catch (NoValidPatchToPlayException e)
            {
                Debug.Log("No valid Patch to play");
                throw;
            }
        }

    }
}
