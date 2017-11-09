using AntonioHR.Amusi.Data;
using AntonioHR.TreeAsset.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntonioHR.Amusi.Data.Nodes;
using AntonioHR.Amusi.Internal;

namespace AntonioHR.Amusi.Internal
{
    public interface MusicNodeVisitor
    {
        void Visit(CueMusicTreeNode n, CachedMusicTreeNode container);
        void Visit(ConditionMusicTreeNode n, CachedMusicTreeNode container);
        void Visit(SelectorMusicTreeNode n, CachedMusicTreeNode container);
        void Visit(SequenceMusicTreeNode n, CachedMusicTreeNode container);

    }

}
