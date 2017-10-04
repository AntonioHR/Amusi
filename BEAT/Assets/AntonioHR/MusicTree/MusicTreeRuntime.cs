﻿using AntonioHR.MusicTree.Nodes;
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
    }

    
    public class NoValidPatchToPlayException : Exception
    {

    }
}
