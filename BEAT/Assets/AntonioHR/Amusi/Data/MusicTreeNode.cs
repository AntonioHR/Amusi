using AntonioHR.Amusi.Internal;
using AntonioHR.TreeAsset;

namespace AntonioHR.Amusi.Data
{
    public abstract class MusicTreeNode : TreeNodeAsset
    {
        public enum ChildrenPolicy { None, Single, Multiple}
        public abstract ChildrenPolicy Policy { get; }

        public abstract void Accept(MusicNodeVisitor vis, CachedMusicTreeNode container);
    }
}