using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AntonioHR.TreeAsset.Internal
{

    public class ReadonlyTreeNode<T> : ITreeNode<ReadonlyTreeNode<T>> where T : TreeNodeAsset
    {
        public class Data
        {
            public ReadonlyTreeNode<T> Right;
            public ReadonlyTreeNode<T> Left;
            public ReadonlyTreeNode<T> Parent;
            public int Depth;
            public int SibilingIndex;
            public int NodeId;
            public int SubtreeDepth;
            public T Asset;
            public ReadOnlyCollection<ReadonlyTreeNode<T>> Children;
        }

        public ReadonlyTreeNode<T> RightSibiling { get { return _data.Right; } }
        public ReadonlyTreeNode<T> LeftSibiling { get { return _data.Left; } }
        public ReadonlyTreeNode<T> Parent { get { return _data.Parent; } }
        public int Depth { get { return _data.Depth; } }
        public int SibilingIndex { get { return _data.SibilingIndex; } }
        public int NodeId { get { return _data.NodeId; } }
        public int SubtreeDepth { get { return _data.SubtreeDepth; } }
        public T Asset { get { return _data.Asset; } }
        public int ChildCount { get { return Children.Count; } }

        public ReadonlyTreeNode<T> LeftmostDescendantOfDepth(int d)
        {
            if(d == Depth)
            {
                return this;
            }
            foreach (var item in Children)
            {
                if(item.SubtreeDepth >= d)
                {
                    return item.LeftmostDescendantOfDepth(d);
                }
            }
            return null;
        }
        public ReadonlyTreeNode<T> RightmostDescendantOfDepth(int d)
        {
            if (d == Depth)
            {
                return this;
            }
            foreach (var item in Children.Reverse())
            {
                if (item.SubtreeDepth >= d)
                {
                    return item.RightmostDescendantOfDepth(d);
                }
            }
            return null;
        }

        public ReadonlyTreeNode<T> LeftmostChild
        {
            get
            {
                var c = ChildCount;
                return c == 0 ? null : Children[0];
            }
        }

        public ReadonlyTreeNode<T> RightmostChild
        {
            get
            {
                var c = ChildCount;
                return c == 0 ? null : Children[c - 1];
            }
        }

        public int LeftSibilingsCount
        {
            get
            {
                return SibilingIndex;
            }
        }
        public int RightSibilingsCount
        {
            get
            {
                return Parent.ChildCount - (SibilingIndex + 1);
            }
        }


        public ReadOnlyCollection<ReadonlyTreeNode<T>> Children
        {
            get
            {
                return _data.Children;
            }
        }


        private Data _data;
        public ReadonlyTreeNode(Data data)
        {
            _data = data;
        }

        public IEnumerable<ReadonlyTreeNode<T>> SibilingsAfter
        {
            get
            {
                return Parent.Children.Skip(SibilingIndex + 1);
            }
        }

        public IEnumerable<ReadonlyTreeNode<T>> SibilingsBefore
        {
            get
            {
                return Parent.Children.Take(SibilingIndex);
            }
        }

        public bool IsRoot
        {
            get
            {
                return Parent == null;
            }
        }

        IEnumerable<ReadonlyTreeNode<T>> ITreeNode<ReadonlyTreeNode<T>>.Children
        {
            get
            {
                return Children.AsEnumerable();
            }
        }

        ITreeNode<ReadonlyTreeNode<T>> ITreeNode<ReadonlyTreeNode<T>>.Parent
        {
            get
            {
                return Parent;
            }
        }
    }
}
