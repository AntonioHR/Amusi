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
                drawer.DrawTree();
                scrollPos = scrollview.scrollPosition;
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawSidebar()
        {
            using (var vertScope = new GUILayout.VerticalScope(configs.Skin.window, GUILayout.ExpandHeight(true)))
            {
                toolbarSelection = GUILayout.Toolbar(toolbarSelection, toolbarOptions);
                if (toolbarSelection == 0)
                    DrawVarsEditor();
                else
                    DrawTracksEditor();
            }
        }

        private static void DrawTracksEditor()
        {
            foreach (var trackDef in MusicTreeEditorManager.Instance.TreeAsset.trackDefinitions)
            {
                bool deleted = false;
                DrawTrackDefEditor(trackDef, out deleted);
                if (deleted)
                    break;
            }

            using (var hor = new GUILayout.HorizontalScope(configs.Skin.box))
            {
                tempVarName = GUILayout.TextField(tempVarName);
                EditorGUI.BeginDisabledGroup(tempVarName.Length == 0);
                if (GUILayout.Button("+", GUILayout.Width(20)))
                {
                    CreateTrack();
                }
                EditorGUI.EndDisabledGroup();
            }
        }


        private static void DrawTrackDefEditor(NoteTrackDefinition trackDef, out bool deleted)
        {
            deleted = false;
            using (var hor = new GUILayout.HorizontalScope(configs.Skin.box))
            {
                GUILayout.Label(trackDef.name);
                if (GUILayout.Button("x", GUILayout.Width(20)))
                {
                    DeleteTrack(trackDef);
                    deleted = true;
                }
            }
        }

        private static void CreateTrack()
        {
            MusicTreeEditorManager.Instance.CachedTree.CreateTrack(tempVarName);
            tempVarName = "";
        }

        private static void DeleteTrack(NoteTrackDefinition trackDef)
        {
            MusicTreeEditorManager.Instance.CachedTree.DeleteTrack(trackDef);
        }

        private static void DrawVarsEditor()
        {
            foreach (var treeVar in MusicTreeEditorManager.Instance.TreeAsset.vars)
            {
                bool deletedAny = false;
                DrawVarEditor(treeVar, out deletedAny);
                if (deletedAny)
                    break;
            }


            using (var hor = new GUILayout.HorizontalScope(configs.Skin.box))
            {
                tempVarName = GUILayout.TextField(tempVarName);
                EditorGUI.BeginDisabledGroup(tempVarName.Length == 0);
                if (GUILayout.Button("+", GUILayout.Width(20)))
                {
                    OpenNewVarMenu();
                }
                EditorGUI.EndDisabledGroup();
            }
        }
        private static void DrawVarEditor(ConditionVariables.ConditionVariable treeVar, out bool deletedAny)
        {
            EditorGUILayout.BeginHorizontal(configs.Skin.box);
            GUILayout.Label(treeVar.name);
            GUILayout.FlexibleSpace();
            deletedAny = false;

            var valueDescripton = treeVar.value;
            switch (valueDescripton.type)
            {
                case ConditionVariables.ConditionVariableValue.Type.Integer:
                    valueDescripton.intValue = EditorGUILayout.IntField(valueDescripton.intValue, GUILayout.Width(50));
                    break;
                case ConditionVariables.ConditionVariableValue.Type.Boolean:
                    valueDescripton.boolValue = EditorGUILayout.Toggle(valueDescripton.boolValue, GUILayout.Width(50));
                    break;
                case ConditionVariables.ConditionVariableValue.Type.Float:
                    valueDescripton.floatValue = EditorGUILayout.FloatField(valueDescripton.floatValue, GUILayout.Width(50));
                    break;
                default:
                    break;
            }
            if(GUILayout.Button("x", GUILayout.Width(20)))
            {
                DeleteVar(treeVar);
                deletedAny = true;
            }
            EditorGUILayout.EndHorizontal();
            var varArea = GUILayoutUtility.GetLastRect();

            if(Event.current.button == 1 && varArea.Contains(Event.current.mousePosition))
            {
                OpenVarMenu(treeVar);
            }
        }
        private static void OpenVarMenu(ConditionVariables.ConditionVariable treeVar)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent(string.Format("Delete {0}", treeVar.name)), false, () =>DeleteVar(treeVar));
            menu.ShowAsContext();
        }
        private static void OpenNewVarMenu()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Boolean"), false, () => CreateVar(ConditionVariableValue.Type.Boolean));
            menu.AddItem(new GUIContent("Integer"), false, () => CreateVar(ConditionVariableValue.Type.Integer));
            menu.AddItem(new GUIContent("Float"), false, () => CreateVar(ConditionVariableValue.Type.Float));
            menu.ShowAsContext();
        }

        private static void CreateVar(ConditionVariableValue.Type type)
        {
            var newVar = new ConditionVariable
            {
                name = tempVarName,
                value = new ConditionVariableValue()
                {
                    type = type
                }
            };
            tempVarName = "";
            MusicTreeEditorManager.Instance.TreeAsset.vars.Add(newVar);
        }
        private static void DeleteVar(ConditionVariable treeVar)
        {
            MusicTreeEditorManager.Instance.TreeAsset.vars.Remove(treeVar);
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