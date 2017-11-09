using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AntonioHR.TreeAsset
{

    public class CachedTreeNode<RT, TA, TNA, RTN> : ITreeNode<RTN> 
        where RT : CachedTree<RT, TA, TNA, RTN>, new()
        where TA : TreeAsset<TNA>
        where TNA : TreeNodeAsset
        where RTN : CachedTreeNode<RT, TA, TNA, RTN>, new()
    {
        public class NodeHierarchy
        {
            public RTN Right;
            public RTN Left;
            public RTN Parent;
            public int Depth;
            public int SibilingIndex;
            public int NodeId;
            public int SubtreeDepth;
            public TNA Asset;
            public RT Tree;
            public ReadOnlyCollection<RTN> Children;
        }

        #region Data Accessors


        public RTN RightSibiling { get { return _hierarchy.Right; } }
        public RTN LeftSibiling { get { return _hierarchy.Left; } }
        public RTN Parent { get { return _hierarchy.Parent; } }
        public int Depth { get { return _hierarchy.Depth; } }
        public int SibilingIndex { get { return _hierarchy.SibilingIndex; } }
        public int NodeId { get { return _hierarchy.NodeId; } }
        public int SubtreeDepth { get { return _hierarchy.SubtreeDepth; } }
        public TNA Asset { get { return _hierarchy.Asset; } }
        public RT Tree { get { return _hierarchy.Tree; } }
        public int ChildCount { get { return Children.Count; } }

        public RTN LeftmostDescendantOfDepth(int d)
        {
            if(d == Depth)
            {
                return (RTN)this;
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
        public RTN RightmostDescendantOfDepth(int d)
        {
            if (d == Depth)
            {
                return (RTN)this;
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

        public RTN LeftmostChild
        {
            get
            {
                var c = ChildCount;
                return c == 0 ? null : Children[0];
            }
        }

        public RTN RightmostChild
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


        public ReadOnlyCollection<RTN> Children
        {
            get
            {
                return _hierarchy.Children;
            }
        }
        public IEnumerable<RTN> ChildrenStartingAt(RTN child)
        {
            return Children.SkipWhile(x => x != child);
        }
        #endregion Data Accessors

        #region ITreeNode Implementation
        public IEnumerable<RTN> SibilingsAfter
        {
            get
            {
                return Parent.Children.Skip(SibilingIndex + 1);
            }
        }

        public IEnumerable<RTN> SibilingsBefore
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

        IEnumerable<RTN> ITreeNode<RTN>.Children
        {
            get
            {
                return Children.AsEnumerable();
            }
        }

        ITreeNode<RTN> ITreeNode<RTN>.Parent
        {
            get
            {
                return Parent;
            }
        }
        #endregion

        

        private NodeHierarchy _hierarchy;
        public CachedTreeNode()
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
    public class CachedTreeNode<TNA> : CachedTreeNode<CachedTree<TNA>, TreeAsset<TNA>, TNA, CachedTreeNode<TNA>> 
        where TNA : TreeNodeAsset
    {

    }
}
