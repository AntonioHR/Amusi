using AntonioHR.ConditionVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntonioHR.MusicTree.Internal;
using AntonioHR.TreeAsset.Internal;

namespace AntonioHR.MusicTree.Nodes
{
    public class ConditionMusicTreeNode: MusicTreeNode
    {
        public Condition condition;


        public override ChildrenPolicy Policy
        {
            get { return ChildrenPolicy.Single; }
        }

        public override void Accept(MusicNodeVisitor vis, PlayableRuntimeMusicTreeNode container)
        {
            vis.Visit(this, container);
        }
    }
}
