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
            return new ReadonlyTree<T>(CreateNodeRecursively(rootAsset));
        }
        private static ReadonlyNode<T> CreateNodeRecursively(TreeNodeAsset nodeAsset, int height = 0)
        {
            var childrenList = new List<ReadonlyNode<T>>();
            var me = new ReadonlyNode<T>();
            me.Height = height;

            ReadonlyNode<T> prev = null;
            int index = 0;
            foreach (var child in nodeAsset._hierarchy.Children)
            {
                ReadonlyNode<T> node = CreateNodeRecursively(child._content, height + 1);
                node.Parent = me;
                node.Left = prev;
                node.myIndex = index;
                node.Asset = (T)child._content;

                childrenList.Add(node);

                prev.Right = node;
                prev = node;
                index++;
            }

            me.Children = childrenList.AsReadOnly();

            return me;

        }


    }
    public class ReadonlyNode<T> : ITreeNode<ReadonlyNode<T>> where T : TreeNodeAsset
    {
        public ReadonlyNode<T> Right;
        public ReadonlyNode<T> Left;
        public ReadonlyNode<T> Parent;
        public int Height;
        public int myIndex;
        public T Asset;
        public ReadOnlyCollection<ReadonlyNode<T>> Children;

        public IEnumerable<ReadonlyNode<T>> SibilingsAfter
        {
            get
            {
                return Parent.Children.Skip(myIndex + 1);
            }
        }

        public IEnumerable<ReadonlyNode<T>> SibilingsBefore
        {
            get
            {
                return Parent.Children.Take(myIndex);
            }
        }

        public bool IsRoot
        {
            get
            {
                return Parent == null;
            }
        }

        IEnumerable<ReadonlyNode<T>> ITreeNode<ReadonlyNode<T>>.Children
        {
            get
            {
                return Children.AsEnumerable();
            }
        }

        ITreeNode<ReadonlyNode<T>> ITreeNode<ReadonlyNode<T>>.Parent
        {
            get
            {
                return Parent;
            }
        }
    }

}
