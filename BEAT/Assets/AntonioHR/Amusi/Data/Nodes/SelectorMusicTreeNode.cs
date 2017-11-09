using AntonioHR.Amusi.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntonioHR.TreeAsset.Internal;

namespace AntonioHR.Amusi.Data.Nodes
{
    public class SelectorMusicTreeNode : MusicTreeNode
    {
        public override ChildrenPolicy Policy
        {
            get { return ChildrenPolicy.Multiple; }
        }

        public override void Accept(MusicNodeVisitor vis, CachedMusicTreeNode container)
        {
            vis.Visit(this, container);
        }
    }
}
