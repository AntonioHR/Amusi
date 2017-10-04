using AntonioHR.MusicTree.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR.MusicTree
{
    public class MusicTreeRuntime
    {
        private MusicTreeAsset tree;
        private CueMusicTreeNode currentlyPlayed;
        private bool playing = false;

        private MusicTreeEnvironment env;

        public MusicTreeRuntime(MusicTreeAsset tree)
        {
            this.tree = tree;
            playing = false;

            env = MusicTreeEnvironment.CreateFrom(tree.vars);
        }


        internal AudioClip SelectNextPatch()
        {
            currentlyPlayed = FindNextCue();
            return currentlyPlayed.clip;
        }

        private CueMusicTreeNode FindNextCue()
        {
            CueMusicTreeNode resultNode = null;
            if(playing)
            {
                var execStateFirstTry = currentlyPlayed.FindNext(env, out resultNode);
                if (execStateFirstTry == MusicTreeNode.ExecutionState.Running)
                {
                    currentlyPlayed = resultNode;
                    return resultNode;
                }
            }

            resultNode = TryExecuteFromStart();

            if (resultNode == null)
                throw new NoValidPatchToPlayException();

            return resultNode;
        }

        private CueMusicTreeNode TryExecuteFromStart()
        {
            CueMusicTreeNode resultNode = null;

            var executionState = tree.Root.Execute(env, out resultNode);

            playing = (executionState == MusicTreeNode.ExecutionState.Running);
            currentlyPlayed = resultNode;

            return resultNode;
        }


        public float GetFloatValue(string name)
        {
            return env.GetFloatValue(name);
        }
        public void SetFloatValue(string name, float newVal)
        {
            env.SetFloatValue(name, newVal);
        }

        public bool GetBoolValue(string name)
        {
            return env.GetBoolValue(name);
        }
        public void SetBoolValue(string name, bool newVal)
        {
            env.SetBoolValue(name, newVal);
        }

        public int GetIntValue(string name)
        {
            return env.GetIntValue(name);
        }
        public void SetIntValue(string name, int newVal)
        {
            env.SetIntValue(name, newVal);
        }
    }

    
    public class NoValidPatchToPlayException : Exception
    {

    }
}
