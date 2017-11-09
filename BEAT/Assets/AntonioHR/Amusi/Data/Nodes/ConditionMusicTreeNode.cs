using AntonioHR.ConditionVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntonioHR.Amusi.Internal;
using AntonioHR.TreeAsset.Internal;

namespace AntonioHR.Amusi.Data.Nodes
{
    public class ConditionMusicTreeNode: MusicTreeNode
    {
        public Condition condition;


        public override ChildrenPolicy Policy
        {
            get { return ChildrenPolicy.Single; }
        }

        public override void Accept(MusicNodeVisitor vis, CachedMusicTreeNode container)
        {
            vis.Visit(this, container);
        }
    }
}
