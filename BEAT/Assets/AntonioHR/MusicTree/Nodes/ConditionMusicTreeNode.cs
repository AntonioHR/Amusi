using AntonioHR.ConditionVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.MusicTree.Nodes
{
    public class ConditionMusicTreeNode: MusicTreeNode
    {
        public Condition condition;


        public override ChildrenPolicy Policy
        {
            get { return ChildrenPolicy.Single; }
        }

        public override MusicTreeNode.ExecutionState ContinueExecution(MusicTreeEnvironment env, MusicTreeNode currentChild, out CueMusicTreeNode result)
        {
            throw new NotImplementedException();
        }
        public override ExecutionState Execute(MusicTreeEnvironment env, out CueMusicTreeNode result)
        {
            throw new NotImplementedException();
        }
    }
}
