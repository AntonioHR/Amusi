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
        #region Inspector-Exposed Parameters
        [SerializeField]
        private MusicTreeAsset musicTree;
        #endregion

        AmusiEngine engine;

        

        #region Tree Envionment Accessors
        public float GetFloatValue(string name)
        {
            return engine.GetFloatValue(name);
        }
        public void SetFloatValue(string name, float newVal)
        {
            engine.SetFloatValue(name, newVal);
        }

        public bool GetBoolValue(string name)
        {
            return engine.GetBoolValue(name);
        }
        public void SetBoolValue(string name, bool newVal)
        {
            engine.SetBoolValue(name, newVal);
        }

        public int GetIntValue(string name)
        {
            return engine.GetIntValue(name);
        }
        public void SetIntValue(string name, int newVal)
        {
            engine.SetIntValue(name, newVal);
        }
        #endregion

        #region EventChecker Acessors
        public void AddListener(string track, int subTrack, INoteEventListener listener)
        {
            engine.AddListener(track, subTrack, listener);
        }

        public void RemoveListener(string track, int subTrack, INoteEventListener listener)
        {
            engine.RemoveListener(track, subTrack, listener);
        }
        #endregion




        #region Unity Messages
        void Awake()
        {
            engine = new AmusiEngine(musicTree, GetComponents<AudioSource>());
        }


        void Update()
        {
            engine.Step();
        }
        #endregion

    }
}
