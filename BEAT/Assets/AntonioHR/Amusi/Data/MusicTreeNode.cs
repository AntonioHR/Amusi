using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntonioHR.TreeAsset;

using AntonioHR.Amusi.Data;
using AntonioHR.Amusi.Internal;

namespace AntonioHR.Amusi.Data
{
    public abstract class MusicTreeNode : TreeNodeAsset
    {
        public enum ChildrenPolicy { None, Single, Multiple}
        public abstract ChildrenPolicy Policy { get; }

        public abstract void Accept(MusicNodeVisitor vis, CachedMusicTreeNode container);
    }
}