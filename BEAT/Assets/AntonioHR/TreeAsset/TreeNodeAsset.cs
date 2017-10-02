﻿using AntonioHR.TreeAsset.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR.TreeAsset
{
    public abstract class TreeNodeAsset: ScriptableObject, ITreeNode<TreeNodeAsset>
    {

        [SerializeField]
        internal TreeHierarchyNodeAsset _hierarchy;


        public IEnumerable<TreeNodeAsset> Children 
        {
            get
            {
                return _hierarchy._children.Select(x=>x._content);
            }
        }

        public IEnumerable<T> GetChildrenAs<T>() where T : TreeNodeAsset
        {
            return _hierarchy._children.Select(x => x._content as T);
        }

        public IEnumerable<TreeNodeAsset> SibilingsAfter
        {
            get { return _hierarchy.SibilingsAfter.Select(x => x._content); }
        }

        public IEnumerable<TreeNodeAsset> SibilingsBefore
        {
            get { return _hierarchy.SibilingsBefore.Select(x => x._content); }
        }

        public ITreeNode<TreeNodeAsset> Parent
        {
            get { return _hierarchy._parent._content; }
        }

        public bool IsRoot
        {
            get { return _hierarchy.IsRoot; }
        }



    }
    public interface ITreeNode<T> where T : ITreeNode<T>
    {
        IEnumerable<T> Children { get; }
        IEnumerable<T> SibilingsAfter { get; }
        IEnumerable<T> SibilingsBefore { get; }
        bool IsRoot { get; }
        ITreeNode<T> Parent { get; }
    }
}
