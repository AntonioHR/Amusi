using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntonioHR.TreeAsset;

namespace AntonioHR.MusicTree
{
    public abstract class MusicTreeNode : TreeNodeAsset
    {
        public enum ChildrenPolicy { None, Single, Multiple}
        public abstract ChildrenPolicy Policy { get; }
    }
    public class SelectorMusicTreeNode : MusicTreeNode
    {
        public override ChildrenPolicy Policy
        {
            get { return ChildrenPolicy.Multiple; }
        }
    }
    public class SequenceMusicTreeNode : MusicTreeNode
    {
        public override ChildrenPolicy Policy
        {
            get { return ChildrenPolicy.Multiple; }
        }
    }
    public class CueMusicTreeNode : MusicTreeNode
    {
        public override ChildrenPolicy Policy
        {
            get { return ChildrenPolicy.None; }
        }
        public AudioClip clip;
    }
}