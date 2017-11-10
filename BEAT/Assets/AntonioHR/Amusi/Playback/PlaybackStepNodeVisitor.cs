using AntonioHR.Amusi.Data.Nodes;
using AntonioHR.Amusi.Internal;
using System;

namespace AntonioHR.Amusi.Playback
{
    public class PlaybackStepNodeVisitor : MusicNodeVisitor
    {
        public CueMusicTreeNode RunningLeaf { get; private set; }

        public MusicTreeEnvironment Environment { get; private set; }
        

        public PlaybackStepNodeVisitor(MusicTreeEnvironment environment)
        {
            this.Environment = environment;
        }




        public void Visit(SelectorMusicTreeNode n, CachedMusicTreeNode nContainer)
        {

            CachedMusicTreeNode first = nContainer.isRunning? nContainer.ActiveChild : nContainer.LeftmostChild;

            foreach (var ch in nContainer.ChildrenStartingAt(first))
            {
                ch.Accept(this);

                switch (ch.ExecutionState)
                {
                    case CachedMusicTreeNode.State.Running:
                        nContainer.ExecutionState = CachedMusicTreeNode.State.Running;
                        nContainer.ActiveChild = ch;
                        return;
                    case CachedMusicTreeNode.State.Idle:
                        throw new Exception("Child should not have set itself to idle");
                    case CachedMusicTreeNode.State.Failed:
                        continue;
                    case CachedMusicTreeNode.State.Complete:
                        nContainer.ExecutionState = CachedMusicTreeNode.State.Complete;
                        nContainer.ActiveChild = ch;
                        return;
                    default:
                        throw new NotImplementedException();
                }
            }

            nContainer.ExecutionState = CachedMusicTreeNode.State.Failed;
            
        }

        public void Visit(SequenceMusicTreeNode n, CachedMusicTreeNode nContainer)
        {
            CachedMusicTreeNode first = nContainer.isRunning ? nContainer.ActiveChild : nContainer.LeftmostChild;

            foreach (var ch in nContainer.ChildrenStartingAt(first))
            {
                ch.Accept(this);

                switch (ch.ExecutionState)
                {
                    case CachedMusicTreeNode.State.Running:
                        nContainer.ExecutionState = CachedMusicTreeNode.State.Running;
                        nContainer.ActiveChild = ch;
                        return;
                    case CachedMusicTreeNode.State.Idle:
                        throw new Exception("Child should not have set itself to idle");
                    case CachedMusicTreeNode.State.Failed:
                        nContainer.ExecutionState = CachedMusicTreeNode.State.Failed;
                        return;
                    case CachedMusicTreeNode.State.Complete:
                        continue;
                    default:
                        throw new NotImplementedException();
                }
            }

            nContainer.ExecutionState = CachedMusicTreeNode.State.Complete;
            nContainer.ActiveChild = nContainer.RightmostChild;
        }

        public void Visit(ConditionMusicTreeNode n, CachedMusicTreeNode nContainer)
        {
            var child = nContainer.LeftmostChild;
            
            if(nContainer.isRunning || Environment.Evaluate(n.condition))
            {
                child.Accept(this);
                nContainer.ExecutionState = child.ExecutionState;
                nContainer.ActiveChild = child;
            } else
            {
                nContainer.ExecutionState = CachedMusicTreeNode.State.Failed;
            }
        }

        public void Visit(CueMusicTreeNode n, CachedMusicTreeNode nContainer)
        {
            if(nContainer.ExecutionState == CachedMusicTreeNode.State.Running)
            {
                //If was already playing, it is assumed to be finished
                nContainer.ExecutionState = CachedMusicTreeNode.State.Complete;
                this.RunningLeaf = null;
            } else 
            {
                nContainer.ExecutionState = CachedMusicTreeNode.State.Running;
                this.RunningLeaf = n;
            }
        }
    }
}
