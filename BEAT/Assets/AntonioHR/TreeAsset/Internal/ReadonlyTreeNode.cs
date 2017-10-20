using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AntonioHR.TreeAsset.Internal
{

    public class RuntimeTreeNode<A, N> : ITreeNode<N> 
        where A : TreeNodeAsset
        where N : RuntimeTreeNode<A, N>
    {
        public class NodeHierarchy
        {
            public N Right;
            public N Left;
            public N Parent;
            public int Depth;
            public int SibilingIndex;
            public int NodeId;
            public int SubtreeDepth;
            public A Asset;
            public ReadOnlyCollection<N> Children;
        }

        #region Data Accessors


        public N RightSibiling { get { return _hierarchy.Right; } }
        public N LeftSibiling { get { return _hierarchy.Left; } }
        public N Parent { get { return _hierarchy.Parent; } }
        public int Depth { get { return _hierarchy.Depth; } }
        public int SibilingIndex { get { return _hierarchy.SibilingIndex; } }
        public int NodeId { get { return _hierarchy.NodeId; } }
        public int SubtreeDepth { get { return _hierarchy.SubtreeDepth; } }
        public A Asset { get { return _hierarchy.Asset; } }
        public int ChildCount { get { return Children.Count; } }

        public N LeftmostDescendantOfDepth(int d)
        {
            if(d == Depth)
            {
                return (N)this;
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
        public N RightmostDescendantOfDepth(int d)
        {
            if (d == Depth)
            {
                return (N)this;
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

        public N LeftmostChild
        {
            get
            {
                var c = ChildCount;
                return c == 0 ? null : Children[0];
            }
        }

        public N RightmostChild
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


        public ReadOnlyCollection<N> Children
        {
            get
            {
                return _hierarchy.Children;
            }
        }
        public IEnumerable<N> ChildrenStartingAt(N child)
        {
            return Children.SkipWhile(x => x != child);
        }
        #endregion Data Accessors

        #region ITreeNode Implementation
        public IEnumerable<N> SibilingsAfter
        {
            get
            {
                return Parent.Children.Skip(SibilingIndex + 1);
            }
        }

        public IEnumerable<N> SibilingsBefore
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

        IEnumerable<N> ITreeNode<N>.Children
        {
            get
            {
                return Children.AsEnumerable();
            }
        }

        ITreeNode<N> ITreeNode<N>.Parent
        {
            get
            {
                return Parent;
            }
        }
        #endregion



        private NodeHierarchy _hierarchy;
        public RuntimeTreeNode()
        {
        }
        
        public void InitializeHierarchy(NodeHierarchy hierarchy)
        {
            this._hierarchy = hierarchy;
            AfterHierarchyInitalized();
        }
        protected virtual void AfterHierarchyInitalized()
        {

        }
    }
    public class RuntimeTreeNode<A> : RuntimeTreeNode<A, RuntimeTreeNode<A>> where A: TreeNodeAsset
    {

    }
}
