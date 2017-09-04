using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntonioHR.TreeAsset;

namespace AntonioHR.MusicTree
{
    public class MusicTree : TreeAsset<MusicTreeNode>
    {

        protected override MusicTreeNode InstantiateRoot()
        {
            var result = ScriptableObject.CreateInstance<SelectorMusicTreeNode>();
            result.name = "Root(Selector)";
            return result;
        }

        protected override MusicTreeNode InstantiateDefaultNode()
        {
            var result = ScriptableObject.CreateInstance<SelectorMusicTreeNode>();
            result.name = "Node(Selector)";
            return result;
        }
    }
}