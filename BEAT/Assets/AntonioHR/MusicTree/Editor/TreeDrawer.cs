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

namespace AntonioHR.MusicTree.Editor
{
    public class TreeDrawer
    {

        public MusicTreeAsset tree;

        #region Drawing Parameters

        Texture sequence_icon;
        Texture selector_icon;
        Texture cue_icon;
        Texture condition_icon;
        #endregion
        

        RuntimeTree<MusicTreeNode> cachedTree;
        TreeNodePositioning<MusicTreeNode> cachedPositioning;
        
        public TreeDrawer(MusicTreeAsset tree)
        {
            this.tree = tree;

            UpdateTreeCache();


            selector_icon = Resources.Load<Texture>("icon_selector");
            sequence_icon = Resources.Load<Texture>("icon_sequence");
            cue_icon = Resources.Load<Texture>("icon_music");
            condition_icon = Resources.Load<Texture>("icon_condition");
        }

        void UpdateTreeCache()
        {

            cachedTree = RuntimeTree<MusicTreeNode>.CreateFrom(tree);
            cachedPositioning = TreeNodePositioning<MusicTreeNode>.CreateFrom(cachedTree);
        }

        public void DrawTree()
        {

            ReserveLayoutSpace();

            DrawNodes();
        }

        private void DrawNodes()
        {
            foreach (var node in cachedTree.AllNodes)
            {
                if (!node.IsRoot)
                {
                    DrawLineToParent(node, node.Asset);
                }
            }

            foreach (var node in cachedTree.AllNodes)
            {
                DrawNode(cachedPositioning.GetBoundsFor(node), node.Asset);

            }
        }

        private void DrawLineToParent(RuntimeTreeNode<MusicTreeNode> node, MusicTreeNode musicTreeNode)
        {
            var parentBounds = cachedPositioning.GetBoundsFor(node.Parent);
            var myBounds = cachedPositioning.GetBoundsFor(node);

            Handles.DrawLine(parentBounds.center, myBounds.center);
        }


        private void DrawNode(Rect bounds, TreeNodeAsset node)
        {
            Color color = Color.gray;

            Texture tex = null;

            if (node is CueMusicTreeNode)
            {
                //color = Color.red;
                tex = cue_icon;
            }
            else if (node is SelectorMusicTreeNode)
            {
                //color = Color.green;
                tex = selector_icon;
            }
            else if (node is SequenceMusicTreeNode)
            {
                //color = Color.cyan;
                tex = sequence_icon;
            } else if(node is ConditionMusicTreeNode)
            {
                tex = condition_icon;
            }


            EditorGUI.DrawRect(bounds, color);
            if (tex != null)
                GUI.DrawTexture(bounds.Resized(Vector2.one * 1), tex);

            if (GUI.Button(bounds, GUIContent.none, GUIStyle.none))
            {
                Selection.activeObject = node;
                var cue = node as CueMusicTreeNode;
                if(cue != null)
                {
                    MusicTreeEditorManager.Instance.OnNodeSelected((MusicTreeNode)node);
                }
            }
        }


        private void ReserveLayoutSpace()
        {
            Vector2 size = cachedPositioning.GetTreeSize();
            GUILayoutUtility.GetRect(size.x, size.y);
        }

    }
}
