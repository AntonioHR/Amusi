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




        public void Visit(SelectorMusicTreeNode n, PlayableMusicTreeNode nContainer)
        {

            PlayableMusicTreeNode first = nContainer.isRunning? nContainer.ActiveChild : nContainer.LeftmostChild;

            foreach (var ch in nContainer.ChildrenStartingAt(first))
            {
                ch.Accept(this);

                switch (ch.ExecutionState)
                {
                    case PlayableMusicTreeNode.State.Running:
                        nContainer.ExecutionState = PlayableMusicTreeNode.State.Running;
                        nContainer.ActiveChild = ch;
                        return;
                    case PlayableMusicTreeNode.State.Idle:
                        throw new Exception("Child should not have set itself to idle");
                    case PlayableMusicTreeNode.State.Failed:
                        continue;
                    case PlayableMusicTreeNode.State.Complete:
                        nContainer.ExecutionState = PlayableMusicTreeNode.State.Complete;
                        nContainer.ActiveChild = ch;
                        return;
                    default:
                        throw new NotImplementedException();
                }
            }

            nContainer.ExecutionState = PlayableMusicTreeNode.State.Failed;
            
        }

        public void Visit(SequenceMusicTreeNode n, PlayableMusicTreeNode nContainer)
        {
            PlayableMusicTreeNode first = nContainer.isRunning ? nContainer.ActiveChild : nContainer.LeftmostChild;

            foreach (var ch in nContainer.ChildrenStartingAt(first))
            {
                ch.Accept(this);

                switch (ch.ExecutionState)
                {
                    case PlayableMusicTreeNode.State.Running:
                        nContainer.ExecutionState = PlayableMusicTreeNode.State.Running;
                        nContainer.ActiveChild = ch;
                        return;
                    case PlayableMusicTreeNode.State.Idle:
                        throw new Exception("Child should not have set itself to idle");
                    case PlayableMusicTreeNode.State.Failed:
                        nContainer.ExecutionState = PlayableMusicTreeNode.State.Failed;
                        return;
                    case PlayableMusicTreeNode.State.Complete:
                        continue;
                    default:
                        throw new NotImplementedException();
                }
            }

            nContainer.ExecutionState = PlayableMusicTreeNode.State.Complete;
            nContainer.ActiveChild = nContainer.RightmostChild;
        }

        public void Visit(ConditionMusicTreeNode n, PlayableMusicTreeNode nContainer)
        {
            var child = nContainer.LeftmostChild;
            
            if(nContainer.isRunning || Environment.Evaluate(n.condition))
            {
                child.Accept(this);
                nContainer.ExecutionState = child.ExecutionState;
                nContainer.ActiveChild = child;
            } else
            {
                nContainer.ExecutionState = PlayableMusicTreeNode.State.Failed;
            }
        }

        public void Visit(CueMusicTreeNode n, PlayableMusicTreeNode nContainer)
        {
            if(nContainer.ExecutionState == PlayableMusicTreeNode.State.Running)
            {
                //If was already playing, it is assumed to be finished
                nContainer.ExecutionState = PlayableMusicTreeNode.State.Complete;
                this.RunningLeaf = null;
            } else 
            {
                nContainer.ExecutionState = PlayableMusicTreeNode.State.Running;
                this.RunningLeaf = n;
            }
        }
    }
}
