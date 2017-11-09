using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using AntonioHR.Amusi.Data.Nodes;
using AntonioHR.Amusi.Editor.Window.NoteSheet.Internal;
using AntonioHR.Amusi.Internal;

namespace AntonioHR.Amusi.Editor.Window.NoteSheet
{
    public class NoteSheetEditorWindow : UnityEditor.EditorWindow
    {
        public static NoteSheetEditorConfigs configs;
        public static string configsAssetName = "NoteTrackEditorConfigs";
        

        private NoteSheetEditor drawer;
        private bool repaintRequired;


        [MenuItem("Amusi/Window/Note Sheet Editor")]
        [MenuItem("Window/Amusi/Note Sheet Editor")]
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



        private void Manager_SelectedCueChanged(CueMusicTreeNode node, CachedMusicTreeNode owner)
        {
            UpdateDrawer(node, owner);
        }

        private void UpdateDrawer(CueMusicTreeNode cue, CachedMusicTreeNode owner)
        {
            if (drawer != null)
            {
                drawer.DataUpdated -= Repaint;
                drawer.OnReplaced();
            }
            drawer = new NoteSheetEditor(cue, owner);
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