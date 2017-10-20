using AntonioHR.TreeAsset.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AntonioHR.TreeAsset
{
    public class RuntimeTree<TA, A, N>
        where TA : TreeAsset<A>
        where A:TreeNodeAsset
        where N: RuntimeTreeNode<A, N>, new()
    {
        public TA Asset { get; private set; }
        public N Root { get; private set; }
        public int NodeCount { get; private set; }
        public IEnumerable<N> AllNodes { get { return Root.Preorder(); } }


        public RuntimeTree()
        {

        }

        public void Init(TA treeAsset, N root, int nodeCount)
        {
            this.Asset = treeAsset;
            this.Root = root;
            this.NodeCount = nodeCount;
            AfterInit();
        }
        protected virtual void AfterInit()
        {

        }

        public static RT CreateTreeFrom<RT>(TA treeAsset) where RT:RuntimeTree<TA, A, N>, new()
        {
            var rootAsset = treeAsset.Root;
            RuntimeTreeNode<A, N>.NodeHierarchy hierarchy;
            int nextNodeId = 0;
            var root = CreateNodeRecursively(rootAsset, 0, ref nextNodeId, out hierarchy);

            root.InitializeHierarchy(hierarchy);

            RT result = new RT();
            result.Init(treeAsset, root, nextNodeId);
            return result;
        }
        public static RuntimeTree<TA, A, N> CreateTreeFrom(TA treeAsset)
        {
            return CreateTreeFrom<RuntimeTree<TA, A, N>>(treeAsset);
        }
        private static N CreateNodeRecursively(TreeNodeAsset nodeAsset, int height, ref int nextNodeId, out RuntimeTreeNode<A, N>.NodeHierarchy myData)
        {
            var childrenList = new List<N>();
            myData = new RuntimeTreeNode<A, N>.NodeHierarchy();
            var me = new N();
            myData.Depth = height;
            myData.NodeId = nextNodeId;
            myData.Asset = (A)nodeAsset;
            myData.SibilingIndex = 0;
            myData.SubtreeDepth = myData.Depth;
            nextNodeId++;

            N prev = null;
            RuntimeTreeNode<A, N>.NodeHierarchy prevHierarchy = null;
            int index = 0;
            foreach (var child in nodeAsset._hierarchy.Children)
            {
                RuntimeTreeNode<A, N>.NodeHierarchy childHierarchy;
                N node = CreateNodeRecursively(child._content, height + 1, ref nextNodeId, out childHierarchy);
                childHierarchy.Parent = me;
                childHierarchy.Left = prev;
                childHierarchy.SibilingIndex = index;

                childrenList.Add(node);
                myData.SubtreeDepth = Math.Max(myData.SubtreeDepth, childHierarchy.SubtreeDepth);

                if (prev != null)
                {
                    prevHierarchy.Right = node;
                    prev.InitializeHierarchy(prevHierarchy);
                }
                prev = node;
                prevHierarchy = childHierarchy;
                index++;
            }
            if (prev != null)
                prev.InitializeHierarchy(prevHierarchy);

            myData.Children = childrenList.AsReadOnly();

            return me;

        }
    }

    public class RuntimeTree<A> : RuntimeTree<TreeAsset<A>, A, RuntimeTreeNode<A>>
        where A : TreeNodeAsset
    {
        public static new RuntimeTree<A> CreateTreeFrom(TreeAsset<A> treeAsset)
        {
            return CreateTreeFrom<RuntimeTree<A>>(treeAsset);
        }
    }
}
