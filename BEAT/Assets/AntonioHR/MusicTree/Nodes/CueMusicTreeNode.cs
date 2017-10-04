using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR.MusicTree.Nodes
{
    public class CueMusicTreeNode : MusicTreeNode
    {
        public override ChildrenPolicy Policy
        {
            get { return ChildrenPolicy.None; }
        }
        public AudioClip clip;
        public BeatFW.BeatPatternAsset pattern;


        public MusicTreeNode.ExecutionState FindNext(MusicTreeEnvironment env, out CueMusicTreeNode result)
        {
            return ((MusicTreeNode)Parent).ContinueExecution(env, this, out result);
        }

        public override MusicTreeNode.ExecutionState Execute(MusicTreeEnvironment env, out CueMusicTreeNode result)
        {
            result = this;
            return ExecutionState.Running;
        }

        public override MusicTreeNode.ExecutionState ContinueExecution(MusicTreeEnvironment env, MusicTreeNode currentChild, out CueMusicTreeNode result)
        {
            throw new InvalidOperationException();
        }
    }
}
