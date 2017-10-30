using AntonioHR.TreeAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntonioHR.TreeAsset.Internal;
using AntonioHR.MusicTree.Nodes;
using AntonioHR.MusicTree.Internal;

namespace AntonioHR.MusicTree.Internal
{
    public class PlayableRuntimeMusicTree : 
        RuntimeTree<PlayableRuntimeMusicTree, MusicTreeAsset, MusicTreeNode, PlayableRuntimeMusicTreeNode>
    {

        private CueMusicTreeNode currentlyPlayedNode;
        private MusicTreeEnvironment env;

        public CueMusicTreeNode CurrentlyPlayedNode { get { return currentlyPlayedNode; } }
        

        protected override void AfterInit()
        {
            env = MusicTreeEnvironment.CreateFrom(Asset.vars);
        }



        internal CueMusicTreeNode SelectNextPatch()
        {
            FindNextCue();

            if (currentlyPlayedNode == null)
                throw new NoValidPatchToPlayException();

            return currentlyPlayedNode;
        }

        private void FindNextCue()
        {
            var result = Root.Execute(env, out currentlyPlayedNode);

            if (result != PlayableRuntimeMusicTreeNode.State.Running)
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

    public class PlayableRuntimeMusicTreeNode : RuntimeTreeNode<PlayableRuntimeMusicTree, MusicTreeAsset, MusicTreeNode, PlayableRuntimeMusicTreeNode>
    {
        internal bool isRunning { get { return ExecutionState == State.Running; } }

        public PlayableRuntimeMusicTree TreeAsPlayable { get { return Tree; } }

        public int BPM
        {
            get
            {
                var cueNode = Asset as CueMusicTreeNode;
                return cueNode == null ? 0 : MusicTreeNodeUtilities.BPMFor(cueNode, Tree.Asset);
            }
        }
        

        public float LengthInBeats
        {
            get
            {
                var cueNode = Asset as CueMusicTreeNode;
                return cueNode == null ? 0 : MusicTreeNodeUtilities.DurationInBeats(cueNode, Tree.Asset);
            }
        }

        public enum State { Idle, Running, Failed, Complete }
        public State ExecutionState { get; set; }
        public PlayableRuntimeMusicTreeNode ActiveChild { get; set; }

        public void Accept(MusicNodeVisitor n)
        {
            Asset.Accept(n, this);
        }
    }

    public class NoValidPatchToPlayException : Exception
    {

    }
}
