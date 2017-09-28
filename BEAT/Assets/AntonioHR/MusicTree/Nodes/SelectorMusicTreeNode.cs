using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.MusicTree.Nodes
{
    public class SelectorMusicTreeNode : MusicTreeNode
    {
        public override ChildrenPolicy Policy
        {
            get { return ChildrenPolicy.Multiple; }
        }


        public override ExecutionState ContinueExecution(MusicTreeNode currentChild, out CueMusicTreeNode result)
        {
            result = null;
            return ExecutionState.Done;
        }

        public override ExecutionState Execute(out CueMusicTreeNode result)
        {

            foreach (var child in GetChildrenAs<MusicTreeNode>())
            {
                var execState = child.Execute(out result);
                if (execState == ExecutionState.Running)
                    return ExecutionState.Running;
                else if (execState == ExecutionState.Done)
                    return ExecutionState.Done;
            }
            result = null;
            return ExecutionState.Fail;
        }
    }
}
