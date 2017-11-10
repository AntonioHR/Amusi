using AntonioHR.Amusi.Data.Nodes;
using AntonioHR.Amusi.Playback;

namespace AntonioHR.Amusi.Internal
{
    public static class NodeVisitorOperations
    {
        public static void Accept(this CachedMusicTreeNode node, MusicNodeVisitor v)
        {
            node.Asset.Accept(v, node);
        }

        public static CachedMusicTreeNode.State Execute(this CachedMusicTreeNode root, MusicTreeEnvironment env, out CueMusicTreeNode resultNode)
        {
            var visitor = new PlaybackStepNodeVisitor(env);
            root.Accept(visitor);
            resultNode = visitor.RunningLeaf;
            return root.ExecutionState;
        }
    }
}
