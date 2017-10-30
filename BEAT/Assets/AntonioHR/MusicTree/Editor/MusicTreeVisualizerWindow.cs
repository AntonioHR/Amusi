using AntonioHR.MusicTree.Nodes;
using AntonioHR.TreeAsset;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.MusicTree.Editor
{
    public class MusicTreeVisualizerWindow : EditorWindow
    {
        public MusicTreeAsset treeFieldValue;

        public Vector2 scrollPos;

        TreeDrawer drawer;

        public static MusicTreeEditorConfigs configs { get; private set; }
        public static string configsAssetName = "MusicTreeEditorConfigs";

        [MenuItem("Window/MusicTree Visualizer")]
        public static void ShowWindow()
        {
            var window = GetWindow<MusicTreeVisualizerWindow>();
            MusicTreeEditorManager.Instance.OnMusicTreeEditorOpened(window);
        }

        private void OnEnable()
        {
            MusicTreeEditorManager.Instance.TreeHierarchyChanged += TreeHierarchyChanged;
            InitializeConfigs();
        }

        private void TreeHierarchyChanged(MusicTree.Internal.PlayableRuntimeMusicTree tree)
        {
            drawer = treeFieldValue != null ? new TreeDrawer(tree) : null;
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
            EditorGUILayout.BeginVertical(BeatSync.Editor.NoteSheetEditorWindow.configs.Skin.box, GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField("ha");
            EditorGUILayout.LabelField("he");
            EditorGUILayout.LabelField("hi");
            EditorGUILayout.EndVertical();
            using (var scrollview = new EditorGUILayout.ScrollViewScope(scrollPos))
            {
                drawer.DrawTree();
                scrollPos = scrollview.scrollPosition;
            }
            EditorGUILayout.EndHorizontal();
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