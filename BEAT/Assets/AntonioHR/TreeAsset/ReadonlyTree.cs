using AntonioHR.TreeAsset.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AntonioHR.TreeAsset
{
    public class ReadonlyTree<T> where T:TreeNodeAsset
    {
        public ReadonlyTreeNode<T> Root { get; private set; }
        public int NodeCount { get; private set; }
        public IEnumerable<ReadonlyTreeNode<T>> AllNodes { get { return Root.Preorder(); } }



        public ReadonlyTree(ReadonlyTreeNode<T> root, int nodeCount)
        {
            this.Root = root;
            this.NodeCount = nodeCount;
        }

        public static ReadonlyTree<T> CreateFrom(TreeAsset<T> treeAsset)
        {
            var rootAsset = treeAsset.Root;
            ReadonlyTreeNode<T>.Data data;
            int nextNodeId = 0;
            var root = CreateNodeRecursively(rootAsset, 0, ref nextNodeId, out data);

            return new ReadonlyTree<T>(root, nextNodeId);
        }
        private static ReadonlyTreeNode<T> CreateNodeRecursively(TreeNodeAsset nodeAsset, int height, ref int nextNodeId, out ReadonlyTreeNode<T>.Data myData)
        {
            var childrenList = new List<ReadonlyTreeNode<T>>();
            myData = new ReadonlyTreeNode<T>.Data();
            var me = new ReadonlyTreeNode<T>(myData);
            myData.Depth = height;
            myData.NodeId = nextNodeId;
            myData.Asset = (T)nodeAsset;
            myData.SibilingIndex = 0;
            myData.SubtreeDepth = myData.Depth;
            nextNodeId++;

            ReadonlyTreeNode<T> prev = null;
            ReadonlyTreeNode<T>.Data prevData = null;
            int index = 0;
            foreach (var child in nodeAsset._hierarchy.Children)
            {
                ReadonlyTreeNode<T>.Data childData;
                ReadonlyTreeNode<T> node = CreateNodeRecursively(child._content, height + 1, ref nextNodeId, out childData);
                childData.Parent = me;
                childData.Left = prev;
                childData.SibilingIndex = index;

                childrenList.Add(node);
                myData.SubtreeDepth = Math.Max(myData.SubtreeDepth, childData.SubtreeDepth);

                if(prevData != null)
                    prevData.Right = node;
                prev = node;
                index++;
            }

            myData.Children = childrenList.AsReadOnly();

            return me;

        }
    }

}
