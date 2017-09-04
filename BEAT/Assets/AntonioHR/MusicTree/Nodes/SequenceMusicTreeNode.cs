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
    }
}
