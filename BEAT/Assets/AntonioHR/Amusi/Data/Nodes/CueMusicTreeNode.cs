using AntonioHR.Amusi.Internal;
using System.Collections.Generic;
using UnityEngine;

namespace AntonioHR.Amusi.Data.Nodes
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

        [HideInInspector]
        public NoteSheet sheet;

        public CueMusicTreeNode()
        {
            sheet = new NoteSheet
            {
                tracks = new List<NoteTrack>()
            };
        }

        public override void Accept(MusicNodeVisitor vis, CachedMusicTreeNode container)
        {
            vis.Visit(this, container);
        }

    }
}
