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
    }
}
