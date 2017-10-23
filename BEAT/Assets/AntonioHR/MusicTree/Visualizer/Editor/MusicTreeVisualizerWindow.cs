using AntonioHR.MusicTree.Nodes;
using AntonioHR.TreeAsset;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.MusicTree.Visualizer.Editor
{
    public class MusicTreeVisualizerWindow : EditorWindow
    {
        public MusicTreeAsset tree;

        public Vector2 scrollPos;

        TreeDrawer drawer;

        [MenuItem("Window/MusicTree Visualizer")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(MusicTreeVisualizerWindow));
        }

        void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            tree = EditorGUILayout.ObjectField(tree, typeof(MusicTreeAsset), false) as MusicTreeAsset;
            if (EditorGUI.EndChangeCheck())
            {
                //Changed
                drawer = tree != null? new TreeDrawer(tree): null;
            }


            using(var scrollview = new EditorGUILayout.ScrollViewScope(scrollPos))
            {
                if(drawer != null)
                {
                    drawer.DrawTree();
                }
                scrollPos = scrollview.scrollPosition;
            }
        }
    }
}