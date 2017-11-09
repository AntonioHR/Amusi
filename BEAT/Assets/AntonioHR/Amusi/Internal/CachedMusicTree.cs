using AntonioHR.TreeAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AntonioHR.Amusi.Data;
using AntonioHR.Amusi.Data.Nodes;
using AntonioHR.Amusi.Playback;

namespace AntonioHR.Amusi.Internal
{
    public class CachedMusicTree : 
        CachedTree<CachedMusicTree, MusicTreeAsset, MusicTreeNode, CachedMusicTreeNode>
    {

        private CueMusicTreeNode currentlyPlayedNode;
        private MusicTreeEnvironment env;

        public CueMusicTreeNode CurrentlyPlayedNode { get { return currentlyPlayedNode; } }

        public IEnumerable<CueMusicTreeNode> AllCues { get { return Root.Preorder().Where(x => x.Asset is CueMusicTreeNode).Select(x=>x.Asset as CueMusicTreeNode); } }

        public int MaxSubTrack {
            get
            {
                if (AllCues.Any())
                    return AllCues.Max(
                        cue => cue.sheet.tracks.Count == 0? 0:cue.sheet.tracks.Max(
                            track =>
                            {
                                if (track.notes.Any())
                                    return track.notes.Max(
                                        note => note.subTrack);
                                else
                                    return 0;
                            }));
                else return 0;
            }
        }

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

            if (result != CachedMusicTreeNode.State.Running)
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

    public class CachedMusicTreeNode : CachedTreeNode<CachedMusicTree, MusicTreeAsset, MusicTreeNode, CachedMusicTreeNode>
    {
        internal bool isRunning { get { return ExecutionState == State.Running; } }

        public bool IsDescendantOf(CachedMusicTreeNode other)
        {
            if (IsRoot)
                return false;
            else if (Parent == other)
                return true;
            else
                return Parent.IsDescendantOf(other);
        }

        public CachedMusicTree TreeAsPlayable { get { return Tree; } }

        public int BPM
        {
            get
            {
                var cueNode = Asset as CueMusicTreeNode;
                return cueNode == null ? 0 : MusicTreeNodeUtilities.BPMFor(cueNode, Tree.Asset);
            }
        }

        public bool CanBeParentOf(CachedMusicTreeNode other)
        {
            if (this.IsDescendantOf(other))
                return false;
            else
                return AllowsMoreChildren;
        }
        public bool AllowsMoreChildren
        {
            get
            {
                switch (Asset.Policy)
                {
                    case MusicTreeNode.ChildrenPolicy.None:
                        return false;
                    case MusicTreeNode.ChildrenPolicy.Single:
                        return ChildCount == 0;
                    case MusicTreeNode.ChildrenPolicy.Multiple:
                        return true;
                    default:
                        throw new NotImplementedException();
                }
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
        public CachedMusicTreeNode ActiveChild { get; set; }

        public void Accept(MusicNodeVisitor n)
        {
            Asset.Accept(n, this);
        }
    }

    public class NoValidPatchToPlayException : Exception
    {

    }
}
