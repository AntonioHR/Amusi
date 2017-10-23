using AntonioHR.MusicTree.Nodes;
using AntonioHR.TreeAsset.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntonioHR.MusicTree.Internal
{
    public interface MusicNodeVisitor
    {
        void Visit(CueMusicTreeNode n, PlayableRuntimeMusicTreeNode container);
        void Visit(ConditionMusicTreeNode n, PlayableRuntimeMusicTreeNode container);
        void Visit(SelectorMusicTreeNode n, PlayableRuntimeMusicTreeNode container);
        void Visit(SequenceMusicTreeNode n, PlayableRuntimeMusicTreeNode container);

    }

}
