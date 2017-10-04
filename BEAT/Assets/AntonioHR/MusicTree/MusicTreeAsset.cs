using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntonioHR.TreeAsset;
using UnityEditor;
using AntonioHR.MusicTree.Nodes;
using AntonioHR.ConditionVariables;

namespace AntonioHR.MusicTree
{
    public class MusicTreeAsset : TreeAsset<MusicTreeNode>
    {

        public List<ConditionVariable> vars;

        protected override MusicTreeNode InstantiateRoot()
        {
            var result = ScriptableObject.CreateInstance<SelectorMusicTreeNode>();
            result.name = "Root(Selector)";
            return result;
        }

        protected override MusicTreeNode InstantiateDefaultNode()
        {
            return InstantiateNode<SelectorMusicTreeNode>();
        }

        protected override subT InstantiateNode<subT>()
        {
            var result = ScriptableObject.CreateInstance<subT>();
            result.name = "Node(Selector)";
            return result;
        }

        


        #region Debug Examples
        [MenuItem("Music Tree/Example Tree")]
        public static void CreateExampleTree()
        {
            var tree = ScriptableObject.CreateInstance<MusicTreeAsset>();
            AssetDatabase.CreateAsset(tree, "Assets/Example Tree.asset");

            AssetDatabase.SaveAssets();
            tree.Init();


            var seq1 = tree.CreateChildFor<SequenceMusicTreeNode>(tree.Root, "Sequence 1");
            var m1 = tree.CreateChildFor<CueMusicTreeNode>(seq1, "Music 1");

            var m2 = tree.CreateChildFor<CueMusicTreeNode>(seq1, "Music 2");

        }
        [MenuItem("Music Tree/Big Example Tree")]
        public static void CreateBigExampleTree()
        {
            var tree = ScriptableObject.CreateInstance<MusicTreeAsset>();
            AssetDatabase.CreateAsset(tree, "Assets/Big Example Tree.asset");

            AssetDatabase.SaveAssets();
            tree.Init();


            var seq1 = tree.CreateChildFor<SequenceMusicTreeNode>(tree.Root, "Sequence 1");
            var m1 = tree.CreateChildFor<CueMusicTreeNode>(seq1, "Music 1");

            var m2 = tree.CreateChildFor<CueMusicTreeNode>(seq1, "Music 2");

            var seq2 = tree.CreateChildFor<SequenceMusicTreeNode>(tree.Root, "Sequence 2");

            var m3 = tree.CreateChildFor<CueMusicTreeNode>(seq2, "Music 3");
            var sel2 = tree.CreateChildFor<SelectorMusicTreeNode>(seq2, "Selector 2");
            var m4 = tree.CreateChildFor<CueMusicTreeNode>(sel2, "Music 4");
            var m5 = tree.CreateChildFor<CueMusicTreeNode>(sel2, "Music 5");
        }
        [MenuItem("Music Tree/Big Example Tree with Conditions")]
        public static void CreateBigExampleTreeWithConds()
        {
            var tree = ScriptableObject.CreateInstance<MusicTreeAsset>();
            AssetDatabase.CreateAsset(tree, "Assets/Big Example Tree With Conds.asset");

            AssetDatabase.SaveAssets();
            tree.Init();

            var cond1 = tree.CreateChildFor<ConditionMusicTreeNode>(tree.Root, "Condition 1");
            var seq1 = tree.CreateChildFor<SequenceMusicTreeNode>(cond1, "Sequence 1");
            var m1 = tree.CreateChildFor<CueMusicTreeNode>(seq1, "Music 1");

            var m2 = tree.CreateChildFor<CueMusicTreeNode>(seq1, "Music 2");

            var seq2 = tree.CreateChildFor<SequenceMusicTreeNode>(tree.Root, "Sequence 2");

            var m3 = tree.CreateChildFor<CueMusicTreeNode>(seq2, "Music 3");
            var sel2 = tree.CreateChildFor<SelectorMusicTreeNode>(seq2, "Selector 2");

            var cond2 = tree.CreateChildFor<ConditionMusicTreeNode>(sel2, "Condition 2");
            var m4 = tree.CreateChildFor<CueMusicTreeNode>(cond2, "Music 4");
            var m5 = tree.CreateChildFor<CueMusicTreeNode>(sel2, "Music 5");
        }
        #endregion
    }
}