using AntonioHR.MusicTree.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AntonioHR.TreeAsset.Internal;
using AntonioHR.MusicTree.BeatSync;

namespace AntonioHR.MusicTree.Nodes
{
    public class CueMusicTreeNode : MusicTreeNode
    {
        public override ChildrenPolicy Policy
        {
            get { return ChildrenPolicy.None; }
        }
        public AudioClip clip;

        public List<NoteTrack> Tracks;

        public override void Accept(MusicNodeVisitor vis, PlayableRuntimeMusicTreeNode container)
        {
            vis.Visit(this, container);
        }
    }
}
