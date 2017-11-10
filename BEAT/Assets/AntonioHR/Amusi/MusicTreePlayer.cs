using AntonioHR.Amusi.BeatSynchronization;
using AntonioHR.Amusi.Data;
using AntonioHR.Amusi.Data.Nodes;
using AntonioHR.Amusi.Internal;
using AntonioHR.Amusi.Playback;
using System;
using UnityEngine;

namespace AntonioHR.Amusi
{
    public class MusicTreePlayer : MonoBehaviour
    {
        public MusicTreeAsset musicTree;

        BeatCounter counter;
        MusicController musicController;
        NoteEventChecker checker;

        CachedMusicTree musicTreeRuntime;
        private CueMusicTreeNode nextCueNode;

        public static MusicTreePlayer Instance { get; private set; }
        public static event Action InstanceChanged;
        public event Action<CueMusicTreeNode> NewNodePlaying;
        public CueMusicTreeNode CurrentNode { get; private set; }


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



        void Awake()
        {
            Instance = this;
            if (InstanceChanged != null)
                InstanceChanged();
            Debug.Log("Initializing");
            musicController = new MusicController(GetComponents<AudioSource>());
            musicController.OnClipCloseToEnd += Controller_OnClipCloseToEnd;
            musicController.OnNewClipStart += MusicController_OnNewClipStarted;
            counter = new BeatCounter();
            musicTreeRuntime = CachedMusicTree.CreateFrom(musicTree);
            checker = new NoteEventChecker(musicTreeRuntime);

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
            if (NewNodePlaying != null)
                NewNodePlaying(nextCueNode);
            CurrentNode = nextCueNode;
        }

        private void Controller_OnClipCloseToEnd()
        {
            try
            {
                nextCueNode = musicTreeRuntime.SelectNextPatch();
                musicController.EnqueueClip(nextCueNode.clip);
            }
            catch (NoValidPatchToPlayException)
            {
                Debug.Log("No valid Patch to play");
                throw;
            }
        }
    }
}
