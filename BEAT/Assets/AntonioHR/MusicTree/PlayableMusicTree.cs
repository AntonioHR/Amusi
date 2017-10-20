using AntonioHR.TreeAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntonioHR.TreeAsset.Internal;
using AntonioHR.MusicTree.Nodes;
using AntonioHR.MusicTree.Internal;

namespace AntonioHR.MusicTree
{
    public class PlayableMusicTree : RuntimeTree<MusicTreeAsset, MusicTreeNode, PlayableMusicTreeNode>
    {

        protected override void AfterInit()
        {
            env = MusicTreeEnvironment.CreateFrom(Asset.vars);
        }

        private CueMusicTreeNode currentlyPlayedNode;
        private MusicTreeEnvironment env;

        internal UnityEngine.AudioClip SelectNextPatch()
        {
            FindNextCue();

            if (currentlyPlayedNode == null)
                throw new NoValidPatchToPlayException();

            return currentlyPlayedNode.clip;
        }

        private void FindNextCue()
        {
            var result = Root.Execute(env, out currentlyPlayedNode);

            if (result != PlayableMusicTreeNode.State.Running)
            {
                //Second Try
                result = Root.Execute(env, out currentlyPlayedNode);
            }
        }
        #region Enviornment Accessors
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
        #endregion
    }

    public class PlayableMusicTreeNode : RuntimeTreeNode<MusicTreeNode, PlayableMusicTreeNode>
    {
        internal bool isRunning { get { return ExecutionState == State.Running; } }

        public enum State { Idle, Running, Failed, Complete }
        public State ExecutionState { get; set; }
        public PlayableMusicTreeNode ActiveChild { get; set; }

        public void Accept(MusicNodeVisitor n)
        {
            Asset.Accept(n, this);
        }
    }

    public class NoValidPatchToPlayException : Exception
    {

    }
}
