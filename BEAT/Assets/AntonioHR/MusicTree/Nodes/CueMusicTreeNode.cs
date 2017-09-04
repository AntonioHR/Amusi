using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
    }
}
