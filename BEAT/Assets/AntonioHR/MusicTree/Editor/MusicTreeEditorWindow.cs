using AntonioHR.MusicTree.Nodes;
using AntonioHR.TreeAsset;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using AntonioHR.MusicTree.Editor.Internal;
using AntonioHR.MusicTree.ConditionVariables;
using System;
using AntonioHR.MusicTree.Internal;

namespace AntonioHR.MusicTree.Editor
{
    public class MusicTreeEditorWindow : EditorWindow
    {
        public MusicTreeAsset treeFieldValue;

        public Vector2 scrollPos;
        
        MusicTree.Editor.Internal.MusicTreeEditor drawer;

        public static MusicTreeEditorConfigs configs { get; private set; }
        public static string configsAssetName = "MusicTreeEditorConfigs";
        public static string[] toolbarOptions = { "Variables", "Tracks" };

        public static string tempVarName = "";
        public static int toolbarSelection = 0;

        [MenuItem("Window/MusicTree Visualizer")]
        public static void ShowWindow()
        {
            var window = GetWindow<MusicTreeEditorWindow>();
            MusicTreeEditorManager.Instance.OnMusicTreeEditorOpened(window);
        }

        private void OnEnable()
        {
            MusicTreeEditorManager.Instance.TreeHierarchyChanged += TreeHierarchyChanged;
            InitializeConfigs();
        }

        private void TreeHierarchyChanged(MusicTree.Internal.PlayableRuntimeMusicTree tree)
        {
            drawer = treeFieldValue != null ? new MusicTreeEditor(tree) : null;
        }
        void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            treeFieldValue = EditorGUILayout.ObjectField(treeFieldValue, typeof(MusicTreeAsset), false) as MusicTreeAsset;
            if (EditorGUI.EndChangeCheck())
            {
                MusicTreeEditorManager.Instance.OnSelectedTreeChanged(treeFieldValue);
            }

            if (drawer == null)
                return;

            EditorGUILayout.BeginHorizontal();
            DrawSidebar();

            using (var scrollview = new EditorGUILayout.ScrollViewScope(scrollPos))
            {
                drawer.Update();
                scrollPos = scrollview.scrollPosition;
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawSidebar()
        {
            using (var vertScope = new GUILayout.VerticalScope(configs.Skin.window, GUILayout.ExpandHeight(true), GUILayout.MaxWidth(200)))
            {
                toolbarSelection = GUILayout.Toolbar(toolbarSelection, toolbarOptions);
                if (toolbarSelection == 0)
                    VarsGUI.DrawVarsEditor();
                else
                    TrackDefsGUI.DrawTracksEditor();
            }
        }


        private static void InitializeConfigs()
        {
            if (configs == null)
            {
                configs = Resources.Load<MusicTreeEditorConfigs>(string.Format("MusicTree/{0}", configsAssetName));
                if (configs == null)
                {
                    if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                        AssetDatabase.CreateFolder("Assets/", "Resources");
                    if (!AssetDatabase.IsValidFolder("Assets/Resources/MusicTree"))
                        AssetDatabase.CreateFolder("Assets/Resources", "MusicTree");
                    configs = ScriptableObject.CreateInstance<MusicTreeEditorConfigs>();
                    AssetDatabase.CreateAsset(configs, string.Format("Assets/Resources/MusicTree/{0}.asset", configsAssetName));
                    AssetDatabase.SaveAssets();
                }
            }
        }
    }
}