using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AntonioHR.TreeAsset.Internal
{
    public class TreeHierarchyNodeAsset : ScriptableObject, ITreeNode<TreeHierarchyNodeAsset>
    {
        [SerializeField]
        internal TreeHierarchyAsset _tree;

        [SerializeField]
        internal List<TreeHierarchyNodeAsset> _children = new List<TreeHierarchyNodeAsset>();
        [SerializeField]
        internal TreeHierarchyNodeAsset _parent;
        [SerializeField]
        public bool _isFloating = false;

        public TreeNodeAsset _content;


        public bool IsRoot { get { return _tree._root == this; } }

        public IEnumerable<TreeHierarchyNodeAsset> Children
        {
            get { return _children.AsEnumerable(); }
        }

        public ITreeNode<TreeHierarchyNodeAsset> Parent
        {
            get { return Parent; }
        }

        public IEnumerable<TreeHierarchyNodeAsset> SibilingsAfter
        {
            get 
            { 
                return IsRoot? Enumerable.Empty<TreeHierarchyNodeAsset>():
                _parent.Children.SkipWhile(c => c != this).Skip(1); 
            }
        }
        public IEnumerable<TreeHierarchyNodeAsset> SibilingsBefore
        {
            get 
            { 
                return IsRoot? Enumerable.Empty<TreeHierarchyNodeAsset>():
                _parent.Children.TakeWhile(c => c != this); 
            }
        }
    }
}
