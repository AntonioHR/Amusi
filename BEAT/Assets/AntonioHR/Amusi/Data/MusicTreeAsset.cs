using AntonioHR.Amusi.Data.Nodes;
using AntonioHR.ConditionVariables;
using AntonioHR.TreeAsset;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.Amusi.Data
{

    public enum BarType { FourFour, ThreeFour}
    public class MusicTreeAsset : TreeAsset<MusicTreeNode>
    {

        [HideInInspector]
        public List<ConditionVariable> vars = new List<ConditionVariable>();


        [HideInInspector]
        public List<NoteTrackDefinition> trackDefinitions = new List<NoteTrackDefinition>();

        [HideInInspector]
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
                    cue.Tracks.Add(new NoteTrack() { name = trackDefinitions[i].name, notes = new List<Note>() });
                }
            }

            return result;
        }

        public static void CreateEmpty()
        {
            var tree = ScriptableObject.CreateInstance<MusicTreeAsset>();

            AssetDatabase.CreateAsset(tree, EditorHelper.GetSelectedPathOrFallback() + "/Music Tree.asset");

            AssetDatabase.SaveAssets();
            tree.Init();
        }
    }
}