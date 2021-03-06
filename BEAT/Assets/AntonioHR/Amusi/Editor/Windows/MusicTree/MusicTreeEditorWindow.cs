﻿using AntonioHR.Amusi.Data;
using AntonioHR.Amusi.Editor.Windows.MusicTree.Internal;
using System;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.Amusi.Editor.Windows.MusicTree
{
    public class MusicTreeEditorWindow : EditorWindow
    {
        public MusicTreeAsset treeFieldValue;

        public Vector2 scrollPos;
        
        MusicTreeEditor drawer;

        public static MusicTreeEditorConfigs configs { get; private set; }
        public static string configsAssetName = "MusicTreeEditorConfigs";
        public static string[] toolbarOptions = { "Tree", "Variables", "Tracks"};

        public static string tempVarName = "";
        public static int toolbarSelection = 0;


        [MenuItem("Amusi/Window/Music Tree Editor")]
        [MenuItem("Window/Amusi/Music Tree Editor")]
        public static void ShowWindow()
        {
            GetWindow<MusicTreeEditorWindow>();
        }

        private void OnEnable()
        {
            MusicTreeEditorManager.Instance.OnMusicTreeEditorOpened(this);
            MusicTreeEditorManager.Instance.TreeHierarchyChanged += TreeHierarchyChanged;
            InitializeConfigs();
        }

        private void TreeHierarchyChanged(Amusi.Internal.CachedMusicTree tree)
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
                    DrawOptionsToolbar();
                else if (toolbarSelection == 1)
                    VarsGUI.DrawVarsEditor();
                else
                    TrackDefsGUI.DrawTracksEditor();
            }
        }

        private static void DrawOptionsToolbar()
        {
            var ta = MusicTreeEditorManager.Instance.TreeAsset;
            ta.defaultBPM = EditorGUILayout.IntField("Music BPM", ta.defaultBPM);
            ta.barType = (BarType) EditorGUILayout.EnumPopup("Measures type", (Enum) ta.barType);
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