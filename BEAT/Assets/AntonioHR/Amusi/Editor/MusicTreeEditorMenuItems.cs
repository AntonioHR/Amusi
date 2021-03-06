﻿using AntonioHR.Amusi.Data;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.Amusi.Editor
{
    public static class MusicTreeEditorMenuItems
    {
        public const string Packagename = "AntonioHR.Amusi";
        public const string MonoEventListenerClass = "MonoDancer";

        [MenuItem("Amusi/Create/Music Tree Asset")]
        [MenuItem("Assets/Create/Music Tree")]
        public static void CreateEmptyTree()
        {
            MusicTreeAsset.CreateEmpty();
        }

        [MenuItem("Amusi/Create/Music Tree Player Object")]
        [MenuItem("GameObject/Music Tree Player", priority =10)]
        public static void CreateMusicTreePlayerObject()
        {
            var obj = new GameObject("Music Tree Player");
            obj.AddComponent<AudioSource>();
            obj.AddComponent<AudioSource>();
            obj.AddComponent<MusicTreePlayer>();
        }

        [MenuItem("Amusi/Create/MonoDancer Script")]
        [MenuItem("Assets/Create/MonoDancer")]
        public static void CreateMonoNoteListenerScript()
        {
            AddCSharpClassTemplate("Editor Window", "NewMonoDancer", false,
                  "using UnityEngine;"
                + "\nusing UnityEditor;"
                + string.Format("\nusing {0};", Packagename)
                + "\n"
                + string.Format("\npublic class CLASS_NAME : {0}", MonoEventListenerClass)
                + "\n{"
                + "\n    //Use this instead of the MonoBehaviour Start Function"
                + "\n    protected override void Init()"
                + "\n    {"
                + "\n        // TODO"
                + "\n    }"
                + "\n    //This is Called whenever a note starts playing"
                + "\n    protected override void OnNoteStart()"
                + "\n    {"
                + "\n        // TODO"
                + "\n    }"
                + "\n    //Use this is called every frame while a note plays. "
                + "\n    //Progress goes from 0 to 1 as the note progresses"
                + "\n    protected override void OnNoteUpdate(float progress)"
                + "\n    {"
                + "\n        // TODO"
                + "\n    }"
                + "\n"
                + "\n    protected override void OnNoteEnd()"
                + "\n    {"
                + "\n        // TODO"
                + "\n    }"
                + "\n}");
        }

        #region Helper from Zenject
        static void AddCSharpClassTemplate(
            string friendlyName, string defaultFileName, bool editorOnly, string templateStr)
        {
            var folderPath = EditorHelper.GetCurrentDirectoryAssetPathFromSelection();

            if (editorOnly && !folderPath.Contains("/Editor"))
            {
                EditorUtility.DisplayDialog("Error",
                    "Editor window classes must have a parent folder above them named 'Editor'.  Please create or find an Editor folder and try again", "Ok");
                return;
            }

            var absolutePath = EditorUtility.SaveFilePanel(
                "Choose name for " + friendlyName,
                folderPath,
                defaultFileName + ".cs",
                "cs");

            if (absolutePath == "")
            {
                // Dialog was cancelled
                return;
            }

            if (!absolutePath.ToLower().EndsWith(".cs"))
            {
                absolutePath += ".cs";
            }

            var className = Path.GetFileNameWithoutExtension(absolutePath);
            File.WriteAllText(absolutePath, templateStr.Replace("CLASS_NAME", className));

            AssetDatabase.Refresh();

            var assetPath = EditorHelper.ConvertFullAbsolutePathToAssetPath(absolutePath);

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
        }
        #endregion  

    }
}
