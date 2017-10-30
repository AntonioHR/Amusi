using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AntonioHR.MusicTree.BeatSync;
using AntonioHR.MusicTree.BeatSync.Editor;
using AntonioHR.MusicTree.Editor;
using AntonioHR.MusicTree.Nodes;
using AntonioHR.MusicTree.Internal;

namespace AntonioHR.MusicTree.BeatSync.Editor
{
    public class NoteSheetEditorWindow : UnityEditor.EditorWindow
    {
        public static NoteSheetEditorConfigs configs;
        public static string configsAssetName = "NoteTrackEditorConfigs";
        

        private NoteSheetDrawer drawer;
        private bool repaintRequired;


        [MenuItem("Window/Note Sheet Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<NoteSheetEditorWindow>();
            MusicTreeEditorManager.Instance.OnNoteSheetEditorOpened(window);
        }
        
        private void OnEnable()
        {
            InitializeConfigs();
            MusicTreeEditorManager.Instance.SelectedCueChanged += Manager_SelectedCueChanged;
        }
        private void OnDestroy()
        {

        }
        void OnGUI()
        {
            if (drawer != null)
                drawer.Update(position);
            if (repaintRequired)
            {
                repaintRequired = false;
                Repaint();
            }
        }
        public void RequireRepaint()
        {
            repaintRequired = true;
        }



        private void Manager_SelectedCueChanged(CueMusicTreeNode node, PlayableRuntimeMusicTreeNode owner)
        {
            UpdateDrawer(node, owner);
        }

        private void UpdateDrawer(CueMusicTreeNode cue, PlayableRuntimeMusicTreeNode owner)
        {
            if (drawer != null)
            {
                drawer.DataUpdated -= Repaint;
                drawer.OnReplaced();
            }
            drawer = new NoteSheetDrawer(cue, owner);
            drawer.DataUpdated += Repaint;
            Repaint();
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

    }
}