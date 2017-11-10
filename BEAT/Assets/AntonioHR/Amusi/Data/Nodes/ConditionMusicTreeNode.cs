using AntonioHR.Amusi.Internal;
using AntonioHR.ConditionVariables;

namespace AntonioHR.Amusi.Data.Nodes
{
    public class ConditionMusicTreeNode: MusicTreeNode
    {
        public Condition condition;


        public override ChildrenPolicy Policy
        {
            get { return ChildrenPolicy.Single; }
        }

        public override void Accept(MusicNodeVisitor vis, CachedMusicTreeNode container)
        {
            vis.Visit(this, container);
        }
    }
}
