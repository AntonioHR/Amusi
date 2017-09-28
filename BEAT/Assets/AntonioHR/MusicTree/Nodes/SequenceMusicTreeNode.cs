using AntonioHR.TreeAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.MusicTree.Nodes
{
    public class SequenceMusicTreeNode : MusicTreeNode
    {
        public override ChildrenPolicy Policy
        {
            get { return ChildrenPolicy.Multiple; }
        }

        public override ExecutionState ContinueExecution(MusicTreeNode currentChild, out CueMusicTreeNode result)
        {
            return ExecuteChildrenAfter(currentChild, out result);
        }

        public override ExecutionState Execute(out CueMusicTreeNode result)
        {
            if (_hierarchy._children.Count == 0)
            {
                result = null;
                return ExecutionState.Done;
            }
            return ExecuteAllChildren(out result);
        }


        private ExecutionState ExecuteChildrenAfter(MusicTreeNode child, out CueMusicTreeNode result)
        {
            return ExecuteChildren(child.SibilingsAfter, out result);
        }

        private ExecutionState ExecuteAllChildren(out CueMusicTreeNode result)
        {
            return ExecuteChildren(Children, out result);
        }

        private  ExecutionState ExecuteChildren(IEnumerable<TreeNodeAsset> childrenToExecute, out CueMusicTreeNode result)
        {
            foreach (var child in childrenToExecute)
            {
                var executionState = ((MusicTreeNode)child).Execute(out result);
                switch (executionState)
                {
                    case ExecutionState.Running:
                        return ExecutionState.Running;
                    case ExecutionState.Fail:
                        return ExecutionState.Fail;
                    case ExecutionState.Skipped:
                        continue;
                    case ExecutionState.Done:
                        continue;
                }
            }
            result = null;
            return ExecutionState.Done;
        }
    }
}
