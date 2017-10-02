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
        public ReadonlyNode<T> Root { get; private set; }


        public ReadonlyTree(ReadonlyNode<T> root)
        {
            Root = root;
        }

        public static ReadonlyTree<T> CreateFrom(TreeAsset<T> treeAsset)
        {
            var rootAsset = treeAsset.Root;
            ReadonlyNode<T>.Data data;
            return new ReadonlyTree<T>(CreateNodeRecursively(rootAsset, 0, out data));
        }
        private static ReadonlyNode<T> CreateNodeRecursively(TreeNodeAsset nodeAsset, int height, out ReadonlyNode<T>.Data myData)
        {
            var childrenList = new List<ReadonlyNode<T>>();
            myData = new ReadonlyNode<T>.Data();
            var me = new ReadonlyNode<T>(myData);
            myData.Height = height;

            ReadonlyNode<T> prev = null;
            ReadonlyNode<T>.Data prevData = null;
            int index = 0;
            foreach (var child in nodeAsset._hierarchy.Children)
            {
                ReadonlyNode<T>.Data childData;
                ReadonlyNode<T> node = CreateNodeRecursively(child._content, height + 1, out childData);
                childData.Parent = me;
                childData.Left = prev;
                childData.SibilingIndex = index;
                childData.Asset = (T)child._content;

                childrenList.Add(node);

                prevData.Right = node;
                prev = node;
                index++;
            }

            myData.Children = childrenList.AsReadOnly();

            return me;

        }


    }

}
