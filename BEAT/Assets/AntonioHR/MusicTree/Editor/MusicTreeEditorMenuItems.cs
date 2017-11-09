using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace AntonioHR.MusicTree.Editor
{
    public static class MusicTreeEditorMenuItems
    {


        [MenuItem("Assets/Create/MonoNote EventListener")]
        public static void CreateMonoNoteListener()
        {
            AddCSharpClassTemplate("Editor Window", "NewMonoNoteEventListener", false,
                  "using UnityEngine;"
                + "\nusing UnityEditor;"
                + "\nusing AntonioHR.MusicTree;"
                + "\nusing AntonioHR.MusicTree.BeatSync;"
                + "\n"
                + "\npublic class CLASS_NAME : MonoNoteEventListener"
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
