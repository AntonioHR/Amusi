using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AntonioHR.MusicTree.BeatSync;
using AntonioHR.MusicTree.BeatSync.Editor;

namespace AntonioHR.MusicTree.BeatSync.NoteTrackEditor
{
    public class NoteTrackEditor : UnityEditor.EditorWindow
    {
        public static NoteTrackEditorConfigs configs;
        public static NoteTrack noteTrack;

        public static void SetTrack(NoteTrack track)
        {
            noteTrack = track;
        }

        [MenuItem("Window/Note Track Visualizer")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(NoteTrackEditor));
        }
        
        private void OnEnable()
        {
            if (configs == null)
            {
                configs = Resources.Load<NoteTrackEditorConfigs>("NoteTrackEditorConfigs");
                if (configs == null)
                {
                    if (!AssetDatabase.IsValidFolder("Resources"))
                        AssetDatabase.CreateFolder("/", "Resources");
                    configs = ScriptableObject.CreateInstance<NoteTrackEditorConfigs>();
                    AssetDatabase.CreateAsset(configs, "Resources/NoteTrackEditorConfigs");
                    AssetDatabase.SaveAssets();
                }
            }
        }

        void OnGUI()
        {
        }
    }
}