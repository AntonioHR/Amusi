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
}