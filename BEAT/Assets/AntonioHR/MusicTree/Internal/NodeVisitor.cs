using AntonioHR.MusicTree.Nodes;
using AntonioHR.TreeAsset.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntonioHR.MusicTree.Internal
{
    public interface MusicNodeVisitor
    {
        void Visit(CueMusicTreeNode n, PlayableMusicTreeNode container);
        void Visit(ConditionMusicTreeNode n, PlayableMusicTreeNode container);
        void Visit(SelectorMusicTreeNode n, PlayableMusicTreeNode container);
        void Visit(SequenceMusicTreeNode n, PlayableMusicTreeNode container);

    }

}
