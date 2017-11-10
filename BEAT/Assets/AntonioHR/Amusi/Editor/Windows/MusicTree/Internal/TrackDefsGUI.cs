using AntonioHR.Amusi.Data;
using AntonioHR.Amusi.Internal;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.Amusi.Editor.Windows.MusicTree.Internal
{
    public static class TrackDefsGUI
    {
        public static string tempVarName = "";

        public static void DrawTracksEditor()
        {
            foreach (var trackDef in MusicTreeEditorManager.Instance.TreeAsset.trackDefinitions)
            {
                bool deleted = false;
                DrawTrackDefEditor(trackDef, out deleted);
                if (deleted)
                    break;
            }

            using (var hor = new GUILayout.HorizontalScope(MusicTreeEditorWindow.configs.Skin.box))
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
            using (var hor = new GUILayout.HorizontalScope(MusicTreeEditorWindow.configs.Skin.box))
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

    }
}
