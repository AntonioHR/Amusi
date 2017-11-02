using AntonioHR.MusicTree.BeatSync.Editor;
using AntonioHR.MusicTree.Nodes;
using AntonioHR.MusicTree.Editor.Internal;
using AntonioHR.TreeAsset;
using AntonioHR.TreeAsset.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using AntonioHR.MusicTree.Internal;


namespace AntonioHR.MusicTree.Editor.Internal
{
    public class MusicTreeNodePositioning : TreeNodePositioning<MusicTreeNodePositioning, PlayableRuntimeMusicTree, MusicTreeAsset, MusicTreeNode, PlayableRuntimeMusicTreeNode> { }
    public class MusicTreeEditor
    {

        #region Drawing Parameters

        Texture SequenceIcon { get { return MusicTreeEditorWindow.configs.SequenceIcon; } }
        Texture SelectorIcon { get { return MusicTreeEditorWindow.configs.SelectorIcon; } }
        Texture CueIcon { get { return MusicTreeEditorWindow.configs.CueIcon; } }
        Texture ConditionIcon { get { return MusicTreeEditorWindow.configs.ConditionIcon; } }
        #endregion


        PlayableRuntimeMusicTree tree;
        MusicTreeNodePositioning cachedPositioning;
        
        public MusicTreeEditor(PlayableRuntimeMusicTree tree)
        {
            this.tree = tree;
            
            cachedPositioning = MusicTreeNodePositioning.CreateFrom(tree);
        }
        
        public void DrawTree()
        {

            ReserveLayoutSpace();

            DrawNodes();
        }

        private void DrawNodes()
        {
            foreach (var node in tree.AllNodes)
            {
                if (!node.IsRoot)
                {
                    DrawLineToParent(node, node.Asset);
                }
            }

            foreach (var node in tree.AllNodes)
            {
                DrawNode(cachedPositioning.GetBoundsFor(node), node);
            }
        }

        private void DrawLineToParent(PlayableRuntimeMusicTreeNode node, MusicTreeNode musicTreeNode)
        {
            var parentBounds = cachedPositioning.GetBoundsFor(node.Parent);
            var myBounds = cachedPositioning.GetBoundsFor(node);

            Handles.DrawLine(parentBounds.center, myBounds.center);
        }


        private void DrawNode(Rect bounds, PlayableRuntimeMusicTreeNode node)
        {
            Color color = Color.gray;

            Texture tex = null;

            if (node.Asset is CueMusicTreeNode)
            {
                //color = Color.red;
                tex = CueIcon;
            }
            else if (node.Asset is SelectorMusicTreeNode)
            {
                //color = Color.green;
                tex = SelectorIcon;
            }
            else if (node.Asset is SequenceMusicTreeNode)
            {
                //color = Color.cyan;
                tex = SequenceIcon;
            } else if(node.Asset is ConditionMusicTreeNode)
            {
                tex = ConditionIcon;
            }


            EditorGUI.DrawRect(bounds, color);
            if (tex != null)
                GUI.DrawTexture(bounds.Resized(Vector2.one * 1), tex);

            if (GUI.Button(bounds, GUIContent.none, GUIStyle.none))
            {
                MusicTreeEditorManager.Instance.OnNodeSelected(node);
            }
        }


        private void ReserveLayoutSpace()
        {
            Vector2 size = cachedPositioning.GetTreeSize();
            GUILayoutUtility.GetRect(size.x, size.y);
        }

    }
}
