﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntonioHR.TreeAsset;
using AntonioHR.MusicTree.Nodes;

namespace AntonioHR.MusicTree
{
    public abstract class MusicTreeNode : TreeNodeAsset
    {
        public enum ExecutionState { Fail, Running, Done, Skipped }

        public enum ChildrenPolicy { None, Single, Multiple}
        public abstract ChildrenPolicy Policy { get; }

        public abstract ExecutionState Execute(MusicTreeEnvironment env, out CueMusicTreeNode result);
        public abstract ExecutionState ContinueExecution(MusicTreeEnvironment env, MusicTreeNode currentChild, out CueMusicTreeNode result);

    }
}