using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntonioHR.TreeAsset;
using AntonioHR.MusicTree.Nodes;
using AntonioHR.MusicTree.Internal;
using AntonioHR.TreeAsset.Internal;

namespace AntonioHR.MusicTree
{
    public abstract class MusicTreeNode : TreeNodeAsset
    {
        public enum ChildrenPolicy { None, Single, Multiple}
        public abstract ChildrenPolicy Policy { get; }

        public abstract void Accept(MusicNodeVisitor vis, PlayableMusicTreeNode container);


    }
}