using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntonioHR.TreeAsset.Internal;

namespace AntonioHR.TreeAsset
{
    public static class TreeOperations
    {
        #region TreeNode
        public static void ChangeParentTo(this TreeNodeAsset node, TreeNodeAsset newParent)
        {
            node._hierarchy.ChangeParentTo(newParent._hierarchy);
        }
        public static void UnattachFromParent(this TreeNodeAsset node)
        {
            node._hierarchy.UnattachFromParent();
        }
  
        public static void DeleteNodeAndChildren(this TreeNodeAsset self)
        {
            self.UnattachFromParent();

            var removedContent = self.Preorder();

            self._hierarchy.DeleteHierarchy();

            foreach (var node in removedContent)
            {
                
            }
        }
        #endregion


        #region Tree Interface
        
        public static IEnumerable<T> Preorder<T>(this T self) where T : ITreeNode<T>
        {
            List<T> list = new List<T>();
            PreorderAdd(self, list);

            return list.AsEnumerable();
        }
        public static IEnumerable<T> Postorder<T>(this T self) where T : ITreeNode<T>
        {
            List<T> list = new List<T>();
            PostorderAdd(self, list);

            return list.AsEnumerable();
        }

        private static void PreorderAdd<T>(T current, List<T> list) where T : ITreeNode<T>
        {
            list.Add(current);
            foreach (var child in current.Children)
            {
                PreorderAdd(child, list);
            }
        }
        private static void PostorderAdd<T>(T current, List<T> list) where T : ITreeNode<T>
        {
            foreach (var child in current.Children)
            {
                PostorderAdd(child, list);
            }
            list.Add(current);
        }
        #endregion
    }
    public class NoParentException<T> : Exception where T:ITreeNode<T>
    {
        public ITreeNode<T> Node { get; private set; }
        public NoParentException(ITreeNode<T> node)
        {
            this.Node = node;
        }
    }
    public class NoNextSibilingException<T> : Exception where T : ITreeNode<T>
    {
        public ITreeNode<T> Node { get; private set; }
        public NoNextSibilingException(ITreeNode<T> node)
        {
            this.Node = node;
        }
    }
}
