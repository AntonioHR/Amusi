using AntonioHR.MusicTree.Nodes;
using AntonioHR.TreeAsset.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.MusicTree.Internal
{
    public static class NodeVisitorOperations
    {
        public static void Accept(this PlayableRuntimeMusicTreeNode node, MusicNodeVisitor v)
        {
            node.Asset.Accept(v, node);
        }

        public static PlayableRuntimeMusicTreeNode.State Execute(this PlayableRuntimeMusicTreeNode root, MusicTreeEnvironment env, out CueMusicTreeNode resultNode)
        {
            var visitor = new ExecutionStepNodeVisitor(env);
            root.Accept(visitor);
            resultNode = visitor.RunningLeaf;
            return root.ExecutionState;
        }
    }
}
