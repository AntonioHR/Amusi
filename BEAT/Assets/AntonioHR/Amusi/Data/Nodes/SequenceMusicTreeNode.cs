using AntonioHR.Amusi.Internal;

namespace AntonioHR.Amusi.Data.Nodes
{
    public class SequenceMusicTreeNode : MusicTreeNode
    {
        public override ChildrenPolicy Policy
        {
            get { return ChildrenPolicy.Multiple; }
        }

        public override void Accept(MusicNodeVisitor vis, CachedMusicTreeNode container)
        {
            vis.Visit(this, container);
        }
        
    }
}
