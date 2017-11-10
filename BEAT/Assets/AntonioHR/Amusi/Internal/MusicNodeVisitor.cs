using AntonioHR.Amusi.Data.Nodes;

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
