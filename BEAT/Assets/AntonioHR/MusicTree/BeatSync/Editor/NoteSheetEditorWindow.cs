using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AntonioHR.MusicTree.BeatSync;
using AntonioHR.MusicTree.BeatSync.Editor;

namespace AntonioHR.MusicTree.BeatSync.Editor
{
    public class NoteSheetEditorWindow : UnityEditor.EditorWindow
    {
        public static NoteSheetEditorConfigs configs;
        public static string configsAssetName = "NoteTrackEditorConfigs";

        public static NoteSheetEditorWindow Instance { get; private set; }

        static NoteSheetDrawer drawer;
        static NoteSheet noteSheet;

        public static void SetSheet(NoteSheet sheet)
        {
            noteSheet = sheet;
            drawer = new NoteSheetDrawer(sheet, 4);
            if (Instance != null)
                Instance.Repaint();
        }

        [MenuItem("Window/Note Sheet Editor")]
        public static void ShowWindow()
        {
            Instance = GetWindow<NoteSheetEditorWindow>(typeof(NoteSheetEditorWindow));
        }
        
        private void OnEnable()
        {
            InitializeConfigs();
        }

        private static void InitializeConfigs()
        {
            if (configs == null)
            {
                configs = Resources.Load<NoteSheetEditorConfigs>(string.Format("MusicTree/{0}", configsAssetName));
                if (configs == null)
                {
                    if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                        AssetDatabase.CreateFolder("Assets/", "Resources");
                    if (!AssetDatabase.IsValidFolder("Assets/Resources/MusicTree"))
                        AssetDatabase.CreateFolder("Assets/Resources", "MusicTree");
                    configs = ScriptableObject.CreateInstance<NoteSheetEditorConfigs>();
                    AssetDatabase.CreateAsset(configs, string.Format("Assets/Resources/MusicTree/{0}.asset",configsAssetName));
                    AssetDatabase.SaveAssets();
                }
            }
        }

        void OnGUI()
        {
            if (drawer != null)
                drawer.Draw();
        }
    }
}