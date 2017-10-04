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

        public override ExecutionState ContinueExecution(MusicTreeEnvironment env, MusicTreeNode currentChild, out CueMusicTreeNode result)
        {
            return ExecuteChildrenAfter(currentChild, env, out result);
        }

        public override ExecutionState Execute(MusicTreeEnvironment env, out CueMusicTreeNode result)
        {
            if (_hierarchy._children.Count == 0)
            {
                result = null;
                return ExecutionState.Done;
            }
            return ExecuteAllChildren(env, out result);
        }


        private ExecutionState ExecuteChildrenAfter(MusicTreeNode child, MusicTreeEnvironment env, out CueMusicTreeNode result)
        {
            return ExecuteChildren(env, child.SibilingsAfter, out result);
        }

        private ExecutionState ExecuteAllChildren(MusicTreeEnvironment env, out CueMusicTreeNode result)
        {
            return ExecuteChildren(env, Children, out result);
        }

        private ExecutionState ExecuteChildren(MusicTreeEnvironment env, IEnumerable<TreeNodeAsset> childrenToExecute, out CueMusicTreeNode result)
        {
            foreach (var child in childrenToExecute)
            {
                var executionState = ((MusicTreeNode)child).Execute(env, out result);
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
