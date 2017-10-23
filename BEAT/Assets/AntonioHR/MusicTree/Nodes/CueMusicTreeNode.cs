using AntonioHR.MusicTree.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AntonioHR.TreeAsset.Internal;

namespace AntonioHR.MusicTree.Nodes
{
    public class CueMusicTreeNode : MusicTreeNode
    {
        public override ChildrenPolicy Policy
        {
            get { return ChildrenPolicy.None; }
        }
        public AudioClip clip;
        public BeatFW.BeatPatternAsset pattern;

        public List<BeatFW.BeatPattern> patterns;

        public override void Accept(MusicNodeVisitor vis, PlayableMusicTreeNode container)
        {
            vis.Visit(this, container);
        }
    }
}
