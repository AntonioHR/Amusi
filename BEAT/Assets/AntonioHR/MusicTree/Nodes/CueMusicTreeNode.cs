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
        
        public bool useCustomBPM;
        public int customBPM;


        public List<NoteTrack> Tracks { get { return sheet.tracks; } }

        public NoteSheet sheet;

        public override void Accept(MusicNodeVisitor vis, PlayableRuntimeMusicTreeNode container)
        {
            vis.Visit(this, container);
        }
    }
}
