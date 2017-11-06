using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntonioHR.TreeAsset;
using UnityEditor;
using AntonioHR.MusicTree.Nodes;
using AntonioHR.MusicTree.ConditionVariables;
using AntonioHR.MusicTree.Internal;
using System;

namespace AntonioHR.MusicTree
{

    public enum BarType { FourFour, ThreeFour}
    public class MusicTreeAsset : TreeAsset<MusicTreeNode>
    {

        public List<ConditionVariable> vars;

        public List<NoteTrackDefinition> trackDefinitions;

        public int defaultBPM;

        public int NotesPerBar
        {
            get
            {
                switch (barType)
                {
                    case BarType.FourFour:
                        return 4;
                    case BarType.ThreeFour:
                        return 3;
                    default:
                        break;
                }
                throw new NotImplementedException();
            }
        }
        

        public BarType barType;


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

            var cue = result as CueMusicTreeNode;
            if(cue != null)
            {
                for (int i = 0; i < trackDefinitions.Count; i++)
                {
                    cue.Tracks.Add(new BeatSync.NoteTrack() {name = trackDefinitions[i].name, notes = new List<BeatSync.Note>()});
                }
            }

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
            tree.CreateChildFor<CueMusicTreeNode>(seq1, "Music 1");

            tree.CreateChildFor<CueMusicTreeNode>(seq1, "Music 2");

        }
        [MenuItem("Music Tree/Big Example Tree")]
        public static void CreateBigExampleTree()
        {
            var tree = ScriptableObject.CreateInstance<MusicTreeAsset>();
            AssetDatabase.CreateAsset(tree, "Assets/Big Example Tree.asset");

            AssetDatabase.SaveAssets();
            tree.Init();


            var seq1 = tree.CreateChildFor<SequenceMusicTreeNode>(tree.Root, "Sequence 1");
            tree.CreateChildFor<CueMusicTreeNode>(seq1, "Music 1");

            tree.CreateChildFor<CueMusicTreeNode>(seq1, "Music 2");

            var seq2 = tree.CreateChildFor<SequenceMusicTreeNode>(tree.Root, "Sequence 2");

            tree.CreateChildFor<CueMusicTreeNode>(seq2, "Music 3");
            var sel2 = tree.CreateChildFor<SelectorMusicTreeNode>(seq2, "Selector 2");
            tree.CreateChildFor<CueMusicTreeNode>(sel2, "Music 4");
            tree.CreateChildFor<CueMusicTreeNode>(sel2, "Music 5");
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
            tree.CreateChildFor<CueMusicTreeNode>(seq1, "Music 1");

            tree.CreateChildFor<CueMusicTreeNode>(seq1, "Music 2");

            var seq2 = tree.CreateChildFor<SequenceMusicTreeNode>(tree.Root, "Sequence 2");

            tree.CreateChildFor<CueMusicTreeNode>(seq2, "Music 3");
            var sel2 = tree.CreateChildFor<SelectorMusicTreeNode>(seq2, "Selector 2");

            var cond2 = tree.CreateChildFor<ConditionMusicTreeNode>(sel2, "Condition 2");
            tree.CreateChildFor<CueMusicTreeNode>(cond2, "Music 4");
            tree.CreateChildFor<CueMusicTreeNode>(sel2, "Music 5");
        }

        #endregion
    }
}