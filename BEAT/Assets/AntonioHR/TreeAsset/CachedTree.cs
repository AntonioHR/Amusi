using System;
using System.Collections.Generic;

namespace AntonioHR.TreeAsset
{
    public class CachedTree<RT, TA, TNA, RTN>
        where RT : CachedTree<RT, TA, TNA, RTN>, new()
        where TA : TreeAsset<TNA>
        where TNA:TreeNodeAsset
        where RTN: CachedTreeNode<RT, TA, TNA, RTN>, new()
    {
        public TA Asset { get; private set; }
        public RTN Root { get; private set; }
        public int NodeCount { get; private set; }
        public IEnumerable<RTN> AllNodes { get { return Root.Preorder(); } }


        public CachedTree()
        {

        }

        public void Init(TA treeAsset, RTN root, int nodeCount)
        {
            this.Asset = treeAsset;
            this.Root = root;
            this.NodeCount = nodeCount;
            AfterInit();
        }
        protected virtual void AfterInit()
        {

        }
        

        public static RT CreateFrom(TA treeAsset)
        {
            RT result = new RT();
            var rootAsset = treeAsset.Root;
            CachedTreeNode<RT, TA, TNA, RTN>.NodeHierarchy hierarchy;
            int nextNodeId = 0;
            var root = CreateNodeRecursively(result, rootAsset, 0, ref nextNodeId, out hierarchy);

            root.InitializeHierarchy(hierarchy);
            result.Init(treeAsset, root, nextNodeId);
            return result;
        }

        private static RTN CreateNodeRecursively(RT tree, TreeNodeAsset nodeAsset, int height, ref int nextNodeId, out CachedTreeNode<RT, TA, TNA, RTN>.NodeHierarchy myData)
        {
            var childrenList = new List<RTN>();
            myData = new CachedTreeNode<RT, TA, TNA, RTN>.NodeHierarchy();
            var me = new RTN();
            myData.Depth = height;
            myData.NodeId = nextNodeId;
            myData.Asset = (TNA)nodeAsset;
            myData.SibilingIndex = 0;
            myData.SubtreeDepth = myData.Depth;
            myData.Tree = tree;
            nextNodeId++;

            RTN prev = null;
            CachedTreeNode<RT, TA, TNA, RTN>.NodeHierarchy prevHierarchy = null;
            int index = 0;
            foreach (var child in nodeAsset._hierarchy.Children)
            {
                CachedTreeNode<RT, TA, TNA, RTN>.NodeHierarchy childHierarchy;
                RTN node = CreateNodeRecursively(tree, child._content, height + 1, ref nextNodeId, out childHierarchy);
                
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

    public class CachedTree<A> : CachedTree<CachedTree<A>, TreeAsset<A>, A, CachedTreeNode<A>>
        where A : TreeNodeAsset
    {
    }
}
