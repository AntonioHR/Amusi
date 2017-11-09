using AntonioHR.Amusi.Internal;
using AntonioHR.TreeAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntonioHR.TreeAsset.Internal;

namespace AntonioHR.Amusi.Data.Nodes
{
    public class SequenceMusicTreeNode : MusicTreeNode
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
