using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntonioHR.MusicTree.Nodes;
using AntonioHR.TreeAsset.Internal;

namespace AntonioHR.MusicTree.Internal
{
    public class ExecutionStepNodeVisitor : MusicNodeVisitor
    {
        public CueMusicTreeNode RunningLeaf { get; private set; }

        public MusicTreeEnvironment Environment { get; private set; }
        

        public ExecutionStepNodeVisitor(MusicTreeEnvironment environment)
        {
            this.Environment = environment;
        }




        public void Visit(SelectorMusicTreeNode n, PlayableRuntimeMusicTreeNode nContainer)
        {

            PlayableRuntimeMusicTreeNode first = nContainer.isRunning? nContainer.ActiveChild : nContainer.LeftmostChild;

            foreach (var ch in nContainer.ChildrenStartingAt(first))
            {
                ch.Accept(this);

                switch (ch.ExecutionState)
                {
                    case PlayableRuntimeMusicTreeNode.State.Running:
                        nContainer.ExecutionState = PlayableRuntimeMusicTreeNode.State.Running;
                        nContainer.ActiveChild = ch;
                        return;
                    case PlayableRuntimeMusicTreeNode.State.Idle:
                        throw new Exception("Child should not have set itself to idle");
                    case PlayableRuntimeMusicTreeNode.State.Failed:
                        continue;
                    case PlayableRuntimeMusicTreeNode.State.Complete:
                        nContainer.ExecutionState = PlayableRuntimeMusicTreeNode.State.Complete;
                        nContainer.ActiveChild = ch;
                        return;
                    default:
                        throw new NotImplementedException();
                }
            }

            nContainer.ExecutionState = PlayableRuntimeMusicTreeNode.State.Failed;
            
        }

        public void Visit(SequenceMusicTreeNode n, PlayableRuntimeMusicTreeNode nContainer)
        {
            PlayableRuntimeMusicTreeNode first = nContainer.isRunning ? nContainer.ActiveChild : nContainer.LeftmostChild;

            foreach (var ch in nContainer.ChildrenStartingAt(first))
            {
                ch.Accept(this);

                switch (ch.ExecutionState)
                {
                    case PlayableRuntimeMusicTreeNode.State.Running:
                        nContainer.ExecutionState = PlayableRuntimeMusicTreeNode.State.Running;
                        nContainer.ActiveChild = ch;
                        return;
                    case PlayableRuntimeMusicTreeNode.State.Idle:
                        throw new Exception("Child should not have set itself to idle");
                    case PlayableRuntimeMusicTreeNode.State.Failed:
                        nContainer.ExecutionState = PlayableRuntimeMusicTreeNode.State.Failed;
                        return;
                    case PlayableRuntimeMusicTreeNode.State.Complete:
                        continue;
                    default:
                        throw new NotImplementedException();
                }
            }

            nContainer.ExecutionState = PlayableRuntimeMusicTreeNode.State.Complete;
            nContainer.ActiveChild = nContainer.RightmostChild;
        }

        public void Visit(ConditionMusicTreeNode n, PlayableRuntimeMusicTreeNode nContainer)
        {
            var child = nContainer.LeftmostChild;
            
            if(nContainer.isRunning || Environment.Evaluate(n.condition))
            {
                child.Accept(this);
                nContainer.ExecutionState = child.ExecutionState;
                nContainer.ActiveChild = child;
            } else
            {
                nContainer.ExecutionState = PlayableRuntimeMusicTreeNode.State.Failed;
            }
        }

        public void Visit(CueMusicTreeNode n, PlayableRuntimeMusicTreeNode nContainer)
        {
            if(nContainer.ExecutionState == PlayableRuntimeMusicTreeNode.State.Running)
            {
                //If was already playing, it is assumed to be finished
                nContainer.ExecutionState = PlayableRuntimeMusicTreeNode.State.Complete;
                this.RunningLeaf = null;
            } else 
            {
                nContainer.ExecutionState = PlayableRuntimeMusicTreeNode.State.Running;
                this.RunningLeaf = n;
            }
        }
    }
}
